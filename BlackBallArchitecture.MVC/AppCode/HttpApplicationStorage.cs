﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackBallArchitecture.MVC.AppCode
{
    /// <summary>
    /// Stores items within the current web application
    /// </summary>
    public class HttpApplicationStorage : BlackBallArchitecture.Contracts.Cache.IPersistentStorage
    {
        public T Load<T>(string key)
        {
			if (HttpContext.Current == null) return default(T);
            var x = (T)HttpContext.Current.Application[key];
            return x;
        }

        public void Save(object itemToSave, string key)
        {
			if (HttpContext.Current == null) return;
            HttpContext.Current.Application[key] = itemToSave;
        }

		public void Clear()
		{
			if (HttpContext.Current == null) return;
			HttpContext.Current.Application.Clear();
		}
    }
}