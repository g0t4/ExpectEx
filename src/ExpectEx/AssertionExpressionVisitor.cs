namespace ExpectEx
{
	using System.Collections.ObjectModel;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text;

	public class AssertionExpressionVisitor : ExpressionVisitor
	{
		private StringBuilder _Builder;
		private bool _NestedAccess;

		public bool EvaluateValues = true;

		public AssertionExpressionVisitor()
		{
			_Builder = new StringBuilder();
		}

		public void GenerateAssertionMessage(Expression expression)
		{
			Visit(expression);
		}

		public override Expression VisitConditional(ConditionalExpression c)
		{
			_Builder.Append("(");
			var conditional = base.VisitConditional(c);
			_Builder.Append(")");
			return conditional;
		}

		public override void VisitConditionalAfterTest(ConditionalExpression expression)
		{
			_Builder.Append(" ? ");
		}

		public override void VisitConditionalAfterTrue(ConditionalExpression expression)
		{
			_Builder.Append(" : ");
		}

		public override Expression VisitBinary(BinaryExpression b)
		{
			if (b.NodeType == ExpressionType.ArrayIndex)
			{
				var wasNested = _NestedAccess;
				_NestedAccess = true;
				var arrayExpression = base.VisitBinary(b);
				_Builder.Append("]");
				if (!wasNested)
				{
					_NestedAccess = false;
					AppendValue(b);
				}
				return arrayExpression;
			}

			_Builder.Append("(");
			var binaryExpression = base.VisitBinary(b);
			_Builder.Append(")");
			AppendValue(b);
			return binaryExpression;
		}

		public override Expression VisitMethodCall(MethodCallExpression m)
		{
			var wasNested = _NestedAccess;
			_NestedAccess = true;
			var methodCall = base.VisitMethodCall(m);
			_Builder.Append(")");
			if (!wasNested)
			{
				_NestedAccess = false;
				AppendValue(m);
			}
			return methodCall;
		}

		private void AppendValue(Expression expression)
		{
			if(!EvaluateValues)
			{
				return;
			}
			var executor = Expression.Lambda(expression).Compile();
			if (executor.Method.ReturnType == typeof (void))
			{
				return;
			}
			var value = executor.DynamicInvoke();
			if (value.GetType() == typeof (string) || value.GetType() == typeof (char))
			{
				value = string.Format("'{0}'", value);
			}
			_Builder.AppendFormat(" [{0}]", value);
		}

		public override void VisitMethodCallBeforeArguments(MethodCallExpression expression)
		{
			_Builder.AppendFormat("{0}(", expression.Method.Name);
		}

		public override ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
		{
			var wasNested = _NestedAccess;
			_NestedAccess = false;
			var list = base.VisitExpressionList(original);

			_NestedAccess = wasNested;
			return list;
		}

		public override void VisitBetweenExpressionListItems(Expression current, Expression next)
		{
			_Builder.Append(", ");
		}

		public override void VisitBinaryNodeType(BinaryExpression b)
		{
			if (b.NodeType == ExpressionType.ArrayIndex)
			{
				if (_Builder[_Builder.Length - 1] == '.')
				{
					_Builder.Remove(_Builder.Length - 1, 1);
				}
				_Builder.Append("[");
				return;
			}
			var operation = typeof (BinaryExpression).GetMethod("GetOperator", BindingFlags.NonPublic | BindingFlags.Instance).
				Invoke(
				b, null);
			if (operation == "=")
			{
				operation = "==";
			}
			_Builder.AppendFormat(" {0} ", operation);
		}

		public override Expression VisitConstant(ConstantExpression constantExpression)
		{
			if (constantExpression.Value.ToString() == constantExpression.Value.GetType().ToString())
			{
				return constantExpression;
			}
			_Builder.Append(constantExpression.Value);
			return constantExpression;
		}

		public override Expression VisitMemberAccess(MemberExpression m)
		{
			var wasNested = _NestedAccess;
			_NestedAccess = true;
			var member = base.VisitMemberAccess(m);
			_Builder.Append(m.Member.Name);
			if (wasNested) _Builder.Append(".");
			else
			{
				_NestedAccess = false;
				AppendValue(m);
			}
			return member;
		}

		public string GetAssertionMessage()
		{
			return _Builder.ToString();
		}
	}
}