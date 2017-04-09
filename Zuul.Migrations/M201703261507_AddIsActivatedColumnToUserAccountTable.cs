using FluentMigrator;

namespace Zuul.Migrations
{
    [Migration(201703261507)]
    public class M201703261507_AddIsActivatedColumnToUserAccountTable : Migration
    {
        public override void Up()
        {
            Create.Column("IsActivated")
                .OnTable("UserAccount")
                .AsBoolean()
                .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("IsActivated")
                .FromTable("UserAccount");
        }
    }
}
