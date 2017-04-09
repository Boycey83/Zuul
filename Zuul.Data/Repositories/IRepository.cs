namespace Zuul.Data.Repositories
{
	public interface IRepository
	{
		void BeginTransaction();
		void CommitTransaction();
		void DisposeTransaction();
	}
}
