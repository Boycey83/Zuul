using System;
using System.Collections.Generic;
using Zuul.BusinessLogic.Exceptions;
using Zuul.Data.Repositories;
using Zuul.Model;
using Zuul.Model.Enums;
using Zuul.Resources;

namespace Zuul.BusinessLogic.Services
{
    public class ThreadService
    {
        private readonly IThreadRepository _threadRepository;
        private readonly IUserAccountRepository _userAccountRepository;

        public ThreadService(IUserAccountRepository userAccountRepository, IThreadRepository threadRepository)
        {
            _userAccountRepository = userAccountRepository;
            _threadRepository = threadRepository;
        }

        public Thread CreateThread(int userId, string title, string message)
        {
            title = title.Trim();
            message = message.Trim();
            var postedBy = _userAccountRepository.GetById(userId);
            ValidateCreateThread(postedBy, title, message);
            var createdDateTimeUtc = DateTime.UtcNow;
            var thread = new Thread(postedBy, title, message, createdDateTimeUtc);
            var threadId = _threadRepository.CreateThread(thread);
            thread.Id = threadId;
            return thread;
        }

        public IEnumerable<Thread> GetTopThreads(ThreadSortOrder sortOrder, int pageNumber, int pageSize)
        {
            return _threadRepository.GetTopThreads(sortOrder, pageNumber, pageSize);
        }

        public int GetThreadCount()
        {
            return _threadRepository.GetThreadCount();
        }

        private void ValidateCreateThread(UserAccount postedBy, string title, string message)
        {
            if (postedBy == null)
            {
                throw new ValidationException(ExceptionMessages.CreateThreadUserAccountNotFound);
            }
            if (title == null || title.Length < 3 || title.Length > 200)
            {
                throw new ValidationException(ExceptionMessages.CreateThreadTitleError);
            }
            if (message == null || message.Length < 3 || message.Length > 4000)
            {
                throw new ValidationException(ExceptionMessages.CreateThreadMessageError);
            }
        }
    }
}
