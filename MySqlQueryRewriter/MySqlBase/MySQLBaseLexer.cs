using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;

namespace parsers
{
	public abstract class MySQLBaseLexer : Lexer, IMySQLRecognizerCommon
	{
		public MySQLBaseLexer(ICharStream input, TextWriter output, TextWriter errorOutput) :
			base(input, output, errorOutput)
		{

		}

		public long serverVersion { get; set; } = int.MaxValue;

		public SqlMode sqlMode { get; set; }

		public bool inVersionComment { get; set; }

		public bool isSqlModeActive(SqlMode mode)
		{
			return sqlMode.HasFlag(mode);
		}

		// Called when a keyword was consumed that represents an internal MySQL function and checks if that
		// keyword is followed by an open parenthesis. If not then it is not considered a keyword but
		// treated like a normal identifier.
		protected int determineFunction(int proposed)
		{
			if (isSqlModeActive(SqlMode.IgnoreSpace))
			{
				var nextChar = (char)InputStream.LA(1);
				while (nextChar == ' ' || nextChar == '\t' || nextChar == '\r' || nextChar == '\n')
				{
					Interpreter.Consume((ICharStream)InputStream);
					Channel = Hidden;
					Type = MySQLLexer.WHITESPACE;
					nextChar = (char)InputStream.LA(1);
				}
			}

			return InputStream.LA(1) == '(' ? proposed : MySQLLexer.IDENTIFIER;
		}

		protected void setType(int type)
		{
			Type = type;
		}

		private readonly Queue<IToken> _pendingTokens = new Queue<IToken>();

		public override IToken NextToken()
		{
			if (_pendingTokens.Count > 0)
			{
				var pending = _pendingTokens.Dequeue();
				return pending;
			}

			var next = base.NextToken();
			if (_pendingTokens.Count > 0)
			{
				var pending = _pendingTokens.Dequeue();
				_pendingTokens.Enqueue(next);
				return pending;
			}

			return next;
		}

		protected void emitDot()
		{
			_pendingTokens.Enqueue(TokenFactory.Create(new Tuple<ITokenSource, ICharStream>(this, (ICharStream)InputStream),
				MySQLLexer.DOT_SYMBOL, ".", Channel, TokenStartCharIndex, TokenStartCharIndex, TokenStartLine,
				TokenStartLine));
			Text = Text.Substring(1);
		}

		protected int checkCharset(string text)
		{
			return MySQLLexer.UNDERSCORE_CHARSET;
		}

		protected bool checkVersion(string versionStr)
		{
			return true;
		}

		protected string getText()
		{
			return Text;
		}

		protected int determineNumericType(string text)
		{
			return MySQLLexer.INT_NUMBER;
		}
	}
}
