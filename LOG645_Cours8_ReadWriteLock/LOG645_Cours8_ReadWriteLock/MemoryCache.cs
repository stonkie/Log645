using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOG645_Cours8_ReadWriteLock
{
    public class MemoryCache
    {
        private class DataStore
        {
            public int BlockSize { get; }
            public ReaderWriterLockSlim[] BlockLocks { get; set; }
            public ReaderWriterLockSlim BufferLengthLock { get; } = new ReaderWriterLockSlim();
            public byte[] Buffer { get; set; }

            public DataStore(int size, int blockSize)
            {
                Buffer = new byte[size];
                BlockLocks = new ReaderWriterLockSlim[(int) Math.Ceiling((size * 1.0) / blockSize)];
                BlockSize = blockSize;

                for (int newBlockIndex = 0; newBlockIndex < BlockLocks.Length; newBlockIndex++)
                {
                    BlockLocks[newBlockIndex] = new ReaderWriterLockSlim();
                }

            }
        }

        private class CacheStream : Stream
        {
            private readonly DataStore _store;
            private long _currentOffset = 0;

            public CacheStream(DataStore store)
            {
                _store = store;
            }

            public override void Flush()
            {
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                _store.BufferLengthLock.EnterReadLock();

                try
                {
                    switch (origin)
                    {
                        case SeekOrigin.Begin:
                            if (offset >= _store.Buffer.Length || offset < 0)
                            {
                                throw new ArgumentOutOfRangeException(nameof(offset),
                                    "The offset is outside of the current buffer.");
                            }

                            _currentOffset = offset;
                            break;
                        case SeekOrigin.Current:
                            if (offset + _currentOffset >= _store.Buffer.Length || offset + _currentOffset < 0)
                            {
                                throw new ArgumentOutOfRangeException(nameof(offset),
                                    "The offset is outside of the current buffer.");
                            }

                            _currentOffset += offset;
                            break;
                        case SeekOrigin.End:
                            if (_store.Buffer.Length - offset >= _store.Buffer.Length ||
                                _store.Buffer.Length - offset < 0)
                            {
                                throw new ArgumentOutOfRangeException(nameof(offset),
                                    "The offset is outside of the current buffer.");
                            }

                            _currentOffset = _store.Buffer.Length - offset;
                            break;
                    }
                }
                finally
                {
                    _store.BufferLengthLock.ExitReadLock();
                }

                return _currentOffset;
            }

            public override void SetLength(long value)
            {
                int blockLockIndex = 0;
                _store.BufferLengthLock.EnterWriteLock();
                
                try
                {   
                    for (; blockLockIndex < _store.BlockLocks.Length; blockLockIndex++)
                    {
                        _store.BlockLocks[blockLockIndex].EnterWriteLock();
                    }
                    

                    byte[] newBuffer = new byte[value];

                    long copySize = _store.Buffer.Length < value ? _store.Buffer.Length : value;

                    Buffer.BlockCopy(_store.Buffer, 0, newBuffer, 0, (int) copySize);

                    _store.Buffer = newBuffer;

                    ReaderWriterLockSlim[] newBlockLocks = new ReaderWriterLockSlim[(int)Math.Ceiling(value * 1.0 / _store.BlockSize)];

                    int copyBlockLockSize = _store.BlockLocks.Length < newBlockLocks.Length
                        ? _store.BlockLocks.Length
                        : newBlockLocks.Length;

                    Array.Copy(_store.BlockLocks, newBlockLocks, copyBlockLockSize);

                    for (int newBlockIndex = _store.BlockLocks.Length; newBlockIndex < newBlockLocks.Length; newBlockIndex++)
                    {
                        newBlockLocks[newBlockIndex] = new ReaderWriterLockSlim();
                    }

                    _store.BlockLocks = newBlockLocks;


                }
                finally
                {
                    for (; blockLockIndex >= 0; blockLockIndex--)
                    {
                        _store.BlockLocks[blockLockIndex].ExitWriteLock();
                    }

                    _store.BufferLengthLock.ExitWriteLock();
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (count < 0 || count > buffer.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(count), "The count is either below zero or larger than the size of the source buffer.");
                }

                _store.BufferLengthLock.EnterReadLock();

                int startBlock = 0;
                int endBlock = -1;

                try
                {
                    if (offset < 0 || offset + count >= _store.Buffer.Length)
                    {
                        throw new ArgumentOutOfRangeException(nameof(offset), "The offset is either below zero or added with count, larger than the size of the data store.");
                    }
                    
                    startBlock = offset / _store.BlockSize;
                    endBlock = (offset + count) / _store.BlockSize;

                    for (int blockIndex = startBlock; blockIndex <= endBlock; blockIndex++)
                    {
                        _store.BlockLocks[blockIndex].EnterReadLock();   
                    }

                    Buffer.BlockCopy(_store.Buffer, offset, buffer, 0, count);

                    return count;
                }
                finally
                {
                    for (int blockIndex = startBlock; blockIndex <= endBlock; blockIndex++)
                    {
                        _store.BlockLocks[blockIndex].ExitReadLock();
                    }

                    _store.BufferLengthLock.ExitReadLock();
                }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (count < 0 || count > buffer.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(count), "The count is either below zero or larger than the size of the source buffer.");
                }

                _store.BufferLengthLock.EnterReadLock();

                int startBlock = 0;
                int endBlock = -1;

                try
                {
                    if (offset < 0 || offset + count >= _store.Buffer.Length)
                    {
                        throw new ArgumentOutOfRangeException(nameof(offset), "The offset is either below zero or added with count, larger than the size of the data store.");
                    }

                    startBlock = offset / _store.BlockSize;
                    endBlock = (offset + count) / _store.BlockSize;

                    for (int blockIndex = startBlock; blockIndex <= endBlock; blockIndex++)
                    {
                        _store.BlockLocks[blockIndex].EnterWriteLock();
                    }

                    Buffer.BlockCopy(buffer, 0, _store.Buffer, offset, count);
                }
                finally
                {
                    for (int blockIndex = startBlock; blockIndex <= endBlock; blockIndex++)
                    {
                        _store.BlockLocks[blockIndex].ExitWriteLock();
                    }

                    _store.BufferLengthLock.ExitReadLock();
                }
            }

            public override bool CanRead => true;

            public override bool CanSeek => true;
            public override bool CanWrite => true;

            public override long Length
            {
                get
                {
                    _store.BufferLengthLock.EnterReadLock();
                    try
                    {
                        return _store.Buffer.Length;
                    }
                    finally
                    {
                        _store.BufferLengthLock.ExitReadLock();
                    }
                }
            }

            public override long Position
            {
                get => _currentOffset;
                set => Seek(value, SeekOrigin.Begin);
            }
        }

        private readonly DataStore _store;

        public MemoryCache(int size, int blockSize)
        {
            _store = new DataStore(size, blockSize);
        }

        public Stream OpenStream()
        {
            return new CacheStream(_store);
        }
    }
}