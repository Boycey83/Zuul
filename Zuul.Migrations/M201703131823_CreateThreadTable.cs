using FluentMigrator;

namespace Zuul.Migrations
{
    [Migration(201703131823)]
    public class M201703131823_CreateThreadTable : Migration
    {
        public override void Up()
        {
            Create.Table("Thread")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserAccountId").AsInt32().NotNullable().ForeignKey("UserAccount", "Id")
                .WithColumn("Title").AsString(200).NotNullable()
                .WithColumn("Message").AsString(4000).NotNullable()
                .WithColumn("CreatedDateTimeUtc").AsDateTime().NotNullable()
                .WithColumn("LastPostDateTimeUtc").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Post");
        }
    }
}
