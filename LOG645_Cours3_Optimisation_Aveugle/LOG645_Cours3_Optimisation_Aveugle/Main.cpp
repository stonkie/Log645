#include <cstdlib>
#include <chrono>

int main(int argc, char* argv[])
{
	const int size = 100000000;

	long long startTime = _Query_perf_counter();
	
    int* values = static_cast<int*>(malloc(sizeof(int) * size));

	#pragma omp parallel for
	for (int i = 0; i < size; i++)
	{
		values[i] = i;
	}

	long long endTime = _Query_perf_counter();

	printf("Time : %lld \n", endTime - startTime);
	printf("Value is %d \n", values[12123]);

	system("pause");
	return EXIT_SUCCESS;
}
