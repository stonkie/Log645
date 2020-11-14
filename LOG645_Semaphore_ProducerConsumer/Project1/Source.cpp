#include <cstdio>
#include <cmath>
#include <ctime>
#include <omp.h>
#include "semaphore.h"
#include <thread>

int main(int argc, char** argv)
{
	srand(time(NULL));
	int queue[5];

	sem_t semLibre;
	sem_init(&semLibre, 0, 5);

	sem_t semPlein;
	sem_init(&semPlein, 0, 0);

#pragma omp parallel sections
	{
#pragma omp section
		{
			int producteurIndex = 0;

			//Producteur
			for (int i = 0; i < 1000; i++)
			{
				sem_wait(&semLibre);
				queue[producteurIndex % 5] = rand() % 100 + 1;
				producteurIndex++;
				sem_post(&semPlein);
			}
		}

#pragma omp section
		{
			int consommateurIndex = 0;
			//Consommateur
			for (int i = 0; i < 1000; i++)
			{
				sem_post(&semPlein);
				printf("%d : valeur = %d \n", i, queue[consommateurIndex % 5]);
				consommateurIndex++;
				sem_wait(&semLibre);
			}
		}
	}

	printf("Fin \n");
	getchar();

	return EXIT_SUCCESS;
}
