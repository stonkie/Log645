#ifndef MATRIX_HPP
#define MATRIX_HPP

double ** allocateMatrix(int rows, int cols);
void deallocateMatrix(int rows, double ** matrix);

void fillMatrix(int rows, int cols, double ** matrix);

#endif
