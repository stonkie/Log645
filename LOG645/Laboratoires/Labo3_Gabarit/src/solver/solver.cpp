#include <chrono>
#include <cstring>
#include <thread>

#include <mpi.h>

#include "solver.hpp"
#include "../matrix/matrix.hpp"

using std::memcpy;

using std::this_thread::sleep_for;
using std::chrono::microseconds;

void solveSeq(int rows, int cols, int iterations, double td, double h, int sleep, double ** matrix) {
    double c, l, r, t, b;
    
    double h_square = h * h;

    double * linePrevBuffer = new double[cols];
    double * lineCurrBuffer = new double[cols];

    for(int k = 0; k < iterations; k++) {

        memcpy(linePrevBuffer, matrix[0], cols * sizeof(double));
        for(int i = 1; i < rows - 1; i++) {

            memcpy(lineCurrBuffer, matrix[i], cols * sizeof(double));
            for(int j = 1; j < cols - 1; j++) {
                c = lineCurrBuffer[j];
                t = linePrevBuffer[j];
                b = matrix[i + 1][j];
                l = lineCurrBuffer[j - 1];
                r = lineCurrBuffer[j + 1];


                sleep_for(microseconds(sleep));
                matrix[i][j] = c * (1.0 - 4.0 * td / h_square) + (t + b + l + r) * (td / h_square);
            }

            memcpy(linePrevBuffer, lineCurrBuffer, cols * sizeof(double));
        }
    }
}

void solvePar(int rows, int cols, int iterations, double td, double h, int sleep, double ** matrix) {

    sleep_for(microseconds(500000));
}
