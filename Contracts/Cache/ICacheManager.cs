using System;

namespace Contracts.Cache
{

	public interface ICacheManager
	{
		int MinutesToRemainInCache { set; }
		bool UseSlidingExpiration { set; }
		string DependsOnFileName { set; }
		void Clear<T>(T itemToCache) where T : ICachable;
		T Load<T>(T objectWithKeySet, Func<string, T> dataStoreDelegate) where T : ICachable;
		void Save<T>(T itemToCache) where T : ICachable;
		void ClearType<T>() where T : ICachable;
		void ClearTag(CacheTags tag, dynamic id);
	}
}
