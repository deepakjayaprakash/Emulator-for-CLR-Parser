using System;
using System.Collections.Generic;
namespace CodeProject.Syntax.LALR
{	
	public class LALRPropogation
	{
		public int LR0TargetState {get;set;}
		public int LR0TargetItem {get;set;}
	};
	
	public class Parser
	{
		HashSet<int> [] m_firstSets;
		List<LR0Item> m_lr0Items;
		List<LR1Item> m_lr1Items;
		
		List<HashSet<int>> m_lr0States;
		List<HashSet<int>> m_lr0Kernels;
		List<HashSet<int>> m_lalrStates;
		Dictionary<int, HashSet<int>> m_lookAheads;
		
		List<int[]> m_lrGotos;
		List<int[]> m_gotoPrecedence;
		
		Grammar m_grammar;
		List<Production> m_productions;
		List<int> m_terminals;
		List<int> m_nonterminals;
		
		List<Dictionary<int, List<LALRPropogation>>> m_lalrPropogations;
		
		ParseTable m_parseTable;
		
		List<int> m_productionPrecedence;
		List<Derivation> m_productionDerivation;
		
		public HashSet<int>[] FirstSets
		{
			get
			{
				return m_firstSets;
			}
		}
		
		public List<LR0Item> LR0Items
		{
			get
			{
				return m_lr0Items;
			}
		}
		
		public List<LR1Item> LR1Items
		{
			get
			{
				return m_lr1Items;
			}
		}
		
		public List<HashSet<int>> LR0States
		{
			get
			{
				return m_lr0States;
			}
		}
		
		public List<HashSet<int>> LR0Kernels
		{
			get
			{
				return m_lr0Kernels;
			}
		}
		
		public List<HashSet<int>> LALRStates
		{
			get
			{
				return m_lalrStates;
			}
		}
		
		public Dictionary<int, HashSet<int>> LookAheads
		{
			get
			{
				return m_lookAheads;
			}
		}
		
		public List<int[]> LRGotos
		{
			get
			{
				return m_lrGotos;
			}
		}
		
		public List<int[]> GotoPrecedence
		{
			get
			{
				return m_gotoPrecedence;
			}
		}
		
		public List<Production> Productions
		{
			get
			{
				return m_productions;
			}
		}
		
		public List<int> Terminals
		{
			get
			{
				return m_terminals;
			}
		}
		
		public List<int> NonTerminals
		{
			get
			{
				return m_nonterminals;
			}
		}
		
		public List<Dictionary<int, List<LALRPropogation>>> LALRPropogations
		{
			get
			{
				return m_lalrPropogations;
			}
		}
		
		public List<int> ProductionPrecedence
		{
			get
			{
				return m_productionPrecedence;
			}
		}
		
		public List<Derivation> ProductionDerivation
		{
			get
			{
				return m_productionDerivation;
			}
		}
		
		public Grammar Grammar
		{
			get
			{
				return m_grammar;
			}
		}
		
		public ParseTable ParseTable
		{
			get
			{
				return m_parseTable;
			}
		}
		
		
		/// <summary>
		/// Adds a propogation to the propogation table
		/// </summary>
		void AddPropogation(int nLR0SourceState, int nLR0SourceItem, int nLR0TargetState, int nLR0TargetItem)
		{
			while(m_lalrPropogations.Count <= nLR0SourceState)
			{
				m_lalrPropogations.Add(new Dictionary<int, List<LALRPropogation>>());
			}
			
			Dictionary<int, List<LALRPropogation>> propogationsForState = m_lalrPropogations[nLR0SourceState];
			List<LALRPropogation> propogationList = null;
			if(!propogationsForState.TryGetValue(nLR0SourceItem, out propogationList))
			{
				propogationList = new List<LALRPropogation>();
				propogationsForState[nLR0SourceItem] = propogationList;
			}
			
			propogationList.Add(new LALRPropogation{LR0TargetState = nLR0TargetState, LR0TargetItem = nLR0TargetItem});
		}
		
