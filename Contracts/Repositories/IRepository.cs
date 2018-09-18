using System;
using System.Collections.Generic;

namespace Contracts.Repositories
{
	/// <summary>
	/// 
	/// </summary>
	public partial interface IRepository : IDisposable
	{
		int SaveChanges();
		void SubmitChanges();

	    IEnumerable<RETURNTYPE> ExecuteStoreQuery<RETURNTYPE>(string commandText, object[] parameters);

	    void ExecuteStoreCommand(string commandText, object[] parameters);



	}
}
