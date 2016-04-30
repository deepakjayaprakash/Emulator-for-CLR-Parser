using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeProject.Syntax.LALR;
namespace CD_GUI
{
    public partial class Form1 : Form
    {
        private static string[,] parseTable;

        // Parsing functions
        static string print(Stack<int> t)
        {
            Stack<int> temp2;
            temp2 = new Stack<int>(t);
            string stack_str = "";
            for (int i = 0; i <= temp2.Count; i++)
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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            // the following program produces a parse table for the following grammar
            //
            // S -> C C
            // C -> c C
            // C -> d

            //
            // 

            Grammar grammar = new Grammar();
            grammar.Tokens = new string[] { "S", "C", "c", "d" };

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

            // generate the parse table
            Parser parser = new Parser(grammar);
            int nStates = parser.ParseTable.Actions.GetLength(0) + 1;
            int nTokens = parser.ParseTable.Actions.GetLength(1) + 1;
            // write the parse table to the screen
            string[,] parseTable = new string[nStates, nTokens];
            parseTable = Debug.DumpParseTable(parser);
            Debug.DumpLALRStates(parser);
            //Debug.DumpLR1Items(parser);
            // Debug.DumpPropogationsForState(parser,1);
            //  Console.WriteLine(Debug.GetTokenName(parser, 5));
            Debug.DumpTerminals(parser);
            // Debug.DumpFirstSets(parser);
            //Debug.DumpLR0Kernels(parser);
            Debug.DumpPropogationTable(parser);
            Debug.DumpNonterminals(parser);
            //Debug.DumpLR1Items(parser);
            Console.WriteLine();
            for (int i = 0; i < nStates; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < nTokens; j++)
                {
                    Console.Write(parseTable[i, j] + "\t\t");
                }
            }
            Button clickedButton = (Button)sender;
            TableLayoutPanel dynamicTableLayoutPanel = new TableLayoutPanel();

            dynamicTableLayoutPanel.Location = new System.Drawing.Point(26, 12);

            dynamicTableLayoutPanel.Name = "TableLayoutPanel1";

            dynamicTableLayoutPanel.Size = new System.Drawing.Size(228, 200);

            dynamicTableLayoutPanel.BackColor = Color.LightBlue;
            //dynamicTableLayoutPanel.


            // Add rows and columns
            dynamicTableLayoutPanel.ColumnCount = nTokens;

            dynamicTableLayoutPanel.RowCount = nStates;

            for (int k = 0; k < nTokens; k++)
                dynamicTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / nTokens));
            for (int k = 0; k < nStates; k++)
                dynamicTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / nStates));






            for (int i = 0; i < nStates; i++)
            {
                //Console.WriteLine();
                for (int j = 0; j < nTokens; j++)
                {
                    Label label = new Label();

                    //label.Location = new Point(10, 10);

                    label.Text = parseTable[i, j];

                    //textBox1.Size = new Size(200, 30);
                    dynamicTableLayoutPanel.Controls.Add(label, j, i);
                    //Console.Write(parseTable[i, j] + "\t\t");
                }
            }



            // Add child controls to TableLayoutPanel and specify rows and column



            //dynamicTableLayoutPanel.Controls.Add(checkBox1, 0, 1);



            Controls.Add(dynamicTableLayoutPanel);
        }

        private void button2_Click(object sender, EventArgs e)
        {


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
        static int parseString(string[,] parseTable, Grammar grammar, string str, string[,] parseInputTable)
        {
            //parseInputTable= new string[30,4];
            Stack<int> stack = new System.Collections.Generic.Stack<int>();
            Stack<string> symbol = new System.Collections.Generic.Stack<string>();
            int inp_pointer = 0;
            Console.WriteLine();
            stack.Push(0);
            Production[] x = grammar.PrecedenceGroups[0].Productions;
            string[] token = grammar.Tokens;
            Console.WriteLine("Stack\tSymbol\tInput\tAction");
            parseInputTable[0, 0] = "Stack";
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
                    temp = print(stack);
                    if (stack.Count > 2)
                    {
                        temp += stack.Peek();
                        Console.Write(stack.Peek());
                    }
                    parseInputTable[l, 0] = temp;
                }
                else
                    Console.Write("");
                Console.Write("\t");
                if (symbol.Count != 0)
                {
                    string temp;
                    temp = print(symbol);
                    if (symbol.Count > 2)
                    {
                        Console.Write(symbol.Peek());
                        temp += symbol.Peek();
                    }
                    parseInputTable[l, 1] = temp;
                }
                else
                    Console.Write("");
                Console.Write("\t");
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
                    Console.WriteLine("\tShift " + state);
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
                    Console.WriteLine("\tReduce " + prod_no);
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
                Console.WriteLine();

            }
        }
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










            Grammar grammar = new Grammar();
            grammar.Tokens = new string[] { "S", "C", "c", "d" };

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

            // generate the parse table
            Parser parser = new Parser(grammar);
            int nStates = parser.ParseTable.Actions.GetLength(0) + 1;
            int nTokens = parser.ParseTable.Actions.GetLength(1) + 1;
            // write the parse table to the screen
            string[,] parseTable = new string[nStates, nTokens];
            parseTable = Debug.DumpParseTable(parser);
            Debug.DumpLALRStates(parser);
            //Debug.DumpLR1Items(parser);
            // Debug.DumpPropogationsForState(parser,1);
            //  Console.WriteLine(Debug.GetTokenName(parser, 5));
            Debug.DumpTerminals(parser);
            // Debug.DumpFirstSets(parser);
            //Debug.DumpLR0Kernels(parser);
            Debug.DumpPropogationTable(parser);
            Debug.DumpNonterminals(parser);
            Debug.DumpLR1Items(parser);
            //Debug.DumpLR0States(parser);
            //Debug.DumpLR1Item(parser, new LR1Item());
            //Debug.DumpLR0States(parser);
            Debug.DumpLR0Kernels(parser);
            Console.WriteLine();
            for (int i = 0; i < nStates; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < nTokens; j++)
                {
                    Console.Write(parseTable[i, j] + "\t\t");
                }
            }
            string str = "cdcccccd$";
            int status;
            string[,] parseInputTable = new string[30, 4];
            status = parseString(parseTable, grammar, str, parseInputTable);
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(parseInputTable[i, j] + "\t\t");
                }
            }
            printGrammar(grammar);

            //Console.WriteLine("\n"+stack.Peek());

        
        TableLayoutPanel dynamicTableLayoutPanel = new TableLayoutPanel();

            dynamicTableLayoutPanel.Location = new System.Drawing.Point(26, 12);

            dynamicTableLayoutPanel.Name = "TableLayoutPanel1";

            dynamicTableLayoutPanel.Size = new System.Drawing.Size(228, 200);

            dynamicTableLayoutPanel.BackColor = Color.LightBlue;
            //dynamicTableLayoutPanel.


            // Add rows and columns
 

            for (int k = 0; k < 4; k++)
                dynamicTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / 4));
            for (int k = 0; k < 30; k++)
                dynamicTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / 30));

            for (int i = 0; i < 30; i++)
            {
                //Console.WriteLine();
                for (int j = 0; j < 4; j++)
                {
                    Label label = new Label();

                    //label.Location = new Point(10, 10);

                    label.Text = parseInputTable[j, i];

                    //textBox1.Size = new Size(200, 30);
                    dynamicTableLayoutPanel.Controls.Add(label, j, i);
                    //Console.Write(parseTable[i, j] + "\t\t");
                }
            }
        }
    }
}