		/// <summary>
		/// Gets the ID for a particular LR0 Item
		/// </summary>
		int GetLR0ItemID(LR0Item item)
		{
			int nItemID = 0;
			foreach(LR0Item oItem in m_lr0Items)
			{
				if(oItem.Equals(item))
				{
					return nItemID;
				}
				nItemID ++;
			}
			m_lr0Items.Add(item);
			return nItemID;
		}
		
		
		/// <summary>
		/// Gets the ID for a particular LR1 Item
		/// </summary>
		int GetLR1ItemID(LR1Item item)
		{
			int nItemID = 0;
			foreach(LR1Item oItem in m_lr1Items)
			{
				if(oItem.Equals(item))
				{
					return nItemID;
				}
				nItemID ++;
			}
			m_lr1Items.Add(item);
			return nItemID;
		}
		
		
		/// <summary>
		/// Gets the ID for a particular LR0 State
		/// </summary>
		int GetLR0StateID(HashSet<int> state, ref bool bAdded)
		{
			int nStateID = 0;
			foreach(HashSet<int> oState in m_lr0States)
			{
				if(oState.SetEquals(state))
				{
					return nStateID;
				}
				nStateID ++;
			}
			m_lr0States.Add(state);
			bAdded = true;
			return nStateID;
		}
		
		/// <summary>
		/// takes a set of LR0 Items and Produces all of the LR0 Items that are reachable by substitution
		/// </summary>
		HashSet<int> LR0Closure(HashSet<int> i)
		{
			HashSet<int> closed = new HashSet<int>();
			List<int> open = new List<int>();
			
			foreach(int itemCopy in i)
			{
				open.Add(itemCopy);
			}
			
			while(open.Count > 0)
			{
				int nItem = open[0];
				open.RemoveAt(0);
				LR0Item item = m_lr0Items[nItem];
				closed.Add(nItem);
				
				int nProduction = 0;
				foreach(Production production in m_productions)
				{
					if((item.Position < m_productions[item.Production].Right.Length) && (production.Left == m_productions[item.Production].Right[item.Position]))
					{
						LR0Item newItem = new LR0Item{Production = nProduction, Position = 0};
						int nNewItemID = GetLR0ItemID(newItem);
						if(!open.Contains(nNewItemID) && !closed.Contains(nNewItemID))
						{
							open.Add(nNewItemID);
							
						}
					}
					nProduction ++;
				}
			}
			
			return closed;
		}
		
		/// <summary>
		/// takes a set of LR1 Items (LR0 items with lookaheads) and produces all of those LR1 items reachable by substitution
		/// </summary>
		HashSet<int> LR1Closure(HashSet<int> i)
		{
			HashSet<int> closed = new HashSet<int>();
			List<int> open = new List<int>();
			
			foreach(int itemCopy in i)
			{
				open.Add(itemCopy);
			}
			
			while(open.Count > 0)
			{
				int nLR1Item = open[0];
				open.RemoveAt(0);
				LR1Item lr1Item = m_lr1Items[nLR1Item];
				LR0Item lr0Item = m_lr0Items[lr1Item.LR0ItemID];
				closed.Add(nLR1Item);
				
				if(lr0Item.Position < m_productions[lr0Item.Production].Right.Length)
				{
					int nToken = m_productions[lr0Item.Production].Right[lr0Item.Position];
					if(m_nonterminals.Contains(nToken))
					{
						List<int> argFirst = new List<int>();
						for(int nIdx = lr0Item.Position + 1; nIdx < m_productions[lr0Item.Production].Right.Length; nIdx++)
						{
							argFirst.Add(m_productions[lr0Item.Production].Right[nIdx]);
						}
						HashSet<int> first = First(argFirst, lr1Item.LookAhead);
						int nProduction = 0;
						foreach(Production production in m_productions)
						{
							if(production.Left == nToken)
							{
								foreach(int nTokenFirst in first)
								{
									LR0Item newLR0Item = new LR0Item{Production = nProduction, Position = 0};
									int nNewLR0ItemID = GetLR0ItemID(newLR0Item);
									LR1Item newLR1Item = new LR1Item{LR0ItemID = nNewLR0ItemID, LookAhead = nTokenFirst};
									int nNewLR1ItemID = GetLR1ItemID(newLR1Item);
									if(!open.Contains(nNewLR1ItemID) && !closed.Contains(nNewLR1ItemID))
									{
										open.Add(nNewLR1ItemID);
									}
								}
							}
							nProduction ++;
						}
					}
				}
			}
			
			return closed;
		}
		
