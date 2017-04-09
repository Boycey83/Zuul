using FluentMigrator;

namespace Zuul.Migrations
{
    [Migration(201703121434)]
    public class M201703121434_CreatePostTable : Migration
    {
        public override void Up()
        {
            Create.Table("Post")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserAccountId").AsInt32().NotNullable().ForeignKey("UserAccount", "Id")
                .WithColumn("ParentPostId").AsInt32().Nullable().ForeignKey("Post", "Id")
                .WithColumn("Title").AsString(200).NotNullable()
                .WithColumn("Message").AsString(4000).NotNullable()
                .WithColumn("CreatedDateTimeUtc").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Post");
        }
    }
}
