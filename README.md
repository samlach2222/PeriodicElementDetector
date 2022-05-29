# PeriodicElementDetector

## What is PeriodicElementDetector?
PeriodicElementDetector is a program that checks if a word is writable using the periodical table of elements.   
Not seriously, it also allows to give the encoding of a writable word using the number of the element.

## How do we know if a word is writable?
A word is writable if it is composed only of elements of the periodic table of elements and the element is not present twice in the word.

## An example?
BOAT can be translate to : B, O and AT element (Boron, Oxygen, Astatine)  
The encoding is : 5 8 85

## With a dictionary?
You can also load a dictionary into the program (here the French dictionary) and retrieve a text file with all the words of the dictionary compatible with the criteria of the periodic table of elements.

## The limits
The algorithm being particularly non-optimized, the search in the dictionary is very long (several days).  
An improvement of the program would allow to search intelligently and not by having a bruteforce reasoning (try all possibilities) to save an extremely consequent time.
