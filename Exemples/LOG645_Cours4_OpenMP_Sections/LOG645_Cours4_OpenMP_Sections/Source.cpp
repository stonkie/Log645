#include <cstdlib>
#include <chrono>
#include <thread>

int main(int argc, char* argv[])
{
	long long startTime = _Query_perf_counter();

#pragma omp parallel sections
	{
#pragma omp section 
		{
			std::this_thread::sleep_for(std::chrono::seconds(1));
		}

#pragma omp section 
		{
			std::this_thread::sleep_for(std::chrono::seconds(1));
		}

#pragma omp section 
		{
			std::this_thread::sleep_for(std::chrono::seconds(1));
		}

#pragma omp section 
		{
			std::this_thread::sleep_for(std::chrono::seconds(1));
		}
	}

	long long endTime = _Query_perf_counter();

	printf("Time : %lld \n", endTime - startTime);

	system("pause");
	return EXIT_SUCCESS;
}
