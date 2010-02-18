namespace ExpectEx
{
	using System;
	using System.Linq.Expressions;

	public class InspectExpressionVisitor : ExpressionVisitor
	{
		public override Expression VisitBinary(BinaryExpression b)
		{
			if (b.NodeType == ExpressionType.Equal && b.Left.GetType() == b.Right.GetType())
			{
				var same = false;
				if (b.Left is MemberExpression)
				{
					same = CompareMembers(b.Left, b.Right);
				}
				else if (b.Left is MethodCallExpression)
				{
					same = CompareMethods(b.Left, b.Right);
				}
				if (same) throw new InspectionWarning("Operands of equality check are the same: always true");

				var leftResult = Expression.Lambda(b.Left).Compile().DynamicInvoke();
				var rightResult = Expression.Lambda(b.Right).Compile().DynamicInvoke();
				
				if(leftResult == null && rightResult == null)
				{
					throw new DefaultValueWarning("Comparison where both values are null.");
				}

				if (leftResult != null && rightResult != null
					&& leftResult.Equals(rightResult)
					&& leftResult.GetType().IsValueType
					)
				{
					var defaultValue = Activator.CreateInstance(leftResult.GetType());
					if (leftResult.Equals(defaultValue))
					{
						throw new DefaultValueWarning("Comparison of value types where both are the default value.");
					}
				}
			}
			return b;
		}

		public bool CompareMethods(Expression left, Expression right)
		{
			if (!(left is MethodCallExpression) || !(right is MethodCallExpression))
			{
				return false;
			}
			var leftMethod = (left as MethodCallExpression);
			var rightMethod = (right as MethodCallExpression);
			return leftMethod.Method == rightMethod.Method
			       && CompareMembers(leftMethod.Object, rightMethod.Object);
		}

		public bool CompareMembers(Expression left, Expression right)
		{
			if (!(left is MemberExpression) || !(right is MemberExpression))
			{
				return false;
			}
			return (left as MemberExpression).Member == (right as MemberExpression).Member;
		}

		public void Check(Expression expression)
		{
			this.Visit(expression);
		}
	}
}