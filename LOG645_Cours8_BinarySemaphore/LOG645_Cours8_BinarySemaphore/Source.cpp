#include <cstdlib>
#include <memory>
#include <filesystem>
#define _TIMESPEC_DEFINED
#include <pthread.h>
#include <semaphore.h>
#include <thread>

pthread_t StartFileAccessThread();
void *CopyFile(void *parameters);

int main(int argc, char* argv[])
{
	pthread_t thread = StartFileAccessThread();

	void *returnValue;
	pthread_join(thread, &returnValue);

	getchar();
	return EXIT_SUCCESS;
}


pthread_t StartFileAccessThread()
{
	std::string sourcePath("source.txt");
	std::string destinationPath("dest.txt");
	sem_t semaphore;
	sem_init(&semaphore, 0, 0);
	pthread_t thread;

	void *parameters[3];
	parameters[0] = &semaphore;
	parameters[1] = &sourcePath;
	parameters[2] = &destinationPath;

	pthread_create(&thread, nullptr, &CopyFile, parameters);

	// sem_wait(&semaphore);
	// sem_destroy(&semaphore);

	printf("Freeing ressources.\n");

	return thread;
}


void *CopyFile(void *rawParameter)
{
	void **parameters = static_cast<void **>(rawParameter);

	sem_t *semaphore = static_cast<sem_t *>(parameters[0]);
	std::string sourcePath(*static_cast<std::string *>(parameters[1]));
	std::string destinationPath(*static_cast<std::string *>(parameters[2]));

	sem_post(semaphore);

	std::this_thread::sleep_for(std::chrono::seconds(2));

	copy_file(sourcePath, destinationPath, std::experimental::filesystem::copy_options::overwrite_existing);

	printf("File copied \n");

	return nullptr;
}


