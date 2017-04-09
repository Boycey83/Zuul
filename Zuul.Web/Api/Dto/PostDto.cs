using System;
using System.Linq;
using System.Collections.Generic;
using Zuul.Model;

namespace Zuul.Web.Api.Dto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PostedByUsername { get; set; }
        public string PostedByEmailAddress { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public IEnumerable<PostDto> Replies { get; set; }
        public int ThreadId { get; set; }

        public static PostDto BuildFromPost(Post post)
        {
            return new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Message = post.Message,
                PostedByUsername = post.PostedBy.Username,
                PostedByEmailAddress = post.PostedBy.EmailAddress,
                CreatedDateTimeUtc = post.CreatedDateTimeUtc,
                Replies = post.Replies.Select(PostDto.BuildFromPost),
                ThreadId = post.Thread.Id
            };
        }
    }
}