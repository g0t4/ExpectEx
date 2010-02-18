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
			var otherAccount = new Account();
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
	}
}