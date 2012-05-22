using System;

namespace ExpressionEvaluator
{
	public interface IMatchEvaluator
	{
		Match Match(string input);
	}
}