		/// <summary>
		/// takes an LR0 state, and a tokenID, and produces the next state given the token and productions of the grammar
		/// </summary>
		int GotoLR0(int nState, int nTokenID, ref bool bAdded, ref int nPrecedence)
		{
			HashSet<int> gotoLR0 = new HashSet<int>();
			HashSet<int> state = m_lr0States[nState];
			foreach(int nItem in state)
			{
				LR0Item item = m_lr0Items[nItem];
				if(item.Position < m_productions[item.Production].Right.Length && (m_productions[item.Production].Right[item.Position] == nTokenID))
				{
					LR0Item newItem = new LR0Item{Production = item.Production, Position = item.Position + 1};
					int nNewItemID = GetLR0ItemID(newItem);
					gotoLR0.Add(nNewItemID);
					int nProductionPrecedence = m_productionPrecedence[item.Production];
					if(nPrecedence < nProductionPrecedence)
					{
						nPrecedence = nProductionPrecedence;
					}
				}
			}
			if(gotoLR0.Count == 0)
			{
				return -1;
			}
			else
			{
				return GetLR0StateID(LR0Closure(gotoLR0), ref bAdded);
			}
		}
		
		/// <summary>
		/// Generates all of the LR 0 Items
		/// </summary>
		void GenerateLR0Items()
		{
			HashSet<int> startState = new HashSet<int>();
			startState.Add(GetLR0ItemID(new LR0Item{ Production = 0, Position = 0}));
			
			bool bIgnore = false;
			List<int> open = new List<int>();
			open.Add(GetLR0StateID(LR0Closure(startState), ref bIgnore));
			
			while(open.Count > 0)
			{
				int nState = open[0];
				open.RemoveAt(0);
				while(m_lrGotos.Count <= nState)
				{
					m_lrGotos.Add(new int [m_grammar.Tokens.Length]);
					m_gotoPrecedence.Add(new int [m_grammar.Tokens.Length]);
				}
				
				for(int nToken = 0; nToken < m_grammar.Tokens.Length; nToken++)
				{
					bool bAdded = false;
					int nPrecedence = int.MinValue;
					int nGoto = GotoLR0(nState, nToken, ref bAdded, ref nPrecedence);
					
					m_lrGotos[nState][nToken] = nGoto;
					m_gotoPrecedence[nState][nToken] = nPrecedence;
					
					if(bAdded)
					{
						open.Add(nGoto);
					}
				}
			}
		}
		

		/// <summary>
		/// Computes the set of first terminals for each token in the grammar
		/// </summary>
		void ComputeFirstSets()
		{
			int nCountTokens = m_nonterminals.Count + m_terminals.Count;
			m_firstSets = new HashSet<int> [nCountTokens];
			for(int nIdx = 0; nIdx < nCountTokens; nIdx++)
			{
				m_firstSets[nIdx] = new HashSet<int>();
				if(m_terminals.Contains(nIdx))
				{
					m_firstSets[nIdx].Add(nIdx);
				}
			}
			
			foreach(Production production in m_productions)
			{
				if(production.Right.Length == 0)
				{
					m_firstSets[production.Left].Add(-1);
				}
			}
			
			bool bDidSomething;
			do
			{
				bDidSomething = false;
				foreach(Production production in m_productions)
				{
					foreach(int nToken in production.Right)
					{
						bool bLookAhead = false;
						foreach(int nTokenFirst in m_firstSets[nToken])
						{
							if(nTokenFirst == -1)
							{
								bLookAhead = true;
							}
							else if(m_firstSets[production.Left].Add(nTokenFirst))
							{
								bDidSomething = true;
							}
						}
						
						if(!bLookAhead)
						{
							break;
						}
					}
				}
			}
			while(bDidSomething);
		}
		
