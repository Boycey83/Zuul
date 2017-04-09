using System;
using System.Collections.Generic;
using Zuul.Model;
using Zuul.Model.Enums;

namespace Zuul.Data.Repositories
{
    public interface IThreadRepository
    {
        int CreateThread(Thread thread);
        Thread GetById(int threadId);
        void UpdateLastPostDateTimeUtc(int threadId, DateTime createdDateTimeUtc);
        IEnumerable<Thread> GetTopThreads(ThreadSortOrder sortOrder, int pageNumber, int pageSize);
        int GetThreadCount();
    }
}
