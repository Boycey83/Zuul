using FluentMigrator;

namespace Zuul.Migrations
{
    [Migration(201703261504)]
    public class M201703261504_AddTokenColumnToUserAccountTable : Migration
    {
        public override void Up()
        {
            Create.Column("Token")
                .OnTable("UserAccount")
                .AsGuid()
                .Nullable();
        }

        public override void Down()
        {
            Delete.Column("Token")
                .FromTable("UserAccount");
        }
    }
}