		/// <summary>
		/// returns the set of terminals that are possible to see next given an arbitrary list of tokens
		/// </summary>
		HashSet<int> First(List<int> tokens, int nTerminal)
		{
			HashSet<int> first = new HashSet<int>();
			foreach(int nToken in tokens)
			{
				bool bLookAhead = false;
				foreach(int nTokenFirst in m_firstSets[nToken])
				{
					if(nTokenFirst == -1)
					{
						bLookAhead = true;
					}
					else
					{
						first.Add(nTokenFirst);
					}
				}
				
				if(!bLookAhead)
				{
					return first;
				}
			}
			
			first.Add(nTerminal);
			return first;
		}
		
		/// <summary>
		/// Initializes the propogation table, and initial state of the LALR table
		/// </summary>
		void InitLALRTables()
		{
			int nLR0State = 0;
			foreach(HashSet<int> lr0State in m_lr0States)
			{
				m_lalrStates.Add(new HashSet<int>());
			}
			foreach(HashSet<int> lr0Kernel in m_lr0Kernels)
			{
				HashSet<int> J = new HashSet<int>();
				foreach(int jLR0ItemID in lr0Kernel) 
				{
					LR1Item lr1Item = new LR1Item{LR0ItemID = jLR0ItemID, LookAhead = -1};
					int nLR1ItemID = GetLR1ItemID(lr1Item);
					J.Add(nLR1ItemID);
				}
				HashSet<int> JPrime = LR1Closure(J);
				foreach(int jpLR1ItemID in JPrime)
				{
					LR1Item lr1Item = m_lr1Items[jpLR1ItemID];
					LR0Item lr0Item = m_lr0Items[lr1Item.LR0ItemID];
					
					if((lr1Item.LookAhead != -1) || (nLR0State == 0)) 
					{
						m_lalrStates[nLR0State].Add(jpLR1ItemID);
					}
					
					if(lr0Item.Position < m_productions[lr0Item.Production].Right.Length)
					{
						int nToken = m_productions[lr0Item.Production].Right[lr0Item.Position];
						LR0Item lr0Successor = new LR0Item{Production = lr0Item.Production, Position = lr0Item.Position + 1};
						int nLR0Successor = GetLR0ItemID(lr0Successor);
						int nSuccessorState = m_lrGotos[nLR0State][nToken];
						if(lr1Item.LookAhead == -1)
						{
							AddPropogation(nLR0State, lr1Item.LR0ItemID, nSuccessorState, nLR0Successor);
						}
						else
						{
							LR1Item lalrItem = new LR1Item{LR0ItemID = nLR0Successor, LookAhead = lr1Item.LookAhead};
							int nLALRItemID = GetLR1ItemID(lalrItem);
							m_lalrStates[nSuccessorState].Add(nLALRItemID);
						}
					}
				}
				
				nLR0State ++;
			}
		}
		
