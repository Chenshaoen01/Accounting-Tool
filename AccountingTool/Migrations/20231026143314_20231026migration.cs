using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingTool.Migrations
{
    /// <inheritdoc />
    public partial class _20231026migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "AccountingDatas",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AccountingDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AccountingDatas");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Time",
                table: "AccountingDatas",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }
    }
}
