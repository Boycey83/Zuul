using System;
using System.Collections.Generic;
using Zuul.BusinessLogic.Exceptions;
using Zuul.Data.Repositories;
using Zuul.Model;
using Zuul.Resources;

namespace Zuul.BusinessLogic.Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IThreadRepository _threadRepository;
        private readonly IUserAccountRepository _userAccountRepository;

        public PostService(IUserAccountRepository userAccountRepository, IPostRepository postRepository, IThreadRepository threadRepository)
        {
            _userAccountRepository = userAccountRepository;
            _postRepository = postRepository;
            _threadRepository = threadRepository;
        }

        public Post CreatePost(int threadId, int? parentPostId, int userId, string title, string message)
        {
            title = title.Trim();
            message = message.Trim();
            var parent = parentPostId.HasValue ? _postRepository.GetById(parentPostId.Value) : null;
            var thread = _threadRepository.GetById(threadId);
            var postedBy = _userAccountRepository.GetById(userId);
            if (parentPostId.HasValue)
            {
                ValidateCreatePost(postedBy, thread, parent, title, message);
            }
            else
            {
                ValidateCreatePost(postedBy, thread, title, message);
            }
            var createdDateTimeUtc = DateTime.UtcNow;
            var post = new Post(thread, parent, postedBy, title, message, createdDateTimeUtc);
            var postId = _postRepository.CreatePost(post);
            _threadRepository.UpdateLastPostDateTimeUtc(threadId, createdDateTimeUtc);
            post.Id = postId;
            return post;
        }

        public IEnumerable<Post> GetThreadReplies(int threadId)
        {
            return _postRepository.GetTopPostsByThreadId(threadId);
        }

        #region Validation Methods

        private void ValidateCreatePost(UserAccount postedBy, Thread thread, string title, string message)
        {
            if (postedBy == null)
            {
                throw new ValidationException(ExceptionMessages.CreatePostUserAccountNotFound);
            }
            if (thread == null)
            {
                throw new ValidationException(ExceptionMessages.CreatePostThreadNotFound);
            }
            if (title == null || title.Length < 3 || title.Length > 200)
            {
                throw new ValidationException(ExceptionMessages.CreatePostTitleError);
            }
            if (message == null ||  message.Length < 3 || message.Length > 4000)
            {
                throw new ValidationException(ExceptionMessages.CreatePostMessageError);
            }
        }

        private void ValidateCreatePost(UserAccount postedBy, Thread thread, Post parent, string title, string message)
        {
            if (parent == null)
            {
                throw new ValidationException(ExceptionMessages.CreatePostParentPostNotFound);
            }
            ValidateCreatePost(postedBy, thread, title, message);
        }

        #endregion
    }
}
