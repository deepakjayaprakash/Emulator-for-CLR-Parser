The objective of this project is to implement an efficient and robust CLR parser using a high level programming language, 
that is capable of correctly parsing any input fed to it. The LR(1) parsing is a technique of bottom-up parsing.
‘L’ says that the input string is scanned from left to right, ‘R’ says that the parsing technique uses rightmost derivations,
and ‘1’ stands for the look-ahead. To avoid some of invalid reductions, the states need to carry more information. 
Extra information is put into state by adding a terminal symbol as the second component in the item. 

	Thus the canonical-LR parser makes full use of look-ahead symbols. This method uses a large set of items, called LR(1) items.

	The LR(1) parsing method consists of a parser stack, that holds non-terminals, grammar symbols and tokens;
	a parsing table that specifies parser actions, and a driver function that interacts with the parser stack,
	table and scanner. The typical actions of a CLR parser include: shift, reduce, and accept or error.

The project work would include a set of predefined grammar and an interface which would convert each phase of the parsing process
into a visual representation and would display onto webpage. The implementation is pretty straight forward and simple.
Then it would take any input string belonging to the grammar language and would show the acceptance or rejection of that
input string and also the steps one by one. 
