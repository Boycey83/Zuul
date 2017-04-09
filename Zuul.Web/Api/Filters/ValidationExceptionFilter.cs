using Autofac.Integration.WebApi;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using System;

namespace Zuul.Web.Api.Filters
{
    public class ValidationExceptionFilter : IAutofacExceptionFilter
    {
        private ISession _session;

        public ValidationExceptionFilter(ISession session)
        {
            _session = session;
        }

        public Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            _session.Transaction.Rollback();
            _session.Transaction.Dispose();
            _session.Dispose();
            var exception = actionExecutedContext.Exception as Exception;
            if (exception != null)
            {
                actionExecutedContext.Response =
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "An exception occurred.",
                        Content = new StringContent(exception.Message)
                    };
            }
            return Task.FromResult(0);
        }
    }
}