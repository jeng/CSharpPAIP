#To include tracing information pass -d:TRACE as the cflag
#> make CFLAGS='-d:TRACE'
CC = dmcs 

exes = GeneralProblemSolver.exe GeneralProblemSolverV2.exe MonkeyProblem.exe MazeProblem.exe

all: $(exes)

GeneralProblemSolver.exe:
	$(CC) $(CFLAGS) GeneralProblemSolver.cs GPS.cs Op.cs

GeneralProblemSolverV2.exe:
	$(CC) $(CFLAGS) GeneralProblemSolver2.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs -out:$@

MonkeyProblem.exe:
	$(CC) $(CFLAGS) MonkeyProblem.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs

MazeProblem.exe:
	$(CC) $(CFLAGS) MazeProblem.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs


.PHONY: clean
clean:
	rm $(exes)
