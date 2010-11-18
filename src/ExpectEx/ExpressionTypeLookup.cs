namespace ExpectEx
{
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public static class ExpressionTypeLookup
	{
		private static Dictionary<ExpressionType, string> BinaryLookup = new Dictionary<ExpressionType, string>
		                                                                 	{
		                                                                 		{ExpressionType.Add, "+"},
		                                                                 		{ExpressionType.AddChecked, "+ checked"},
		                                                                 		{ExpressionType.And, "&"},
		                                                                 		{ExpressionType.AndAlso, "&&"},
		                                                                 		{ExpressionType.Subtract, "-"},
		                                                                 		{ExpressionType.SubtractChecked, "- checked"},
		                                                                 		{ExpressionType.Multiply, "*"},
		                                                                 		{ExpressionType.MultiplyChecked, "* checked"},
		                                                                 		{ExpressionType.Divide, "/"},
		                                                                 		{ExpressionType.Modulo, "%"},
		                                                                 		{ExpressionType.Or, "|"},
		                                                                 		{ExpressionType.OrElse, "||"},
		                                                                 		{ExpressionType.LessThan, "<"},
		                                                                 		{ExpressionType.LessThanOrEqual, "<="},
		                                                                 		{ExpressionType.GreaterThan, ">"},
		                                                                 		{ExpressionType.GreaterThanOrEqual, ">="},
		                                                                 		{ExpressionType.Equal, "=="},
		                                                                 		{ExpressionType.NotEqual, "!="},
		                                                                 		{ExpressionType.Coalesce, "??"},
		                                                                 		{ExpressionType.ArrayIndex, "[]"},
		                                                                 		{ExpressionType.RightShift, ">>"},
		                                                                 		{ExpressionType.LeftShift, "<<"},
		                                                                 		{ExpressionType.ExclusiveOr, "^"},
		                                                                 	};

		public static string GetBinaryOperation(ExpressionType expressionType)
		{
			if (BinaryLookup.ContainsKey(expressionType))
			{
				return BinaryLookup[expressionType];
			}
			return expressionType.ToString();
		}
	}
}