using System;
using System.Configuration;
using System.Web;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Dependency
{

	public interface IContainerAccessor
	{
		IUnityContainer Container { get; }
	}

	public class Dependency
	{
		public static void Register(IUnityContainer container)
		{
			UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
			config.Containers.Default.Configure(container);
		}

		public static T Resolve<T>()
		{
			return Container.Resolve<T>();
		}

		public static T Resolve<T>(object name)
		{
			return Container.Resolve<T>(name.ToString());
		}

		private static IUnityContainer _container;
		public static IUnityContainer Container
		{
			get
			{
				// Testing only - see Set method below
				if (_container != null) return _container;

				var context = HttpContext.Current;

				// The most likely situation where this occurs is because we are on a different thread. These threads
				// are not part of the 'current' context, hence explosion
				if (context == null)
				{
					try
					{
						IUnityContainer c = new UnityContainer();
						Register(c);
						return c;
					}
					catch
					{
						// When running from the TEST projects, the above fails. 
						return null;
					}
				}

				// Get it from global. This is cached in the Application, so is the ideal scenario
				var accessor = context.ApplicationInstance as IContainerAccessor;
				if (accessor == null)
				{
					return null;
				}

				var container = accessor.Container;

				if (container == null)
				{
					throw new InvalidOperationException("No Unity container found");
				}


				return container;
			}

			// Only for testing purposes
			set
			{
				_container = value;
			}
		}
	}
}
