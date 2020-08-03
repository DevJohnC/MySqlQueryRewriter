using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace parsers
{
	public abstract class MySQLBaseRecognizer : Parser, IMySQLRecognizerCommon
	{
		protected MySQLBaseRecognizer(ITokenStream input) : base(input)
		{
		}

		protected MySQLBaseRecognizer(ITokenStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
		{
		}

		public long serverVersion { get; set; } = 50630;

		public SqlMode sqlMode { get; set; }

		public bool isSqlModeActive(SqlMode mode)
		{
			return sqlMode.HasFlag(mode);
		}
	}
}
