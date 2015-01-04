#To include tracing information pass -d:TRACE as the cflag
#> make CFLAGS='-d:TRACE'
CC = dmcs 

all: GeneralProblemSolver GeneralProblemSolverV2 MonkeyProblem

GeneralProblemSolver:
	$(CC) $(CFLAGS) GeneralProblemSolver.cs GPS.cs Op.cs

GeneralProblemSolverV2:
	$(CC) $(CFLAGS) GeneralProblemSolver2.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs -out:$@.exe

MonkeyProblem:
	$(CC) $(CFLAGS) MonkeyProblem.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs

.PHONY: clean
clean:
	rm *.exe
