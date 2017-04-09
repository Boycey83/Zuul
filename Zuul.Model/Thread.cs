using System;
using System.Collections.Generic;

namespace Zuul.Model
{
    public class Thread
    {
        public Thread()
        {
        }

        public Thread(UserAccount postedBy, string title, string message, DateTime createdDateTimeUtc)
            : this()
        {
            PostedBy = postedBy;
            Title = title;
            Message = message;
            CreatedDateTimeUtc = createdDateTimeUtc;
            LastPostDateTimeUtc = createdDateTimeUtc;
        }
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime CreatedDateTimeUtc { get; set; }
        public virtual DateTime LastPostDateTimeUtc { get; set; }
        public virtual UserAccount PostedBy { get; set; }
        public virtual int PostCount { get; set; }
    }
}
