namespace ExpectEx.NUnit.Tests
{
	using System;
	using System.Linq.Expressions;
	using global::NUnit.Framework;

	[TestFixture]
	public class AssertionHelperExTests : AssertionHelperEx
	{
		[Test]
		public void Expect_FailedAssertion_ThrowsAssertionException()
		{
			Assert.That(() => Expect(() => false), Throws.TypeOf<AssertionException>().With.Message.EqualTo("Assertion failed: False"));
		}

		[Test]
		public void Expect_PassingAssertion_DoesNotThrowException()
		{
			Assert.That(() => Expect(() => true),  Throws.Nothing);			
		}
	}
}