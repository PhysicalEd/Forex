//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Contracts;
//using Contracts.DataManagers;
//using Contracts.Entities.Data;
//using Contracts.Enums;
//using Contracts.Security;

//namespace Logic
//{
//	public class DatabaseLogger : ILogger
//	{

//		/// <summary>
//		/// Logs the given message
//		/// </summary>
//		/// <param name="log"></param>
//		public int Log(string message)
//		{
//			return this.Log(message, SystemLogTypes.Info);
//		}

//		/// <summary>
//		/// Logs the given message
//		/// </summary>
//		/// <param name="log"></param>
//		/// <param name="type"></param>
//		public int Log(string message, SystemLogTypes type)
//		{
//			var log = new SystemLog()
//			{
//				Message = message,
//				SystemLogTypeID = (int)type,
//			};
//			return this.Log(log);
//		}

//		/// <summary>
//		/// Logs this error and description
//		/// </summary>
//		/// <param name="ex"></param>
//		/// <param name="description"></param>
//		public int Log(Exception ex, string description)
//		{
//			var log = new SystemLog()
//			{
//				Message = description,
//				SystemLogTypeID = (int)SystemLogTypes.Error,
//				Details = ex.ToString()
//			};
//			return this.Log(log);
//		}

//		/// <summary>
//		/// Returns log messages matching the given filter
//		/// </summary>
//		/// <param name="type"></param>
//		/// <param name="fromDate"></param>
//		/// <param name="?"></param>
//		/// <param name="?"></param>
//		/// <param name="toDate"></param>
//		/// <returns></returns>
//		public List<SystemLog> GetLog(SystemLogTypes type, DateTime fromDate, DateTime toDate)
//		{
//			int systemLogTypeID = (int) type;
//			using(var cxt = DataStore.CreateDataStore())
//			{
//				var data = (
//				           	from log in cxt.SystemLog
//				           	where (log.SystemLogTypeID == systemLogTypeID || systemLogTypeID == 0)
//				           	      && log.WhenOccurred >= fromDate
//				           	      && log.WhenOccurred <= toDate
//				           	orderby log.WhenOccurred descending
//				           	select log
//				           );
//				return data.ToList();
//			}
//		}

//		/// <summary>
//		/// Logs the given message
//		/// </summary>
//		/// <param name="log"></param>
//		public int Log(Exception ex)
//		{
//			var log = new SystemLog()
//			{
//				Message = ex.Message,
//				SystemLogTypeID = (int)SystemLogTypes.Error,
//				Details = ex.ToString()
//			};
//			return this.Log(log);
//		}

//		/// <summary>
//		/// Logs the given message
//		/// </summary>
//		/// <param name="logSummary"></param>
//		private int Log(SystemLog logSummary)
//		{
//			// We can use Unity to call back to whichever storage mechanism we are using for the 'current user' info, without actually knowing what it is
//			var personID = Dependency.Dependency.Resolve<ICurrentUser>().PersonID;

//			// Log
//			logSummary.PersonID = personID;
//			logSummary.WhenOccurred = DateTime.Now;
//			var svc = Dependency.Dependency.Resolve<ISystemLogManager>();
//			svc.SaveLog(logSummary);
//			return logSummary.SystemLogID;
//		}
//	}
//}
