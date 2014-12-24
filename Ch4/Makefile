#To include tracing information pass -d:TRACE as the cflag
#> make CFLAGS='-d:TRACE'
CC = dmcs 

all: GeneralProblemSolver

GeneralProblemSolver:
	$(CC) $(CFLAGS) GeneralProblemSolver.cs GPS.cs Op.cs

.PHONY: clean
clean:
	rm *.exe
