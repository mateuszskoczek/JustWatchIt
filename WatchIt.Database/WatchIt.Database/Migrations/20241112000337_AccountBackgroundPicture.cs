using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchIt.Database.Migrations
{
    /// <inheritdoc />
    public partial class AccountBackgroundPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediumPhotoBackgroundSettings",
                schema: "media");

            migrationBuilder.CreateTable(
                name: "MediumPhotoBackground",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsUniversal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FirstGradientColor = table.Column<byte[]>(type: "bytea", nullable: false),
                    SecondGradientColor = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumPhotoBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediumPhotoBackground_MediumPhotos_PhotoId",
                        column: x => x.PhotoId,
                        principalSchema: "media",
                        principalTable: "MediumPhotos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountBackgroundPictures",
                schema: "accounts",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    BackgroundId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBackgroundPictures", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_AccountBackgroundPictures_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "accounts",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountBackgroundPictures_MediumPhotoBackground_BackgroundId",
                        column: x => x.BackgroundId,
                        principalSchema: "media",
                        principalTable: "MediumPhotoBackground",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountBackgroundPictures_AccountId",
                schema: "accounts",
                table: "AccountBackgroundPictures",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountBackgroundPictures_BackgroundId",
                schema: "accounts",
                table: "AccountBackgroundPictures",
                column: "BackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_MediumPhotoBackground_Id",
                schema: "media",
                table: "MediumPhotoBackground",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediumPhotoBackground_PhotoId",
                schema: "media",
                table: "MediumPhotoBackground",
                column: "PhotoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBackgroundPictures",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "MediumPhotoBackground",
                schema: "media");

            migrationBuilder.CreateTable(
                name: "MediumPhotoBackgroundSettings",
                schema: "media",
                columns: table => new
                {
                    PhotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstGradientColor = table.Column<byte[]>(type: "bytea", nullable: false),
                    IsUniversal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SecondGradientColor = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumPhotoBackgroundSettings", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_MediumPhotoBackgroundSettings_MediumPhotos_PhotoId",
                        column: x => x.PhotoId,
                        principalSchema: "media",
                        principalTable: "MediumPhotos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediumPhotoBackgroundSettings_PhotoId",
                schema: "media",
                table: "MediumPhotoBackgroundSettings",
                column: "PhotoId",
                unique: true);
        }
    }
}
