using System;
using CodeProject.Syntax.LALR;

namespace TestProject
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//
			// the following program produces a parse table for the following grammar
			// for infix expressions, and appropriately applies operator precedence of
			// the + - * / operators, otherwise evaluating the leftmost operations first
			//
			// S' -> e
			// e -> i
			// e -> ( e )
			// e -> e * e
			// e -> e / e
			// e -> e + e
			// e -> e - e
			//
			// 
			
			Grammar grammar = new Grammar();
			grammar.Tokens = new string[]{"S'", "e", "+", "-", "*", "/", "i", "(", ")"};
			
			grammar.PrecedenceGroups = new PrecedenceGroup[]
			{
				new PrecedenceGroup
				{
					Derivation = Derivation.None,
					Productions = new Production[]
					{
						//S' -> e
						new Production{
							Left = 0,
							Right = new int[]{1}
						},
						//e -> i
						new Production{
							Left = 1,
							Right = new int[]{6}
						},
						//e -> ( e )
						new Production{
							Left = 1,
							Right =  new int[]{7, 1, 8}
						}
					}
				},
				new PrecedenceGroup
				{
					Derivation = Derivation.LeftMost,
					Productions = new Production[]
					{
						//e -> e * e
						new Production{
							Left = 1,
							Right = new int[]{1, 4, 1}
						},
						//e -> e / e
						new Production{
							Left = 1,
							Right = new int[]{1, 5, 1}
						},
					}
				},
				new PrecedenceGroup
				{
					//productions are left associative and bind less tightly than * or /
					Derivation = Derivation.LeftMost,
					Productions = new Production[]
					{
						//e -> e + e
						new Production{
							Left = 1,
							Right = new int[]{1, 2, 1}
						},
						//e -> e - e
						new Production{
							Left = 1,
							Right = new int[]{1, 3, 1}
						}
					}
				}
			};

			// generate the parse table
			Parser parser = new Parser(grammar);
			
			// write the parse table to the screen
			Debug.DumpParseTable(parser);
            //Debug.DumpLALRStates(parser);
            //Debug.DumpLR1Items(parser);
            // Debug.DumpPropogationsForState(parser,1);
            //  Console.WriteLine(Debug.GetTokenName(parser, 5));
            Debug.DumpTerminals(parser);
            // Debug.DumpFirstSets(parser);
            //Debug.DumpLR0Kernels(parser);
            //Debug.DumpPropogationTable(parser);
            Debug.DumpNonterminals(parser);
            
		}
	}
}

