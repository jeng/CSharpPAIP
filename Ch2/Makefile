CC = dmcs

all: RandomSentenceOne RandomSentenceGrammar 

RandomSentenceOne:
	$(CC) RandomSentenceOne.cs SimpleGrammar.cs RandomSingleton.cs ListExtensions.cs

RandomSentenceGrammar:
	$(CC) RandomSentenceGrammar.cs RandomSingleton.cs ListExtensions.cs Rule.cs GrammarParser.cs

clean:
	rm *.exe
