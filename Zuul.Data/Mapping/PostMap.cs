using FluentNHibernate.Mapping;
using Zuul.Model;

namespace Zuul.Data.Mapping
{
    public class PostMap : ClassMap<Post>
    {
        public PostMap()
        {
            Id(x => x.Id);
            References(x => x.PostedBy, "UserAccountId");
            HasMany(x => x.Replies).Table("Reply").KeyColumns.Add("ParentPostId");
            References(x => x.Parent, "ParentPostId");
            References(x => x.Thread, "ThreadId");
            Map(x => x.Title);
            Map(x => x.Message);
            Map(x => x.CreatedDateTimeUtc);
        }
    }
}
