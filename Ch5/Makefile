#To include tracing information pass -d:TRACE as the cflag
#> make CFLAGS='-d:TRACE'
CC = dmcs 

exes = Eliza.exe ConvertElizaDef.exe

all: $(exes)

Eliza.exe: Eliza.cs SimpleIO.cs
	$(CC) $(CFLAGS) $^ -out:$@

ConvertElizaDef.exe: ConvertElizaDef.cs SexpParser.cs
	$(CC) $(CFLAGS) $^ -out:$@

.PHONY: clean
clean:
	rm $(exes)
