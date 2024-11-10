using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WatchIt.Database.Migrations
{
    /// <inheritdoc />
    public partial class AccountsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "accounts");

            migrationBuilder.EnsureSchema(
                name: "genders");

            migrationBuilder.CreateTable(
                name: "Genders",
                schema: "genders",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Password = table.Column<byte[]>(type: "bytea", maxLength: 1000, nullable: false),
                    LeftSalt = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RightSalt = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    JoinDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    ActiveDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    GenderId = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Genders_GenderId",
                        column: x => x.GenderId,
                        principalSchema: "genders",
                        principalTable: "Genders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountFollows",
                schema: "accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowerId = table.Column<long>(type: "bigint", nullable: false),
                    FollowedId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountFollows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountFollows_Accounts_FollowedId",
                        column: x => x.FollowedId,
                        principalSchema: "accounts",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountFollows_Accounts_FollowerId",
                        column: x => x.FollowerId,
                        principalSchema: "accounts",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountProfilePictures",
                schema: "accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", maxLength: -1, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UploadDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProfilePictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountProfilePictures_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "accounts",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountRefreshTokens",
                schema: "accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsExtendable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRefreshTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "accounts",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountFollows_FollowedId",
                schema: "accounts",
                table: "AccountFollows",
                column: "FollowedId");

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

            migrationBuilder.CreateIndex(
                name: "IX_AccountProfilePictures_AccountId",
                schema: "accounts",
                table: "AccountProfilePictures",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountProfilePictures_Id",
                schema: "accounts",
                table: "AccountProfilePictures",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRefreshTokens_AccountId",
                schema: "accounts",
                table: "AccountRefreshTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRefreshTokens_Id",
                schema: "accounts",
                table: "AccountRefreshTokens",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_GenderId",
                schema: "accounts",
                table: "Accounts",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Id",
                schema: "accounts",
                table: "Accounts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genders_Id",
                schema: "genders",
                table: "Genders",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountFollows",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "AccountProfilePictures",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "AccountRefreshTokens",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "Genders",
                schema: "genders");
        }
    }
}
