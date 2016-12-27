using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace SQLClient.Mac
{
	public class LoginModel
	{
		public LoginModel()
		{
		}

		public SqlConnection Login(string server, string login, string password)
		{
			var task = this.LoginAsync(server, login, password);
			task.Wait();

			return task.Result;
		}
		

		public SqlConnection Login(string server, string port, string login, string password)
		{
			var task = this.LoginAsync(server, port, login, password);
			task.Wait();

			return task.Result;
		}

		public async Task<SqlConnection> LoginAsync(string server, string login, string password)
		{

			var port = "1433";

			if (server.Contains(":"))
			{
				var splited = server.Split(':');
				server = splited[0];
				port = splited[1];
			}

			return await LoginAsync(server, port, login, password);
		}
		

		public async Task<SqlConnection> LoginAsync(string server, string port, string login, string password)
		{
			var connectionString = $"Server={server},{port};Database=master;UID={login};PWD={password};MultipleActiveResultSets=true;";

			var connection = new SqlConnection(connectionString);
			await connection.OpenAsync();

			return connection;

		}

	}
}
