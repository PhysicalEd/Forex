using System;
using System.Web;

namespace Common.Extensions
{
	public static class HttpRequestExtensions
	{
		/// <summary>
		/// Parses the encrypted URL for this key in the parameters
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="encyprypedKey"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T Params<T>(this HttpRequest request, string unencryptedKey, T defaultValue){
			var valueFromRequest = request.Params[unencryptedKey];
			if (string.IsNullOrEmpty(valueFromRequest)) return defaultValue;
			return (T)Convert.ChangeType(valueFromRequest, typeof(T));
		}

		/// <summary>
		/// Parses the encrypted URL for this key in the parameters
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="request"></param>
		/// <param name="unencryptedKey"></param>
		/// <returns></returns>
		public static T Params<T>(this HttpRequest request, string unencryptedKey)
		{
			return request.Params<T>(unencryptedKey, default(T));
		}

	}
}
