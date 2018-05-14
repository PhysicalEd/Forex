using System;
using System.Configuration;
using System.Data.Entity;
using Contracts.Repositories;
using Data.EF.Database;

namespace Logic
{
	public partial class EntityFrameworkInitializer : IRepositoryInitializer
	{
		public static int DefaultCommandTimeout = 30;
		public bool StoreBuilderInCache = true;
		private string ConnectionString = "";
		private string ApplicationName = "";

		

		/// <summary>
		/// Creates a new repository connection to a database
		/// </summary>
		/// <returns></returns>
		public IRepository Create(string callerDescription)
		{
			this.ApplicationName = callerDescription;
			return this.Create();
		}

		/// <summary>
		/// Creates a new repository connection to a database
		/// </summary>
		/// <returns></returns>
		public IRepository Create()
		{

			// Load connection string

			var config = ConfigurationManager.ConnectionStrings["Data.Properties.Settings.BlackBallArchitectureConnectionString"];
			if (config == null) throw new Exception("Data.Properties.Settings.BlackBallArchitectureConnectionString has not been configured");
			this.ConnectionString = config.ConnectionString;
			if (string.IsNullOrWhiteSpace(this.ConnectionString)) throw new Exception("Data.Properties.Settings.BlackBallArchitectureConnectionString has not been configured");

			// Create and return connection
			Database.SetInitializer<CodeFirstModel>(null);
			var result = new CodeFirstModel(this.ConnectionString);
			result.ApplicationName = this.ApplicationName;
			result.CommandTimeout = DefaultCommandTimeout;
			return result;
		}
	}
}
