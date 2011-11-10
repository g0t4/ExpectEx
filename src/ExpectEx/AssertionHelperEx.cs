namespace ExpectEx.NUnit
{
	using System;
	using System.Linq.Expressions;
	using global::NUnit.Framework;

	public class AssertionHelperEx : AssertionHelper
	{
		public static bool EnableExpressionInspection;

		public virtual void Expect(Expression<Func<bool>> expression)
		{
			if (EnableExpressionInspection)
			{
				var inspector = new InspectExpressionVisitor();
				inspector.Check(expression);
			}
			var pass = expression.Compile().Invoke();
			var visitor = new AssertionExpressionVisitor();
			visitor.GenerateAssertionMessage(expression);
			if (pass)
			{
				Console.Write("Assertion passed: ");
				Console.WriteLine(visitor.GetAssertionMessage());
				return;
			}
			throw new AssertionException(string.Format("Assertion failed: {0}", visitor.GetAssertionMessage()));
		}
	}
}