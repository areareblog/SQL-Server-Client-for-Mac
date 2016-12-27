using System;
namespace SQLClient.Mac
{
	public class TableDefinitionItem
	{
		public string ColumnName
		{
			get;
			set;
		}

		public bool IsPrimaryKey
		{
			get;
			set;
		}

		public string DataType
		{
			get;
			set;
		}

		public bool IsNullable
		{
			get;
			set;
		}
	}
}
