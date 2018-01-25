using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceF.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateText",
                table: "parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstwroteText",
                table: "parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastwroteText",
                table: "parties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToDateTextText",
                table: "parties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateText",
                table: "parties");

            migrationBuilder.DropColumn(
                name: "FirstwroteText",
                table: "parties");

            migrationBuilder.DropColumn(
                name: "LastwroteText",
                table: "parties");

            migrationBuilder.DropColumn(
                name: "ToDateTextText",
                table: "parties");
        }
    }
}
