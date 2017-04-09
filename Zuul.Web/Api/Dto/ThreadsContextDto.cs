using System.Collections.Generic;

namespace Zuul.Web.Api.Dto
{
    public class ThreadsContextDto
    {
        public string LoggedInAsUsername { get; set; }
        public int PageNumber { get; set; }
        public int ThreadCount { get; set; }
        public IEnumerable<ThreadDto> Threads { get; set; }
    }
}