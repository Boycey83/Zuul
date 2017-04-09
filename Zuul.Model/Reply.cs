using System;
using System.Collections.Generic;

namespace Zuul.Model
{
    public class Post
    {
        public Post()
        {
            Replies = new List<Post>();
        }

        public Post(Thread thread, Post parent, UserAccount postedBy, string title, string message, DateTime createdDateTimeUtc)
            :this()
        {
            Thread = thread;
            Parent = parent;
            PostedBy = postedBy;
            Title = title;
            Message = message;
            CreatedDateTimeUtc = createdDateTimeUtc;
        }

        public virtual int Id { get; set; }
        public virtual Thread Thread { get; set; }
        public virtual Post Parent { get; set; }
        public virtual IEnumerable<Post> Replies { get; set; }
        public virtual string Title { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime CreatedDateTimeUtc { get; set; }
        public virtual UserAccount PostedBy { get; set; }
    }
}
