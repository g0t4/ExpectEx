namespace ExpectEx.Tests
{
	using System;
	using System.Linq.Expressions;
	using global::NUnit.Framework;
	using NUnit;

	[TestFixture]
	public class InspectExpressionVisitorTests : AssertionHelperEx
	{
		[Test]
		public void Visit_CompareSameMembers_ThrowsWarning()
		{
			var account = new Account();
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => account == account;

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.TypeOf<InspectionWarning>());
		}

		[Test]
		public void Visit_CompareSameMethodsOnSameObject_ThrowsWarning()
		{
			var account = new Account();
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => account.GetName() == account.GetName();

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.TypeOf<InspectionWarning>());
		}

		[Test]
		public void Visit_CompareSameMethodsOnSameObjectThroughMethod_ThrowsWarning()
		{
			var account = new Account();
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => account.GetName() == account.GetName();

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.TypeOf<InspectionWarning>());
		}

		[Test]
		public void Visit_CompareSameMethodsOnDifferntObjects_ThrowsWarning()
		{
			var account = new Account();
			account.SetName("A");
			var otherAccount = new Account();
			account.SetName("B");
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => account.GetMe().GetName() == otherAccount.GetMe().GetName();

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.Nothing);
		}

		[Test]
		public void Visit_DifferentMembers_DoesNotThrowWarning()
		{
			var account = new Account();
			var otherAccount = new Account();
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => account == otherAccount;

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.Nothing);			
		}

		[Test]
		public void Visit_ComparisonOfDefaultValueForValueType_Warns()
		{
			int a = default(int);
			int b = default(int);
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => a == b;

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.TypeOf<DefaultValueWarning>().With.Message.EqualTo("Comparison of value types where both are the default value."));
		}

		[Test]
		public void Visit_ComparisonOfNotDefaultValueForValueType_DoesNotWarn()
		{
			int a = 1;
			int b = default(int);
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => a == b;

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.Nothing);
		}

		[Test]
		public void Visit_ComparisonOfDefaultValueForReferenceType_Warns()
		{
			Account account = null;
			Account otherAccount = null;
			var inspector = new InspectExpressionVisitor();
			Expression<Func<bool>> expression = () => account == otherAccount;

			TestDelegate action = () => inspector.Visit(expression);

			Assert.That(action, Throws.TypeOf<DefaultValueWarning>().With.Message.EqualTo("Comparison where both values are null."));
		}
	
	}
}