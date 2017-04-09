using FluentMigrator;

namespace Zuul.Migrations
{
    [Migration(201703131826)]
    public class M201703131826_AddThreadIdColumnToPostTable : Migration
    {
        public override void Up()
        {
            Create.Column("ThreadId")
                .OnTable("Post")
                .AsInt32()
                .ForeignKey("Thread", "Id");
        }

        public override void Down()
        {
            Delete.Column("ThreadId")
                .FromTable("Post");
        }
    }
}
