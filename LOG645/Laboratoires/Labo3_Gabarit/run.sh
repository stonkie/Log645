make all

echo ""
echo "<<<"
mpirun -np 2 ./lab3 9 9 360 0.00025 0.1
echo ">>>"

echo ""
echo "<<<"
mpirun -np 2 ./lab3 5 9 300 0.01 1
echo ">>>"

make clean
