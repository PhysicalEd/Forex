using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Contracts.Repositories;

namespace Data.EF.Database
{
	/// <summary>
	/// </summary>
	public partial class CodeFirstModel : DbContext, IRepository
	{

		private DateTime StartTime;

		public string ApplicationName = "";

		protected override void Dispose(bool disposing)
		{
			

			base.Dispose(disposing);
		}

		public CodeFirstModel(string connectionString)
			: base(connectionString)
		{
			
		}

        //public GetTables()
        //{
        //    //return this.Database
        //}


        public int CommandTimeout
		{
			set { this.Core.CommandTimeout = value; }
		}

		private ObjectContext Core
		{
			get { return (this as IObjectContextAdapter).ObjectContext; }

		}
        
		public IEnumerable<RETURNTYPE> ExecuteStoreQuery<RETURNTYPE>(string commandText, object[] parameters)
		{
			return this.Core.ExecuteStoreQuery<RETURNTYPE>(commandText, parameters);
		}

		public void ExecuteStoreCommand(string commandText, object[] parameters)
		{
			this.Core.ExecuteStoreCommand(commandText, parameters);

		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			LoadTables(modelBuilder);
		}

		/// <summary>
		/// Saves the changes back to the data store
		/// </summary>
		public void SubmitChanges()
		{
			this.SaveChanges();
		}
	}
}


