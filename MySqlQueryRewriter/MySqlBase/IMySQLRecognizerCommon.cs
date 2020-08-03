using System;

namespace parsers
{
	[Flags]
	public enum SqlMode
	{
		NoMode = 0,
		AnsiQuotes = 1 << 0,
		HighNotPrecedence = 1 << 1,
		PipesAsConcat = 1 << 2,
		IgnoreSpace = 1 << 3,
		NoBackslashEscapes = 1 << 4
	}

	public interface IMySQLRecognizerCommon
	{
		long serverVersion { get; }

		SqlMode sqlMode { get; }

		bool isSqlModeActive(SqlMode mode);
	}
}
