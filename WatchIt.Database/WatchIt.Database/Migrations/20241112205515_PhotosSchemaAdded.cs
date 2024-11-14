using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchIt.Database.Migrations
{
    /// <inheritdoc />
    public partial class PhotosSchemaAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountBackgroundPictures_MediumPhotoBackground_BackgroundId",
                schema: "accounts",
                table: "AccountBackgroundPictures");

            migrationBuilder.DropTable(
                name: "MediumPhotoBackground",
                schema: "media");

            migrationBuilder.DropTable(
                name: "MediumPhotos",
                schema: "media");

            migrationBuilder.EnsureSchema(
                name: "photos");

            migrationBuilder.CreateTable(
                name: "Photos",
                schema: "photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediumId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", maxLength: -1, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UploadDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Media_MediumId",
                        column: x => x.MediumId,
                        principalSchema: "media",
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoBackground",
                schema: "photos",
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
                    table.PrimaryKey("PK_PhotoBackground", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoBackground_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalSchema: "photos",
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoBackground_Id",
                schema: "photos",
                table: "PhotoBackground",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoBackground_PhotoId",
                schema: "photos",
                table: "PhotoBackground",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Id",
                schema: "photos",
                table: "Photos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_MediumId",
                schema: "photos",
                table: "Photos",
                column: "MediumId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountBackgroundPictures_PhotoBackground_BackgroundId",
                schema: "accounts",
                table: "AccountBackgroundPictures",
                column: "BackgroundId",
                principalSchema: "photos",
                principalTable: "PhotoBackground",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountBackgroundPictures_PhotoBackground_BackgroundId",
                schema: "accounts",
                table: "AccountBackgroundPictures");

            migrationBuilder.DropTable(
                name: "PhotoBackground",
                schema: "photos");

            migrationBuilder.DropTable(
                name: "Photos",
                schema: "photos");

            migrationBuilder.CreateTable(
                name: "MediumPhotos",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MediumId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", maxLength: -1, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UploadDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediumPhotos_Media_MediumId",
                        column: x => x.MediumId,
                        principalSchema: "media",
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediumPhotoBackground",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstGradientColor = table.Column<byte[]>(type: "bytea", nullable: false),
                    IsUniversal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_MediumPhotos_Id",
                schema: "media",
                table: "MediumPhotos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediumPhotos_MediumId",
                schema: "media",
                table: "MediumPhotos",
                column: "MediumId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountBackgroundPictures_MediumPhotoBackground_BackgroundId",
                schema: "accounts",
                table: "AccountBackgroundPictures",
                column: "BackgroundId",
                principalSchema: "media",
                principalTable: "MediumPhotoBackground",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
