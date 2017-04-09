using NHibernate;
using System.Web.Mvc;

namespace Zuul.Web.Filters
{
    public class NhSessionFilterMvc : IActionFilter 
    {
        private ISession _session;

        public NhSessionFilterMvc(ISession session)
        {
            _session = session;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _session.BeginTransaction();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var transaction = _session.Transaction;
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
            _session.Close();
        }
    }
}