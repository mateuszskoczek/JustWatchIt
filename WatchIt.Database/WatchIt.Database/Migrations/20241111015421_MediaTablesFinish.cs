using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchIt.Database.Migrations
{
    /// <inheritdoc />
    public partial class MediaTablesFinish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "MediumPictures",
                schema: "media",
                columns: table => new
                {
                    MediumId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", maxLength: -1, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UploadDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumPictures", x => x.MediumId);
                    table.ForeignKey(
                        name: "FK_MediumPictures_Media_MediumId",
                        column: x => x.MediumId,
                        principalSchema: "media",
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediumPhotoBackgroundSettings",
                schema: "media",
                columns: table => new
                {
                    PhotoId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsUniversal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FirstGradientColor = table.Column<byte[]>(type: "bytea", maxLength: 3, nullable: false),
                    SecondGradientColor = table.Column<byte[]>(type: "bytea", maxLength: 3, nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_MediumPictures_MediumId",
                schema: "media",
                table: "MediumPictures",
                column: "MediumId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediumPhotoBackgroundSettings",
                schema: "media");

            migrationBuilder.DropTable(
                name: "MediumPictures",
                schema: "media");

            migrationBuilder.DropTable(
                name: "MediumPhotos",
                schema: "media");
        }
    }
}
