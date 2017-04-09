using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using System.Linq;
using Zuul.BusinessLogic.Services;
using Zuul.Web.Api.Dto;
using Zuul.Model.Enums;
using Zuul.Resources;

namespace Zuul.Web.Api
{
    [RoutePrefix("api/Forum")]
    public class ForumController : ApiController
    {
        private const int PageSize = 25;
        private readonly PostService _postService;
        private readonly ThreadService _threadService;

        public ForumController(PostService postService, ThreadService threadService)
        {
            _postService = postService;
            _threadService = threadService;
        }

        #region Public Actions

        [HttpGet]
        [Route("Threads")]
        public ThreadsContextDto GetThreads()
        {
            return GetThreads(ThreadSortOrder.DateDesc, 1);
        }

        [HttpGet]
        [Route("Threads/Page/{pageNumber:int}")]
        public ThreadsContextDto GetThreads(int pageNumber)
        {
            return GetThreads(ThreadSortOrder.DateDesc, pageNumber);
        }

        [HttpGet]
        [Route("Threads/Sort/{sortOrder}")]
        public ThreadsContextDto GetThreads(ThreadSortOrder sortOrder)
        {
            return GetThreads(sortOrder, 1);
        }

        [HttpGet]
        [Route("Threads/Sort/{sortOrder}/Page/{pageNumber:int}")]
        public ThreadsContextDto GetThreads(ThreadSortOrder sortOrder, int pageNumber)
        {
            var threads = _threadService.GetTopThreads(sortOrder, pageNumber, PageSize);
            var threadCount = _threadService.GetThreadCount();
            return new ThreadsContextDto
            {
                Threads = threads.Select(ThreadDto.BuildFromThread),
                ThreadCount = threadCount,
                LoggedInAsUsername = User.Identity.GetUserName(),
                PageNumber = pageNumber
            };
        }

        [HttpGet]
        [Route("Thread/{threadId:int}/Replies")]
        public IEnumerable<PostDto> GetThreadReplies(int threadId)
        {
            var posts = _postService.GetThreadReplies(threadId);
            return posts.Select(PostDto.BuildFromPost);
        }

        #endregion

        #region Logged In Actions

        [HttpPost]
        [Route("Thread")]
        public ThreadDto CreateNewThread(CreatePostDto postDto)
        {
            int userId = 0;
            if (int.TryParse(User.Identity.GetUserId(), out userId))
            {
                var thread = _threadService.CreateThread(userId, postDto.Title, postDto.Message);
                return ThreadDto.BuildFromThread(thread);
            };
            throw new ValidationException(ExceptionMessages.CreateThreadMustBeLoggedIn);
        }

        [HttpPost]
        [Route("Thread/{threadId:int}/Reply")]
        public PostDto CreateReplyToThread(int threadId, CreatePostDto postDto)
        {
            int userId = 0;
            if (int.TryParse(User.Identity.GetUserId(), out userId))
            {
                var post = _postService.CreatePost(threadId, null, userId, postDto.Title, postDto.Message);
                return PostDto.BuildFromPost(post);
            };
            throw new ValidationException(ExceptionMessages.CreatePostMustBeLoggedIn);
        }

        [HttpPost]
        [Route("Thread/{threadId:int}/Post/{postId:int}/Reply")]
        public PostDto CreateReplyToPost(int threadId, int postId, CreatePostDto postDto)
        {
            int userId = 0;
            if (int.TryParse(User.Identity.GetUserId(), out userId))
            {
                var post = _postService.CreatePost(threadId, postId, userId, postDto.Title, postDto.Message);
                return PostDto.BuildFromPost(post);
            };
            throw new ValidationException(ExceptionMessages.CreatePostMustBeLoggedIn);
        }

        #endregion
    }
}