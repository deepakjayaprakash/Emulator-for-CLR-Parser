using System;
using CodeProject.Syntax.LALR;
using System.Collections.Generic;
namespace CD_grammar1
{
    class MainClass
    {
        private static string[,]  parseTable;

        // Parsing functions
        static string print(Stack<int> t)
        {
            Stack<int> temp2;
            temp2 = new Stack<int>(t);
            string stack_str = "";
            for (int i = 0; i <=temp2.Count; i++)
                stack_str += temp2.Pop().ToString();
            char[] charArray = stack_str.ToCharArray();
            //Array.Reverse(charArray);
            stack_str = new string(charArray);
            Console.Write(stack_str);
            return stack_str;
        }
          static string print(Stack<string> t)
        {
            Stack<string> temp2;
            temp2 = new Stack<string>(t);
            string stack_str = "";
            for (int i = 0; i <= temp2.Count; i++)
                stack_str += temp2.Pop().ToString();
            char[] charArray = stack_str.ToCharArray();
            //Array.Reverse(charArray);
            stack_str = new string(charArray);
            Console.Write(stack_str);
            return stack_str;
        }


        public static int getIndex(string[] token, string s, int inp)
        {
            if (s[inp] == '$')
            {
                //Console.WriteLine(1);
                return 1;

            }
            for (int i = 0; i < token.Length; i++)
            {
                if (token[i] == s[inp].ToString())
                {
                    //Console.WriteLine(i + 2);
                    return (i + 2);
                }

            }
            return -1;
        }


        //parsing function
        static int parseString(string[,] parseTable,Grammar grammar, string str,string[,] parseInputTable)
        {
            //parseInputTable= new string[30,4];
            Stack<int> stack = new System.Collections.Generic.Stack<int>();
            Stack<string> symbol = new System.Collections.Generic.Stack<string>();
            int inp_pointer = 0;
            Console.WriteLine();
            stack.Push(0);
            Production[] x = grammar.PrecedenceGroups[0].Productions;
            string[] token = grammar.Tokens;
            Console.WriteLine("InputParsing");
            Console.WriteLine("Stack tt Symbol tt Input tt Action end");
            parseInputTable[0, 0]= "Stack";
            parseInputTable[0, 1] = "Symbol";
            parseInputTable[0, 2] = "Input";
            parseInputTable[0, 3] = "Action";
            int l = 0;
            while (true)
            {
                // Console.WriteLine(++l);
                //input parsing
                l++;
                if (stack.Count != 0)
                {
                    string temp;
                    temp=print(stack);
                    if (stack.Count > 2)
                    {
                        temp += stack.Peek();
                        Console.Write(stack.Peek());
                    }
                    parseInputTable[l, 0] = temp;
                }
                else
                    Console.Write("");
                Console.Write("tt");
                if (symbol.Count != 0)
                {
                    string temp;
                    temp=print(symbol);
                    if (symbol.Count > 2)
                    {
                        Console.Write(symbol.Peek());
                        temp += symbol.Peek();
                    }
                    parseInputTable[l, 1] = temp;
                }
                else
                    Console.Write("");
                Console.Write("tt");
                Console.Write(str.Substring(inp_pointer, str.Length - inp_pointer));
                parseInputTable[l, 2] = str.Substring(inp_pointer, str.Length - inp_pointer);

                string value = parseTable[stack.Peek() + 1, getIndex(token, str, inp_pointer)];
                if (stack.Peek() == 0 && symbol.Count != 0 && symbol.Peek() == "S" && str[inp_pointer] == '$')
                {
                    Console.WriteLine("\n Input parsed!!");
                    return 1;
                    //break;
                }

                else if (value.Contains("S"))
                {

                    int state = Int32.Parse(value[2].ToString());
                    stack.Push(state);

                    symbol.Push(str[inp_pointer].ToString());
                    inp_pointer++;
                    Console.WriteLine("tt Shift " + state+" end");
                    parseInputTable[l, 3] = "\tShift " + state;

                }
                else if (value.Contains("R"))
                {
                    int prod_no = Int32.Parse(value[2].ToString());
                    for (int i = 0; i < x[prod_no].Right.Length; i++)
                    {
                        symbol.Pop();
                        stack.Pop();
                    }
                    int token_inserted = x[prod_no].Left;
                    symbol.Push(token[token_inserted]);
                    //Console.WriteLine(stack.Peek() + "\t" + symbol.Peek() + "\t" + str.Substring(inp_pointer, str.Length - inp_pointer));

                    if (stack.Peek() == 0 && symbol.Count != 0 && symbol.Peek() == "S" && str[inp_pointer] == '$')
                    {
                        Console.WriteLine("\n Input parsed!!");
                        return 1;
                       // break;
                    }
                    string temp = parseTable[stack.Peek() + 1, token_inserted + 2];
                    int state_inserted = Int32.Parse(temp[2].ToString());
                    stack.Push(state_inserted);
                    Console.WriteLine("tt Reduce " + prod_no+" end");
                    parseInputTable[l, 3] = "\tReduce " + prod_no;

                }
                else
                {
                    Console.WriteLine("\nParse error!");
                    return -1;
                    //break;
                }

            }
        }


