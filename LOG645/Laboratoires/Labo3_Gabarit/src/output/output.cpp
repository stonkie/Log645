#include <iomanip>
#include <iostream>
#include <chrono>

#include "output.hpp"

using namespace std::chrono;

using std::cout;
using std::endl;
using std::fixed;
using std::flush;
using std::setprecision;
using std::setw;

void printMatrix(int rows, int cols, double ** matrix) {
    for(int row = 0; row < rows; row++) {
        for(int col = 0; col < cols; col++) {
            cout << fixed << setw(12) << setprecision(2) << matrix[row][col] << flush;
        }

        cout << endl << flush;
    }

    cout << endl << flush;
}

void printStatistics(int threads, long runtime_seq, long runtime_par) {
    double acceleration = 1.0 * runtime_seq / runtime_par;
    double efficiency = acceleration / threads;

    cout << "Runtime sequential: " << runtime_seq / 1000000.0 << " seconds" << endl << flush;
    cout << "Runtime parallel  : " << runtime_par / 1000000.0 << " seconds" << endl << flush;
    cout << "Acceleration      : " << acceleration << endl << flush;
    cout << "Efficiency        : " << efficiency << endl << flush;
}
