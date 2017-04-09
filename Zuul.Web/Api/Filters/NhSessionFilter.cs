using Autofac.Integration.WebApi;
using NHibernate;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Net.Http.Formatting;
using System.IO;
using System.Net;
using System.Collections.Generic;

namespace Zuul.Web.Api.Filters
{
    public class NhSessionFilter : IAutofacActionFilter
    {
        private ISession _session;

        public NhSessionFilter(ISession session)
        {
            _session = session;
        }

        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            _session.BeginTransaction();
            return Task.FromResult(0);
        }

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var response = actionExecutedContext.Response;
            if (response != null)
            {
                if (response.Content != null)
                {
                    var objectContent = response.Content as ObjectContent;
                    if (objectContent != null)
                    {
                        response.Content =
                            new SessionClosingObjectContent(objectContent.ObjectType, objectContent.Value, objectContent.Formatter, _session);
                        foreach (var header in objectContent.Headers)
                        {
                            response.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                        }
                    }
                }
                else
                {
                    var transaction = _session.Transaction;
                    if (transaction != null && transaction.IsActive)
                    {
                        transaction.Commit();
                    }
                    _session.Close();
                }
            }
            return Task.FromResult(0);
        }

        private class SessionClosingObjectContent : ObjectContent
        {
            private ISession _session;

            public SessionClosingObjectContent(Type type, object value, MediaTypeFormatter formatter, ISession session)
                : base(type, value, formatter)
            {
                _session = session;
            }

            protected async override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                // Wait until the serializing has finished before closing the session.
                await base.SerializeToStreamAsync(stream, context);
                var transaction = _session.Transaction;
                if (transaction != null && transaction.IsActive)
                {
                    transaction.Commit();
                }
                _session.Close();
            }
        }

    }
}