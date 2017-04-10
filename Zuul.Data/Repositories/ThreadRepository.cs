using NHibernate;
using System;
using System.Linq;
using Zuul.Model;
using System.Collections.Generic;
using NHibernate.Linq;
using Zuul.Model.Enums;

namespace Zuul.Data.Repositories
{
    public class ThreadRepository : RepositoryBase, IThreadRepository
    {
        private ISession _session;

        public ThreadRepository(ISession session)
        {
            _session = session;
        }

        public int CreateThread(Thread thread)
        {
            return (int)_session.Save(thread);
        }

        public Thread GetById(int threadId)
        {
            return _session.Get<Thread>(threadId);
        }

        public void UpdateLastPostDateTimeUtc(int threadId, DateTime lastPostDateTimeUtc)
        {
            var thread = _session.Get<Thread>(threadId);
            thread.LastPostDateTimeUtc = lastPostDateTimeUtc;
            _session.Update(thread);
        }

        public IEnumerable<Thread> GetTopThreads(ThreadSortOrder sortOrder, int pageNumber, int pageSize)
        {
            var threadsToSkip = (pageNumber - 1) * pageSize;
            var query = _session.Query<Thread>();
            query = ApplyThreadSortOrder(sortOrder, query);
            return query.Skip(threadsToSkip).Take(pageSize).Fetch(t => t.PostedBy);
        }

        public int GetThreadCount()
        {
            return _session.Query<Thread>().Count();
        }

        private IQueryable<Thread> ApplyThreadSortOrder(ThreadSortOrder sortOrder, IQueryable<Thread> query)
        {
            switch (sortOrder)
            {
                case ThreadSortOrder.Date:
                    return query.OrderBy(t => t.LastPostDateTimeUtc);
                case ThreadSortOrder.DateDesc:
                    return query.OrderByDescending(t => t.LastPostDateTimeUtc);
                case ThreadSortOrder.PostedBy:
                    return query.OrderBy(t => t.PostedBy.Username);
                case ThreadSortOrder.PostedByDesc:
                    return query.OrderByDescending(t => t.PostedBy.Username);
                case ThreadSortOrder.Subject:
                    return query.OrderBy(t => t.Title);
                case ThreadSortOrder.SubjectDesc:
                    return query.OrderByDescending(t => t.Title);
                case ThreadSortOrder.Size:
                    return query.OrderBy(t => t.PostCount);
                case ThreadSortOrder.SizeDesc:
                    return query.OrderByDescending(t => t.PostCount);
                default:
                    throw new ArgumentException("ThreadSortOrder not recognised");
            };
        }
    }
}
