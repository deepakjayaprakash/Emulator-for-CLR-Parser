using System;
namespace CodeProject.Syntax.LALR
{
	/// <summary>
	/// the type of action the parser will perform
	/// </summary>
	public enum ActionType
	{
		Reduce,
		Shift,
		Error
	};
	
	/// <summary>
	/// A parse table entry
	/// </summary>
	public class Action
	{
		public ActionType ActionType {get;set;}
		public int ActionParameter {get;set;}
		
		public bool Equals(Action action)
		{
			return (ActionType == action.ActionType) && (ActionParameter == action.ActionParameter);
		}
	};
	
	/// <summary>
	/// Directs the parser on which action to perform at a given state on a particular input
	/// </summary>
	public class ParseTable
	{
		public Action [,] Actions {get;set;}
	};
}

