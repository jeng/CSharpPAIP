#To include tracing information pass -d:TRACE as the cflag
#> make CFLAGS='-d:TRACE'
CC = dmcs 

exes = GeneralProblemSolver.exe GeneralProblemSolverV2.exe \
	   MonkeyProblem.exe MazeProblem.exe StackPermute.exe \
	   Permute.exe BlockProblem.exe DessertProblem.exe

all: $(exes)

GeneralProblemSolver.exe: GeneralProblemSolver.cs GPS.cs Op.cs
	$(CC) $(CFLAGS) $^

GeneralProblemSolverV2.exe: GeneralProblemSolver2.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs CLExt.cs
	$(CC) $(CFLAGS) $^ -out:$@

MonkeyProblem.exe: MonkeyProblem.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs CLExt.cs
	$(CC) $(CFLAGS) $^

MazeProblem.exe: MazeProblem.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs CLExt.cs
	$(CC) $(CFLAGS) $^ 

StackPermute.exe: StackPermute.cs
	$(CC) $(CFLAGS) $^

Permute.exe: Permute.cs
	$(CC) $(CFLAGS) $^

BlockProblem.exe: BlockProblem.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs CLExt.cs
	$(CC) $(CFLAGS) $^

DessertProblem.exe: DessertProblem.cs GPS2.cs Op2.cs OpAction.cs ActionState.cs CLExt.cs
	$(CC) $(CFLAGS) $^


.PHONY: clean
clean:
	rm $(exes)
