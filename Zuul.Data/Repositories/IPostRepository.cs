using System.Collections.Generic;
using Zuul.Model;

namespace Zuul.Data.Repositories
{
    public interface IPostRepository
    {
        Post GetById(int value);
        int CreatePost(Post reply);
        IEnumerable<Post> GetTopPostsByThreadId(int threadId);
    }
}
