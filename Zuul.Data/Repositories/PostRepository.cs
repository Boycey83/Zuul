﻿using NHibernate;
using System.Linq;
using System.Collections.Generic;
using Zuul.Model;
using NHibernate.Transform;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Zuul.Data.Repositories
{
    public class PostRepository : RepositoryBase, IPostRepository
    {
        private ISession _session;

        public PostRepository(ISession session)
        {
            _session = session;
        }

        public Post GetById(int id)
        {
            return _session.Get<Post>(id);
        }

        public int CreatePost(Post reply)
        {
            return (int)_session.Save(reply);
        }

        public IEnumerable<Post> GetTopPostsByThreadId(int threadId)
        {
            return
                _session
                    .CreateCriteria<Post>()
                    .Add(Restrictions.Eq("Thread.Id", threadId))
                    .AddOrder(Order.Asc("CreatedDateTimeUtc"))
                    .SetFetchMode("Replies", FetchMode.Join).CreateAlias("Replies", "r", JoinType.LeftOuterJoin)
                    .AddOrder(Order.Asc("r.CreatedDateTimeUtc"))
                    .SetFetchMode("PostedBy", FetchMode.Join)
                    .SetResultTransformer(new DistinctRootEntityResultTransformer())
                    .SetMaxResults(1250)
                    .List<Post>()
                    .Where(p => p.Parent == null);
        }
    }
}