		/// <summary>
		/// Calculates the states in the LALR table
		/// </summary>
		void CalculateLookAheads()
		{
			bool bChanged;
			do
			{
				bChanged = false;
				int nState = 0;
				foreach(Dictionary<int, List<LALRPropogation>> statePropogations in m_lalrPropogations)
				{
					bool bStateChanged = false;
					foreach(int nLR1Item in m_lalrStates[nState])
					{
						LR1Item lr1Item = m_lr1Items[nLR1Item];
						
						if(statePropogations.ContainsKey(lr1Item.LR0ItemID))
						{
							foreach(LALRPropogation lalrPropogation in statePropogations[lr1Item.LR0ItemID])
							{
								int nGoto = lalrPropogation.LR0TargetState;
								LR1Item item = new LR1Item{LR0ItemID = lalrPropogation.LR0TargetItem, LookAhead = lr1Item.LookAhead};
								if(m_lalrStates[nGoto].Add(GetLR1ItemID(item)))
								{
									bChanged = true;
									bStateChanged = true;
								}
							}
						}
					}
					
					if(bStateChanged)
					{
						m_lalrStates[nState] = LR1Closure(m_lalrStates[nState]);
					}
					nState ++;
				}
			}
			while(bChanged);
		}
		
		/// <summary>
		/// Initializes the tokens for the grammar
		/// </summary>
		void InitSymbols()
		{	
			for(int nSymbol = 0; nSymbol < m_grammar.Tokens.Length; nSymbol++)
			{
				bool bTerminal = true;
				foreach(Production production in m_productions)
				{
					if(production.Left == nSymbol)
					{
						bTerminal = false;
						break;
					}
				}
				
				if(bTerminal)
				{
					m_terminals.Add(nSymbol);
				}
				else
				{
					m_nonterminals.Add(nSymbol);
				}
			}
		}

		/// <summary>
		/// Converts an LR0 State to an LR0 Kernel consisting of only the 'initiating' LR0 Items in the state
		/// </summary>
		public void ConvertLR0ItemsToKernels()
		{
			foreach(HashSet<int> lr0State in m_lr0States)
			{
				HashSet<int> lr0Kernel = new HashSet<int>();
				foreach(int nLR0Item in lr0State)
				{
					LR0Item item = m_lr0Items[nLR0Item];
					if(item.Position != 0)
					{
						lr0Kernel.Add(nLR0Item);
					}
					else if(m_productions[item.Production].Left == 0)
					{
						lr0Kernel.Add(nLR0Item);
					}
				}
				m_lr0Kernels.Add(lr0Kernel);
			}
		}
		
