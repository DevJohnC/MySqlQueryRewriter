using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using parsers;
using System;
using System.Collections.Generic;

namespace MySqlQueryRewriter
{
	/// <summary>
	/// Visits parsed mysql query nodes.
	/// </summary>
	public class MySqlQueryVisitor : MySQLParserBaseListener
	{
		public delegate RewriteResult TerminalNodeRewriter(MySqlQueryWriter writer, ITerminalNode node);

		private readonly MySqlQueryWriter _writer;

		private readonly Dictionary<int, List<TerminalNodeRewriter>> _terminalNodeRewriters = new Dictionary<int, List<TerminalNodeRewriter>>();

		public MySqlQueryVisitor(MySqlQueryWriter writer)
		{
			_writer = writer ?? throw new ArgumentNullException(nameof(writer));
		}

		public void AddTerminalNodeRewriter(int symbolType, TerminalNodeRewriter terminalNodeRewriter)
		{
			if (!_terminalNodeRewriters.TryGetValue(symbolType, out var rewriterList))
			{
				rewriterList = new List<TerminalNodeRewriter>();
				_terminalNodeRewriters.Add(symbolType, rewriterList);
			}

			rewriterList.Add(terminalNodeRewriter);
		}

		public override void VisitErrorNode([NotNull] IErrorNode node)
		{
			base.VisitErrorNode(node);
		}

		public override void VisitTerminal([NotNull] ITerminalNode node)
		{
			if (_terminalNodeRewriters.TryGetValue(node.Symbol.Type, out var rewriterList))
			{
				var writeTokenText = true;
				foreach (var rewriter in rewriterList)
				{
					if (rewriter(_writer, node) == RewriteResult.ReplacedSymbol)
					{
						writeTokenText = false;
						break;
					}
				}

				if (writeTokenText)
				{
					WriteRawTokenText(node);
				}
			}
			else
			{
				WriteRawTokenText(node);
			}
		}

		private void WriteRawTokenText(ITerminalNode node)
		{
			if (node.Symbol.Type == -1) //  Don't write raw EOFs
			{
				return;
			}
			_writer.WriteSymbol(node.GetText());
		}
	}
}
