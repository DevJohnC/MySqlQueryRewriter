using System.Text;

namespace MySqlQueryRewriter
{
	/// <summary>
	/// Writes MySql queries.
	/// </summary>
	public class MySqlQueryWriter
	{
		private readonly StringBuilder _text = new StringBuilder();

		public void WriteSelect()
		{
			_text.Append("SELECT ");
		}

		public void WriteInsert()
		{
			_text.Append("INSERT ");
		}

		public void WriteUpdate()
		{
			_text.Append("UPDATE ");
		}

		public void WriteDelete()
		{
			_text.Append("DELETE ");
		}

		public void WriteIdentifier(string identifier)
		{
			_text.Append($"{identifier} ");
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
