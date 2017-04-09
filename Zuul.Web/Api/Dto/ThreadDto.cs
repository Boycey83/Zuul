using System;
using System.Linq;
using Zuul.Model;

namespace Zuul.Web.Api.Dto
{
    public class ThreadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PostedByUsername { get; set; }
        public string PostedByEmailAddress { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime LastPostDateTimeUtc { get; set; }
        public int PostCount { get; set; }

        public static ThreadDto BuildFromThread(Thread thread)
        {
            return new ThreadDto
            {
                Id = thread.Id,
                Title = thread.Title,
                Message = thread.Message,
                PostedByUsername = thread.PostedBy.Username,
                PostedByEmailAddress = thread.PostedBy.EmailAddress,
                CreatedDateTimeUtc = thread.CreatedDateTimeUtc,
                LastPostDateTimeUtc = thread.LastPostDateTimeUtc,
                PostCount = thread.PostCount + 1
            };
        }
    }
}