#include <cstdlib>
#include <chrono>

int main(int argc, char* argv[])
{
	int x = 0;
	const int size = 100;

	long long startTime = _Query_perf_counter();

	#pragma omp parallel for
	for (int i = 0; i < size; i++)
	{
			printf("Test %d\n", i);
	}

	long long endTime = _Query_perf_counter();

	printf("Time : %lld \n", endTime - startTime);
	
	system("pause");
	return EXIT_SUCCESS;
}
