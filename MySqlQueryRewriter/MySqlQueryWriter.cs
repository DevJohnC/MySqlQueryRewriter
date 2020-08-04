using System.Text;

namespace MySqlQueryRewriter
{
	/// <summary>
	/// Writes MySql queries.
	/// </summary>
	public class MySqlQueryWriter
	{
		private readonly StringBuilder _text = new StringBuilder();

		public void WriteToken(string tokenStr)
		{
			_text.Append($"{tokenStr} ");
		}

		public override int GetHashCode()
		{
			return _text.GetHashCode();
		}

		public override string ToString()
		{
			return _text.ToString();
		}
	}
}
