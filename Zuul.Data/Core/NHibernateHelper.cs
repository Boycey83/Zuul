using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using Vigo.DataAccess.NHibernate.Mapping;

namespace Zuul.Data.Core
{
	public sealed class NHibernateHelper
	{
		static readonly object FactoryLock = new object();
		private static ISessionFactory _sessionFactory;

		public static ISessionFactory SessionFactory
		{
			get
			{
				lock (FactoryLock)
				{
					if (_sessionFactory == null)
					{
						InitializeSessionFactory();
					}
				}
				return _sessionFactory;
			}

		}

		public static ISession OpenSession()
		{
			return SessionFactory.OpenSession();
		}

		private static void InitializeSessionFactory()
		{
			_sessionFactory = Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2005.ConnectionString(
					System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
					.ShowSql())
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserAccountMap>())
				.CurrentSessionContext<WebSessionContext>()
				.BuildSessionFactory();
		}

		public static ISessionFactory GetSessionFactory()
		{
			return SessionFactory;
		}
	}
}
