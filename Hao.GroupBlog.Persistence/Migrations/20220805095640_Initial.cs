using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hao.GroupBlog.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Column",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Logo = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Intro = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TopicId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Column", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Favorite",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    NoteId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    ColumnId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileResource",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    OwnId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Suffix = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileResource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    Password = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Limited = table.Column<bool>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContentId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    ColumnId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Keys = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Hits = table.Column<int>(type: "INTEGER", nullable: false),
                    Opened = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProfileName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Intro = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedById = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteContent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Backups1 = table.Column<string>(type: "TEXT", nullable: true),
                    Backups2 = table.Column<string>(type: "TEXT", nullable: true),
                    Backups3 = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteContent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sequence",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupKey = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    TrgetId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Logo = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    DomainId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLastLoginRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoginId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MemberId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLastLoginRecord", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Domain_Name",
                table: "Domain",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_ColumnId",
                table: "Favorite",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_MemberId",
                table: "Favorite",
                column: "MemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_NoteId",
                table: "Favorite",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FileResource_Code",
                table: "FileResource",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileResource_OwnId",
                table: "FileResource",
                column: "OwnId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_UserName",
                table: "Member",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_ContentId",
                table: "Note",
                column: "ContentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sequence_GroupKey",
                table: "Sequence",
                column: "GroupKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLastLoginRecord_LoginId",
                table: "UserLastLoginRecord",
                column: "LoginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLastLoginRecord_MemberId",
                table: "UserLastLoginRecord",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Column");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropTable(
                name: "FileResource");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "NoteContent");

            migrationBuilder.DropTable(
                name: "Sequence");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropTable(
                name: "UserLastLoginRecord");
        }
    }
}
