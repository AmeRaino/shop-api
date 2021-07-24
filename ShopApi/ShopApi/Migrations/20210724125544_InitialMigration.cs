using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Birthday = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordReset = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOption",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOption", x => new { x.ProductId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_ProductOption_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSku",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SkuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sku = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSku", x => new { x.ProductId, x.SkuId });
                    table.ForeignKey(
                        name: "FK_ProductSku_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthenticationProvider",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    KeyProvided = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ProviderType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthenticationProvider_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccount_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionValue",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    ValueName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionValue", x => new { x.ProductId, x.OptionId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_ProductOptionValue_ProductOption_ProductId_OptionId",
                        columns: x => new { x.ProductId, x.OptionId },
                        principalTable: "ProductOption",
                        principalColumns: new[] { "ProductId", "OptionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSkuValue",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SkuId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: false),
                    ProductSkuProductId = table.Column<int>(type: "int", nullable: true),
                    ProductSkuSkuId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSkuValue", x => new { x.ProductId, x.SkuId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_ProductSkuValue_ProductOption_ProductId_OptionId",
                        columns: x => new { x.ProductId, x.OptionId },
                        principalTable: "ProductOption",
                        principalColumns: new[] { "ProductId", "OptionId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSkuValue_ProductOptionValue_ProductId_OptionId_ValueId",
                        columns: x => new { x.ProductId, x.OptionId, x.ValueId },
                        principalTable: "ProductOptionValue",
                        principalColumns: new[] { "ProductId", "OptionId", "ValueId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSkuValue_ProductSku_ProductId_SkuId",
                        columns: x => new { x.ProductId, x.SkuId },
                        principalTable: "ProductSku",
                        principalColumns: new[] { "ProductId", "SkuId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSkuValue_ProductSku_ProductSkuProductId_ProductSkuSkuId",
                        columns: x => new { x.ProductSkuProductId, x.ProductSkuSkuId },
                        principalTable: "ProductSku",
                        principalColumns: new[] { "ProductId", "SkuId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationProvider_UserId",
                table: "AuthenticationProvider",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSku_Sku",
                table: "ProductSku",
                column: "Sku");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSkuValue_ProductId_OptionId_ValueId",
                table: "ProductSkuValue",
                columns: new[] { "ProductId", "OptionId", "ValueId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSkuValue_ProductSkuProductId_ProductSkuSkuId",
                table: "ProductSkuValue",
                columns: new[] { "ProductSkuProductId", "ProductSkuSkuId" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_UserId",
                table: "UserAccount",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticationProvider");

            migrationBuilder.DropTable(
                name: "ProductSkuValue");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "ProductOptionValue");

            migrationBuilder.DropTable(
                name: "ProductSku");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ProductOption");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
