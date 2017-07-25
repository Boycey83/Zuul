using FluentNHibernate.Mapping;
using Zuul.Model;

namespace Vigo.DataAccess.NHibernate.Mapping
{
    public class UserAccountMap : ClassMap<UserAccount>
    {
        public UserAccountMap()
        {
            Id(x => x.Id);
            Map(x => x.Username).Length(50).Unique().Not.Nullable();
            Map(x => x.EmailAddress).Length(256).Unique().Not.Nullable();
            Map(x => x.PasswordSalt).Length(24).Not.Nullable();
            Map(x => x.PasswordHash).Length(64).Not.Nullable();
            Map(x => x.Token).Not.Nullable();
            Map(x => x.ResetToken).Nullable();
            Map(x => x.ResetTokenExpiry).Nullable();
            Map(x => x.IsActivated);
        }
	}
}