		/// <summary>
		/// Helper function that returns true if the list of actions contains an action
		/// </summary>
		bool ListContainsAction(List<Action> list, Action action)
		{
			foreach(Action listAction in list)
			{
				if(listAction.Equals(action))
				{
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Generates the parse table given the lalr states, and grammar
		/// </summary>
		void GenerateParseTable()
		{
			m_parseTable = new ParseTable();
			m_parseTable.Actions = new Action[m_lalrStates.Count, m_grammar.Tokens.Length + 1];
			for(int nStateID = 0; nStateID < m_lalrStates.Count; nStateID++)
			{
				HashSet<int> lalrState = m_lalrStates[nStateID];
				
				for(int nToken = -1; nToken < m_grammar.Tokens.Length; nToken++)
				{
					List<Action> actions = new List<Action>();
					string sToken = "$";
					if(nToken >= 0)
					{
						sToken = m_grammar.Tokens[nToken];
						
						if(m_lrGotos[nStateID][nToken] >= 0)
						{
							actions.Add(new Action{ActionType = ActionType.Shift, ActionParameter = m_lrGotos[nStateID][nToken]});
						}
					}
	
					foreach(int nLR1ItemID in lalrState)
					{
						LR1Item lr1Item = m_lr1Items[nLR1ItemID];
						LR0Item lr0Item = m_lr0Items[lr1Item.LR0ItemID];
						
						if((lr0Item.Position == m_productions[lr0Item.Production].Right.Length) && lr1Item.LookAhead == nToken)
						{
							Action action = new Action{ActionType = ActionType.Reduce, ActionParameter = lr0Item.Production};
							if(!ListContainsAction(actions, action))
							{
								actions.Add(action);
							}
						}
					}
					
					int nMaxPrecedence = int.MinValue;
					List<Action> importantActions = new List<Action>();
					foreach(Action action in actions)
					{
						int nActionPrecedence = int.MinValue;
						if(action.ActionType == ActionType.Shift)
						{
							nActionPrecedence = m_gotoPrecedence[nStateID][nToken]; //nToken will never be -1
						}
						else if(action.ActionType == ActionType.Reduce)
						{
							nActionPrecedence = m_productionPrecedence[action.ActionParameter];
						}
						
						if(nActionPrecedence > nMaxPrecedence)
						{
							nMaxPrecedence = nActionPrecedence;
							importantActions.Clear();
							importantActions.Add(action);
						}
						else if (nActionPrecedence == nMaxPrecedence)
						{
							importantActions.Add(action);
						}
					}
					
					if(importantActions.Count == 1)
					{
						m_parseTable.Actions[nStateID, nToken + 1] = importantActions[0];
					}
					else if(importantActions.Count > 1)
					{
						Action shiftAction = null;
						List<Action> reduceActions = new List<Action>();
						foreach(Action action in importantActions)
						{
							if(action.ActionType == ActionType.Reduce)
							{
								reduceActions.Add(action);
							}
							else
							{
								shiftAction = action;
							}
						}
						
						Derivation derv = m_grammar.PrecedenceGroups[-nMaxPrecedence].Derivation;
						if(derv == Derivation.LeftMost && reduceActions.Count == 1)
						{
							m_parseTable.Actions[nStateID, nToken + 1] = reduceActions[0];
						}
						else if(derv == Derivation.RightMost && shiftAction != null)
						{
							m_parseTable.Actions[nStateID, nToken + 1] = shiftAction;
						}
						else
						{
							if(derv == Derivation.None && reduceActions.Count == 1)
							{
								Console.WriteLine("Error, shift-reduce conflict in grammar");
							}
							else
							{
								Console.WriteLine("Error, reduce-reduce conflict in grammar");
							}
							m_parseTable.Actions[nStateID, nToken + 1] = new Action{ActionType = ActionType.Error, ActionParameter = nToken};
						}
					}
					else
					{
						m_parseTable.Actions[nStateID, nToken + 1] = new Action{ActionType = ActionType.Error, ActionParameter = nToken};
					}
				}
			}
		}
		
		/// <summary>
		/// helper function
		/// </summary>
		void PopulateProductions()
		{
			int nPrecedence = 0;
			foreach(PrecedenceGroup oGroup in m_grammar.PrecedenceGroups)
			{
				foreach(Production oProduction in oGroup.Productions)
				{
					m_productions.Add(oProduction);
					m_productionPrecedence.Add(nPrecedence);
					m_productionDerivation.Add(oGroup.Derivation);
				}
				nPrecedence --;
			}
		}
		
		/// <summary>
		/// constructor, construct parser table
		/// </summary>
		public Parser(Grammar grammar)
		{
			m_lrGotos = new List<int[]>();
			m_gotoPrecedence = new List<int[]>();
			m_lr0Items = new List<LR0Item>();
			m_lr1Items = new List<LR1Item>();
			m_lr0States = new List<HashSet<int>>();
			m_lr0Kernels = new List<HashSet<int>>();
			m_lalrStates = new List<HashSet<int>>();
			m_terminals = new List<int>();
			m_nonterminals = new List<int>();
			m_lalrPropogations = new List<Dictionary<int, List<LALRPropogation>>>();
			m_grammar = grammar;
			m_productions =  new List<Production>();
			m_productionDerivation = new List<Derivation>();
			m_productionPrecedence = new List<int>();
			
			PopulateProductions();
			InitSymbols();
			GenerateLR0Items();
			ComputeFirstSets();
			ConvertLR0ItemsToKernels();
			InitLALRTables();
			CalculateLookAheads();
			GenerateParseTable();
		}
	}
}

