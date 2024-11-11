using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WatchIt.Database.Migrations
{
    /// <inheritdoc />
    public partial class MediumTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountProfilePictures",
                schema: "accounts",
                table: "AccountProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_AccountProfilePictures_Id",
                schema: "accounts",
                table: "AccountProfilePictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountFollows",
                schema: "accounts",
                table: "AccountFollows");

            migrationBuilder.DropIndex(
                name: "IX_AccountFollows_FollowerId",
                schema: "accounts",
                table: "AccountFollows");

            migrationBuilder.DropIndex(
                name: "IX_AccountFollows_Id",
                schema: "accounts",
                table: "AccountFollows");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "accounts",
                table: "AccountProfilePictures");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "accounts",
                table: "AccountFollows");

            migrationBuilder.EnsureSchema(
                name: "genres");

            migrationBuilder.EnsureSchema(
                name: "media");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "genders",
                table: "Genders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountProfilePictures",
                schema: "accounts",
                table: "AccountProfilePictures",
                column: "AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountFollows",
                schema: "accounts",
                table: "AccountFollows",
                columns: new[] { "FollowerId", "FollowedId" });

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "genres",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    OriginalTitle = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Duration = table.Column<long>(type: "bigint", nullable: true),
                    ReleaseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Budget = table.Column<decimal>(type: "money", nullable: true),
                    HasEnded = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediumGenres",
                schema: "media",
                columns: table => new
                {
                    MediumId = table.Column<long>(type: "bigint", nullable: false),
                    GenreId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumGenres", x => new { x.GenreId, x.MediumId });
                    table.ForeignKey(
                        name: "FK_MediumGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "genres",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediumGenres_Media_MediumId",
                        column: x => x.MediumId,
                        principalSchema: "media",
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Id",
                schema: "genres",
                table: "Genres",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_Id",
                schema: "media",
                table: "Media",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediumGenres_MediumId",
                schema: "media",
                table: "MediumGenres",
                column: "MediumId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediumGenres",
                schema: "media");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "genres");

            migrationBuilder.DropTable(
                name: "Media",
                schema: "media");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountProfilePictures",
                schema: "accounts",
                table: "AccountProfilePictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountFollows",
                schema: "accounts",
                table: "AccountFollows");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "genders",
                table: "Genders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "accounts",
                table: "AccountProfilePictures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "accounts",
                table: "AccountFollows",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountProfilePictures",
                schema: "accounts",
                table: "AccountProfilePictures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountFollows",
                schema: "accounts",
                table: "AccountFollows",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccountProfilePictures_Id",
                schema: "accounts",
                table: "AccountProfilePictures",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountFollows_FollowerId",
                schema: "accounts",
                table: "AccountFollows",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountFollows_Id",
                schema: "accounts",
                table: "AccountFollows",
                column: "Id",
                unique: true);
        }
    }
}
