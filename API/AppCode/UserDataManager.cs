using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace API.AppCode
{
	public class UserDataManager
	{
		private Dictionary<string, string> CurrentValues = new Dictionary<string, string>();

		/// <summary>
		/// Records this item in state
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set(string name, object value)
		{
			try
			{
				if (HttpContext.Current != null && HttpContext.Current.Response != null && HttpContext.Current.Response.Cookies != null)
				{
					var encodedName = name;

					// Remove from response
					var responseCookie = HttpContext.Current.Response.Cookies[encodedName];
					if (value == null && responseCookie != null) responseCookie.Expires = DateTime.Now.AddDays(-1);
					if (value == null) return;

					// Add
					if (responseCookie == null) responseCookie = new HttpCookie(encodedName);
					var encodedValue = value.ToString();
					responseCookie.Value = encodedValue;
					responseCookie.Expires = DateTime.Now.AddDays(14);
					HttpContext.Current.Response.Cookies.Add(responseCookie);
				}
			}
			finally
			{
				// Save to memory as well for quick access later
				if (value == null)
				{
					if (CurrentValues.ContainsKey(name)) CurrentValues.Remove(name);
				}
				else
				{
					CurrentValues[name] = value.ToString();
				}
			}
		}

		/// <summary>
		/// Returns this value from our state bag
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T Get<T>(string name)
		{
			bool isNullable = false;
			try
			{
				Type newType = typeof (T);
				// ChangeType throws exception for null types, so just throw it here
				if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
				{
					newType = new NullableConverter(typeof(T)).UnderlyingType;
					isNullable = true;
				}

				// Already recorded in memory?
				if (CurrentValues.ContainsKey(name)) return (T)Convert.ChangeType(CurrentValues[name], newType);

				if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies != null)
				{
					var cook = HttpContext.Current.Request.Cookies[name];
					if (cook != null && cook.Value != null)
					{
						string unencryptedValue = cook.Value;
						T result = (T)Convert.ChangeType(unencryptedValue, newType);
						return result;
					}
				}else if (isNullable)
				{
					return (T)(object)null;
				}
			}
			catch{}

			// Return default
			if (isNullable) return (T) (object) null;
			return default(T);
		}

	}
}
