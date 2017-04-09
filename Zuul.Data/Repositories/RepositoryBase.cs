using NHibernate;

namespace Zuul.Data.Repositories
{
	public abstract class RepositoryBase : IRepository
	{
		private ISession _session;
		private  ITransaction _transaction;

		public RepositoryBase(ISession session)
		{
			_session = session;
		}

		public RepositoryBase()
		{
		}

		public void BeginTransaction()
		{
			_transaction = _session.BeginTransaction();
		}

		public void CommitTransaction()
		{
			_transaction.Commit();
		}

		public void DisposeTransaction()
		{
			_transaction.Dispose();
		}
	}
}
