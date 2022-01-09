#ifndef SOLVER_HPP
#define SOLVER_HPP

void solveSeq(int rows, int cols, int iterations, double td, double h, int sleep, double ** matrix);
void solvePar(int rows, int cols, int iterations, double td, double h, int sleep, double ** matrix);

#endif
