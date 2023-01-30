using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenPhotos.Core.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    DateTaken = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Iso = table.Column<string>(type: "text", nullable: false),
                    FStop = table.Column<string>(type: "text", nullable: false),
                    FocalLength = table.Column<string>(type: "text", nullable: false),
                    Longitude = table.Column<int>(type: "integer", nullable: false),
                    Attitude = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Confidence = table.Column<int>(type: "integer", nullable: false),
                    MyProperty = table.Column<int>(type: "integer", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_PhotoId",
                table: "Tag",
                column: "PhotoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}
