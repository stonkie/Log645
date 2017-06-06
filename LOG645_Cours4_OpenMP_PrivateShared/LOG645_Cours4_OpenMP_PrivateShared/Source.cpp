#include <cstdlib>
#include <stdio.h>
#include <omp.h>
#include <chrono>
#include <thread>

int main(int argc, char ** argv) {
	
#pragma omp parallel//  default(private) // private(rang, nprocs)
	{
		int rang = 0;
		int nprocs = 0;

		rang = omp_get_thread_num();
		nprocs = omp_get_num_threads();

		printf("Bonjour, je suis %d(parmi %d threads)\n", rang, nprocs);
	}

	system("pause");
	return EXIT_SUCCESS;
}