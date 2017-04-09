using FluentNHibernate.Mapping;
using Zuul.Model;

namespace Zuul.Data.Mapping
{
    public class ThreadMap : ClassMap<Thread>
    {
        public ThreadMap()
        {
            Id(x => x.Id);
            References(x => x.PostedBy, "UserAccountId");
            Map(x => x.Title);
            Map(x => x.Message);
            Map(x => x.CreatedDateTimeUtc);
            Map(x => x.LastPostDateTimeUtc);
            Map(x => x.PostCount).Formula("(SELECT COUNT(Post.Id) FROM Post WHERE Post.ThreadId = Id)");
        }
    }
}
