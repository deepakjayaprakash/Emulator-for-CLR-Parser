using System;
namespace CodeProject.Syntax.LALR
{
	/// <summary>
	/// Describes how to resolve ambiguities within a precedence group
	/// </summary>
	public enum Derivation
	{
		None,
		LeftMost,
		RightMost
	};
	
	/// <summary>
	/// A grammatical production
	/// </summary>
	public class Production
	{
		public int Left{get;set;}
		public int[] Right{get;set;}
	};
	
	/// <summary>
	/// A collection of productions at a particular precedence
	/// </summary>
	public class PrecedenceGroup
	{
		public Derivation Derivation {get;set;}
		public Production[] Productions{get;set;}
	};
	
	/// <summary>
	/// All of the information required to make a Parser
	/// </summary>
	public class Grammar
	{
		public string[] Tokens{get;set;}
		
		public PrecedenceGroup[] PrecedenceGroups {get;set;}
	};
}

