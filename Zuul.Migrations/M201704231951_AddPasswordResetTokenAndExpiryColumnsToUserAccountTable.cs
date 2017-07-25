using FluentMigrator;

namespace Zuul.Migrations
{
    [Migration(201704231951)]
    public class M201704231951_AddPasswordResetTokenAndExpiryColumnsToUserAccountTable : Migration
    {
        public override void Up()
        {
            Create.Column("ResetToken")
                .OnTable("UserAccount")
                .AsGuid()
                .Nullable();
            Create.Column("ResetTokenExpiry")
                .OnTable("UserAccount")
                .AsDateTime()
                .Nullable();
        }

        public override void Down()
        {
            Delete.Column("ResetTokenExpiry")
                .FromTable("UserAccount");
            Delete.Column("ResetToken")
                .FromTable("UserAccount");
        }
    }
}
