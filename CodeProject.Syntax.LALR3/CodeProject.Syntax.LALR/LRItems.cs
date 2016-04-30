using System;
namespace CodeProject.Syntax.LALR
{
	/// <summary>
	/// An item required for an LR0 Parser Construction
	/// </summary>
	public class LR0Item
	{
		public int Production {get;set;}
		public int Position {get; set;}
		
		public bool Equals(LR0Item item)
		{
			return (Production == item.Production) && (Position == item.Position);
		}
	};
	
	/// <summary>
	/// An item required for an LR1 Parser Construction
	/// </summary>
	public class LR1Item
	{
		public int LR0ItemID {get;set;}
		public int LookAhead {get;set;}
		
		public bool Equals(LR1Item item)
		{
			return (LR0ItemID == item.LR0ItemID) && (LookAhead == item.LookAhead);
		}
	};

}

