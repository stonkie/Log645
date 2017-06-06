#include <cstdlib>
#include <chrono>

int main(int argc, char* argv[])
{
	long long startTime = _Query_perf_counter();

#pragma omp parallel 
	{
		printf("Hello World2!\n");
		printf("Hello World!\n");
	}
	long long endTime = _Query_perf_counter();

	printf("Time : %lld \n", endTime - startTime);

	system("pause");
	return EXIT_SUCCESS;
}
