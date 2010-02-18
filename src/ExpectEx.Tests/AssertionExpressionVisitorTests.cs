namespace ExpectEx.Tests
{
	using System;
	using System.Linq.Expressions;
	using global::NUnit.Framework;

	[TestFixture]
	public class ExpressionPrinterTests : AssertionHelper
	{
		[Test]
		public void Print_TrueConstant_PrintsTrue()
		{
			var visitor = new AssertionExpressionVisitor();

			visitor.VisitConstant(Expression.Constant(true));

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("True"));
		}

		[Test]
		public void Print_ConvertConstant_PrintsNumber()
		{
			Expression<Func<decimal>> constantExpression = () => 5;
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(constantExpression);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("5"));
		}

		[Test]
		public void Print_VariableConstant_PrintsNameAndValue()
		{
			var number = 5m;
			Expression<Func<decimal>> constantExpression = () => number;
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(constantExpression);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("number [5]"));
		}

		[Test]
		public void Print_UnaryExpression_PrintsOperator()
		{
			var intNumber = 5;
			Expression<Func<decimal>> unaryExpression = () => intNumber;
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(unaryExpression);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("intNumber [5]"));
		}

		[Test]
		public void Print_BinaryExpression_Prints()
		{
			var a = 1;
			var b = 2;
			Expression<Func<decimal>> binaryExpression = () => a*b;
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(binaryExpression);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("(a [1] * b [2]) [2]"));
		}

		[Test]
		public void Print_NestedBinaryExpression_Prints()
		{
			var a = 1;
			var b = 2;
			var c = 3;
			Expression<Func<decimal>> binaryExpression = () => a*(b + c);
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(binaryExpression);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("(a [1] * (b [2] + c [3]) [5]) [5]"));
		}

		[Test]
		public void Print_MemberAccess_Prints()
		{
			var account = new Account();
			var initialDeposit = 100;
			account.Deposit(initialDeposit);
			var visitor = new AssertionExpressionVisitor();
			Expression<Func<decimal>> memberAccess = () => account.Balance;

			visitor.Visit(memberAccess);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("account.Balance [100]"));
		}

		[Test]
		public void Print_Conditional_Prints()
		{
			var a = true;
			var b = false;
			var c = true;
			Expression<Func<bool>> memberAccess = () => a ? b : c;
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(memberAccess);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("(a [True] ? b [False] : c [True])"));
		}

		private string GetMessage(AssertionExpressionVisitor visitor)
		{
			var message = visitor.GetAssertionMessage();
			Console.WriteLine(message);
			return message;
		}

		[Test]
		public void Print_Array_Prints()
		{
			var a = new[] {1, 2};
			Expression<Func<int>> arrayAccessor = () => a[1];
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(arrayAccessor);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("a[1] [2]"));
		}

		[Test]
		public void Print_MethodCall_Prints()
		{
			var account = new Account();
			account.SetName("Bob");
			var visitor = new AssertionExpressionVisitor();
			Expression<Func<string>> methodCall = () => account.GetName();

			visitor.Visit(methodCall);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("account.GetName() ['Bob']"));
		}

		[Test]
		public void Print_MethodWithArgument_Prints()
		{
			var account = new Account();
			account.SetName("Bob");
			var visitor = new AssertionExpressionVisitor();
			decimal amount = 100;
			decimal otherAmount = 200;
			Expression<Action> methodWithArgument = () => account.Deposits(amount, otherAmount);

			visitor.Visit(methodWithArgument);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("account.Deposits(amount [100], otherAmount [200])"));
		}

		[Test]
		public void Print_QueryWithArgument_Prints()
		{
			var account = new Account();
			var visitor = new AssertionExpressionVisitor();
			int a = 2;
			Expression<Action> queryWithArgument = () => account.AddNumbers(a, 1);

			visitor.Visit(queryWithArgument);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("account.AddNumbers(a [2], 1) [3]"));
		}

		[Test]
		public void Print_EqualityExpression_PrintsWithDoubleEqualSign()
		{
			var condition1 = 1;
			var condition2 = 2;
			Expression<Func<bool>> equalityExpression = () => condition1 == condition2;
			var visitor = new AssertionExpressionVisitor();

			visitor.Visit(equalityExpression);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("(condition1 [1] == condition2 [2]) [False]"));
		}

		[Test]
		public void Print_WithEvaluatingOff_PrintsWithoutValues()
		{
			var a = 1;
			var b = 2;
			var visitor = new AssertionExpressionVisitor();
			visitor.EvaluateValues = false;
			Expression<Func<bool>> expression = () => a + b + 3 == 6;

			visitor.Visit(expression);

			var message = GetMessage(visitor);
			Expect(message, Is.EqualTo("(((a + b) + 3) == 6)"));
		}
	}
}