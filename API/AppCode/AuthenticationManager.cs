//using System;
//using System.Linq;
//using System.Web;
//using Common.Navigation;
//using Contracts.DataManagers;
//using Contracts.Security;

//namespace API.AppCode
//{
//    public class AuthenticationManager : ICurrentUser
//	{

//		public int? PersonID
//		{
//			get
//			{
//				if (!IsAuthenticated) { return null; }
//				return int.Parse(HttpContext.Current.User.Identity.Name);
//			}
//		}

//		private string _UserSessionIdentifier = "";
//		public string UserSessionIdentifier
//		{
//			get
//			{
//				// Check memory first
//				if (!string.IsNullOrEmpty(_UserSessionIdentifier)) return _UserSessionIdentifier;

//				// Pull from session
//				var userDataMgr = new UserDataManager();
//				_UserSessionIdentifier = userDataMgr.Get<string>(SessionKeys.CURRENT_USER_SESSION);
//				if (!string.IsNullOrEmpty(_UserSessionIdentifier)) return _UserSessionIdentifier;

//				// Save for next time
//				_UserSessionIdentifier = Guid.NewGuid().ToString();
//				userDataMgr.Set(SessionKeys.CURRENT_USER_SESSION, _UserSessionIdentifier);

//				return _UserSessionIdentifier;
//			}
//		}

//		private string _DisplayName = "";
//		public string DisplayName
//		{
//			get
//			{
//				if (!IsAuthenticated) return "";

//				// Check memory first
//				if (!string.IsNullOrEmpty(_DisplayName)) return _DisplayName;

//				// Pull from session
//				var userDataMgr = new UserDataManager();
//				_DisplayName = userDataMgr.Get<string>(SessionKeys.CURRENT_USER_NAME);
//				if (!string.IsNullOrEmpty(_DisplayName)) return _DisplayName;

//				// Save for next time
//				_DisplayName = Dependency.Dependency.Resolve<IPersonManager>().GetPerson(this.PersonID.Value).FirstOrDefault().FirstName;
//				userDataMgr.Set(SessionKeys.CURRENT_USER_NAME, _DisplayName);

//				return _UserSessionIdentifier;
//			}
//			set
//			{
//				new UserDataManager().Set(SessionKeys.CURRENT_USER_NAME, value);
//				_DisplayName = value;
//			}
//		}

//		public bool IsAuthenticated
//		{
//			get
//			{
//				if (HttpContext.Current == null || HttpContext.Current.User == null || HttpContext.Current.User.Identity == null) return false;
//				return HttpContext.Current.User.Identity.IsAuthenticated;
//			}
//		}
//	}
//}
