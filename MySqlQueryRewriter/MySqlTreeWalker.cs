using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace MySqlQueryRewriter
{
	public class MySqlTreeWalker : ParseTreeWalker
	{
		public static readonly MySqlTreeWalker Instance = new MySqlTreeWalker();

		public override void Walk(IParseTreeListener listener, IParseTree t)
		{
			if (t is IErrorNode)
			{
				listener.VisitErrorNode((IErrorNode)t);
				return;
			}
			else
			{
				if (t is ITerminalNode)
				{
					listener.VisitTerminal((ITerminalNode)t);
					return;
				}
			}
			IRuleNode r = (IRuleNode)t;
			EnterRule(listener, r);
			int n = r.ChildCount;
			for (int i = 0; i < n; i++)
			{
				Walk(listener, r.GetChild(i));
			}
			ExitRule(listener, r);
		}

		protected override void EnterRule(IParseTreeListener listener, IRuleNode r)
		{
			ParserRuleContext ctx = (ParserRuleContext)r.RuleContext;
			listener.EnterEveryRule(ctx);
			ctx.EnterRule(listener);
		}

		protected override void ExitRule(IParseTreeListener listener, IRuleNode r)
		{
			ParserRuleContext ctx = (ParserRuleContext)r.RuleContext;
			ctx.ExitRule(listener);
			listener.ExitEveryRule(ctx);
		}
	}
}