        //printing grammar
        static void printGrammar(Grammar grammar)
        {
            Production[] x = grammar.PrecedenceGroups[0].Productions;
            string[] token = grammar.Tokens;
            for (int i = 0; i < x.Length; i++)
            {

                Console.Write(token[x[i].Left] + " --> ");
                for (int j = 0; j < x[i].Right.Length; j++)
                {
                    Console.Write(token[x[i].Right[j]] + " ");
                }
                Console.WriteLine("end");
                Console.WriteLine();

            }
        }


        public static void Main(string[] args)
        {

            //
            // the following program produces a parse table for the following grammar
            //
            // S -> C C
            // C -> c C
            // C -> d

            //
            //

            /*
            S->aAbA
            S->aAbc
            A->a


    */
    /*
            Grammar grammar = new Grammar();
            grammar.Tokens = new string[] { "S", "C", "c", "d"};

            grammar.PrecedenceGroups = new PrecedenceGroup[]
            {
                new PrecedenceGroup
                {
                    Derivation = Derivation.None,
                    Productions = new Production[]
                    {
						//S -> C C
						new Production{
                            Left = 0,
                            Right = new int[]{1 , 1}
                        },
						//C -> c C
						new Production{
                            Left = 1,
                            Right = new int[]{2 , 1}
                        },
						//C -> d
						new Production{
                            Left = 1,
                            Right =  new int[]{3}
                        }
                    }
                }
            };

*/

            Grammar grammar = new Grammar();
            grammar.Tokens = new string[] { "S", "A", "a", "b", "c"};

            grammar.PrecedenceGroups = new PrecedenceGroup[]
            {
                new PrecedenceGroup
                {
                    Derivation = Derivation.None,
                    Productions = new Production[]
                    {
						//S -> a A c
						new Production{
                            Left = 0,
                            Right = new int[]{2 , 1, 4}
                        },
						//A ->  c A b
						new Production{
                            Left = 1,
                            Right = new int[]{4 , 1, 3}
                        },
						//A -> b
						new Production{
                            Left = 1,
                            Right =  new int[]{3}
                        }
                    }
                }
            };

/*
            Grammar grammar2 = new Grammar();
            grammar.Tokens = new string[] { "S", "A", "a", "b", "c" };

            grammar.PrecedenceGroups = new PrecedenceGroup[]
            {
                new PrecedenceGroup
                {
                    Derivation = Derivation.None,
                    Productions = new Production[]
                    {
						//S -> a A b A
						new Production{
                            Left = 0,
                            Right = new int[]{2 , 1 , 3, 1}
                        },
						//S -> a A b c
						new Production{
                            Left = 0,
                            Right = new int[]{2 , 1, 3, 4}
                        },
						//A -> a
						new Production{
                            Left = 1,
                            Right =  new int[]{2}
                        }
                    }
                }
            };
*/
          //  Console.WriteLine(args[1]);
            

            // generate the parse table
            Parser parser;
        //    if(args[1]=="1")
             parser = new Parser(grammar);

  /*          else if(args[1]=="2")
               parser = new Parser(grammar1);
            else 
                parser = new Parser(grammar2);

    */
            int nStates = parser.ParseTable.Actions.GetLength(0)+1;
            int nTokens = parser.ParseTable.Actions.GetLength(1)+1;
            // write the parse table to the screen
            string[,] parseTable = new string[nStates, nTokens];
            
           parseTable = Debug.DumpParseTable(parser);

            if (args[0].Equals("1"))
            {
                Console.WriteLine("States");
                Debug.DumpCLRStates(parser);
            }
            //Debug.DumpLR1Items(parser);
            // Debug.DumpPropogationsForState(parser,1);
            //  Console.WriteLine(Debug.GetTokenName(parser, 5));
            if (args[0] == "2")
            {
                Console.WriteLine("Terminals");
                Debug.DumpTerminals(parser);
            }
            // Debug.DumpFirstSets(parser);
            //Debug.DumpLR0Kernels(parser);
            if (args[0] == "3")
            {
                Console.WriteLine("PropagationTable");
                Debug.DumpPropogationTable(parser);
            }
            if (args[0] == "4")
            {
                Console.WriteLine("Nonterminals");
                Debug.DumpNonterminals(parser);
            }
          //   Debug.DumpLR1Items(parser);
            //Debug.DumpLR0States(parser);
            //Debug.DumpLR1Item(parser, new LR1Item());
            //Debug.DumpLR0States(parser);
      //      Debug.DumpLR0Kernels(parser);
           Console.WriteLine();
            if (args[0] == "5")
            {
                for (int i = 0; i < nStates; i++)
                {
                    Console.WriteLine();
                    for (int j = 0; j < nTokens; j++)
                    {
                        if (parseTable[i, j] == " ")
                            Console.Write("bltt");
                        else
                        Console.Write(parseTable[i, j] + "tt");
                    }
                    Console.WriteLine("end");
                }
            }
            
string str = args[1];
            int status;
            string[,] parseInputTable=new string[30,4];
            status=parseString(parseTable, grammar, str, parseInputTable);
            if (args[0] == "6")
            {
                for (int i = 0; i < 30; i++)
                {
                    if (parseInputTable[i, 0].Equals(""))
                        break;
                    Console.WriteLine();
                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write(parseInputTable[i, j] + "\t\t");
                    }
                }
            }
            if (args[0] == "7")
            {
            Console.WriteLine("Grammar");
            printGrammar(grammar);
            }

            //Console.WriteLine("\n"+stack.Peek());

        }
       
    }
}
