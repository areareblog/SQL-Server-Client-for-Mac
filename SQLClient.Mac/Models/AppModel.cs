using System;
using System.Data;
using System.Data.SqlClient;

namespace SQLClient.Mac
{
	public class AppModel
	{
		public SqlConnection Connection
		{
			get;
			set;
		}

		public AppModel()
		{
			
		}

		public bool HasConnection
		{
			get { return Connection != null; }
		}


		public void CloseConnection()
		{
			if (HasConnection && Connection.State == ConnectionState.Open)
			{
				Connection.Close();
				Connection.Dispose();
				Connection = null;
			}
		}

	}
}
