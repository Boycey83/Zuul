using FluentMigrator;

namespace Zuul.Migrations
{
    [Migration(201703071901)]
    public class M201703111856_CreateUserAccountTable : Migration
    {
        public override void Up()
        {
            Create.Table("UserAccount")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Username").AsString(30).Unique().NotNullable()
                .WithColumn("EmailAddress").AsString(256).Unique().NotNullable()
                .WithColumn("PasswordSalt").AsString(24).NotNullable()
                .WithColumn("PasswordHash").AsString(64).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("UserAccount");
        }
    }
}
