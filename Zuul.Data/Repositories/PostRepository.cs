using NHibernate;
using System.Linq;
using System.Collections.Generic;
using Zuul.Model;
using NHibernate.Transform;

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
                    .SetFetchMode("Replies", FetchMode.Join)
                    .SetResultTransformer(new DistinctRootEntityResultTransformer())
                    .SetMaxResults(3000)
                    .List<Post>()
                    .Where(p => p.Parent == null && p.Thread.Id == threadId);
        }
    }
}
