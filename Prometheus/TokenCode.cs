namespace Prometheus
{
    class TokenCode
    {

        #region Properties

        private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string lexeme;

		public string Lexeme
		{
			get { return lexeme; }
			set { lexeme = value; }
		}


		private int line;

		public int Line
		{
			get { return line; }
			set { line = value; }
		}

		private int column;

		public int Column
		{
			get { return column; }
			set { column = value; }
		}

        #endregion

        #region Constructors

        public TokenCode(string name, string lexeme, int line, int column)
		{
			this.name	= name;
			this.lexeme = lexeme;
			this.line	= line;
			this.column = column;
		}

        #endregion

    }
}
