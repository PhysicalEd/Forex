using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using Dependency;
using Contracts.Cache;

namespace API.AppCode
{
    /// <summary>
    /// Manges retrieval and storage of objects into the cache
    /// </summary>
    public class WebCacheManager : ICacheManager
	{

		#region Properties

		protected bool UseCache
		{
			get
			{
				return Configuration.Current.UseCache;
			}
		}

		private int _MinutesToRemainInCache = 5;
		public int MinutesToRemainInCache
		{
			private get
			{
				return _MinutesToRemainInCache;
			}
			set
			{
				_MinutesToRemainInCache = value;
			}
		}

		private bool _UseSlidingExpiration = false;
		public bool UseSlidingExpiration
		{
			private get { return _UseSlidingExpiration; }
			set { _UseSlidingExpiration = value; }
		}

		private string _DependsOnFileName = "";
		public string DependsOnFileName
		{
			private get { return _DependsOnFileName; }
			set { _DependsOnFileName = value; }
		}

		#endregion

		/// <summary>
		/// Clears all the items with this tag
		/// </summary>
		/// <param name="tag"></param>
		public void ClearTag(CacheTags cacheTag, dynamic id)
		{
			if (id == null) id = "";
			string tag = cacheTag.ToString() + id;
			var keysToRemove = new List<string>();
			foreach (DictionaryEntry item in System.Web.HttpRuntime.Cache)
			{
				if (!(item.Value is ICachable)) continue;
				var key = (ICachable) item.Value;
				if (key.Tags.Contains(tag))
				{
					keysToRemove.Add(item.Key.ToString());
				}
			}

			// Remove each
			foreach (var cacheKey in keysToRemove)
			{
				try
				{
					if (System.Web.HttpRuntime.Cache[cacheKey] != null) System.Web.HttpRuntime.Cache.Remove(cacheKey);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Returns this item
		/// </summary>
		/// <param name="key"></param>
		/// <param name="dataStoreDelegate">A function to recall that retrieves the value from the actual data store, if not present</param>
		/// <returns></returns>
		public T Load<T>(T objectWithKeySet, Func<string, T> dataStoreDelegate) where T : ICachable
        {
			string cacheKey = this.GetCacheKey(objectWithKeySet);
			T cachedObject = default(T);
			
			if (this.UseCache)
			{
				cachedObject = (T)System.Web.HttpRuntime.Cache[cacheKey];
			}

			// If the object is not cached, we store it here
			if (cachedObject == null && dataStoreDelegate != null)
			{
				cachedObject = dataStoreDelegate.Invoke(objectWithKeySet.CacheKey);
				this.Save(cachedObject);
			}

			// Return
			return cachedObject;
        }

		/// <summary>
		/// Removes this item from the cache
		/// </summary>
		/// <param name="itemToCache"></param>
		public void Clear<T>(T itemToCache) where T : ICachable
		{
			string cacheKey = this.GetCacheKey(itemToCache);
			if (string.IsNullOrEmpty(cacheKey)) return;

			try
			{
				if (System.Web.HttpRuntime.Cache[cacheKey] != null) System.Web.HttpRuntime.Cache.Remove(cacheKey);
			}catch
			{
			}
		}

		/// <summary>
		/// Removes all cached items of this type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void ClearType<T>() where T : ICachable
		{
			var prefix = GetCacheKeyPrefix(typeof(T));
			var keysToRemove = new List<string>();
			foreach (DictionaryEntry de in HttpRuntime.Cache)
			{
				var key = de.Key.ToString();
				if (key.StartsWith(prefix)) keysToRemove.Add(key);
			}

			// Remove each
			foreach(var cacheKey in keysToRemove)
			{
				try
				{
					if (System.Web.HttpRuntime.Cache[cacheKey] != null) System.Web.HttpRuntime.Cache.Remove(cacheKey);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Saves this item to the cache
		/// </summary>
		/// <param name="itemToCache"></param>
		public void Save<T>(T itemToCache) where T : ICachable
		{
			string cacheKey = this.GetCacheKey(itemToCache);
			if (string.IsNullOrEmpty(cacheKey)) { return; }
			this.Clear(itemToCache);

			// Cannot store longer than a year - this is a .Net restriction on slidingExpiration
			var maxTimeSpan = new TimeSpan(365, 0, 0, 0, 0);
			this.MinutesToRemainInCache = Math.Min(this.MinutesToRemainInCache, (int)Math.Floor(maxTimeSpan.TotalMinutes));

			//Create the dependencies container, if there are any file dependencies
			CacheDependency dependencies = null;

			if (!string.IsNullOrEmpty(this.DependsOnFileName) && System.IO.File.Exists(this.DependsOnFileName))
			{
				dependencies = new CacheDependency(this.DependsOnFileName);
			}

			// Save with a sliding expiration or a set time
			if (this.UseSlidingExpiration)
			{
				TimeSpan ts = TimeSpan.FromMinutes(this.MinutesToRemainInCache);
				System.Web.HttpRuntime.Cache.Add(cacheKey, itemToCache, dependencies, System.Web.Caching.Cache.NoAbsoluteExpiration,
											 ts,
											 CacheItemPriority.Default, null);
			}
			else
			{
				System.Web.HttpRuntime.Cache.Add(cacheKey, itemToCache, dependencies, DateTime.Now.AddMinutes(this.MinutesToRemainInCache),
												 System.Web.Caching.Cache.NoSlidingExpiration,
												 CacheItemPriority.Default, null);
			}
		}

		/// <summary>
		/// Calculates a unique key for this cachable item
		/// </summary>
		/// <param name="itemToCache"></param>
		/// <returns></returns>
		private string GetCacheKey<T>(T itemToCache) where T : ICachable
		{
			if (itemToCache == null) return "";
			return GetCacheKeyPrefix(typeof(T)) + "_" + itemToCache.CacheKey;
		}

		/// <summary>
		/// Returns the prefix of this type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private string GetCacheKeyPrefix(Type type)
		{
			if (type == null) return "";
			return type.ToString();
		}
	}
}
