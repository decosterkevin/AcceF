using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceF.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Comment = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "organisateurs",
                columns: table => new
                {
                    OrganisateurId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Adress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Phone_number1 = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    ZIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organisateurs", x => x.OrganisateurId);
                });

            migrationBuilder.CreateTable(
                name: "parties",
                columns: table => new
                {
                    PartyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Accepted = table.Column<bool>(nullable: false),
                    Adress = table.Column<string>(nullable: true),
                    AlreadyDone = table.Column<bool>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmployeesAsStrings = table.Column<string>(nullable: true),
                    FilesAsStrings = table.Column<string>(nullable: true),
                    Firstwrote = table.Column<DateTime>(nullable: false),
                    Frequency = table.Column<int>(nullable: false),
                    Lastwrote = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OrgaAsStrings = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    StandsAsStrings = table.Column<string>(nullable: true),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    WriteMonthly = table.Column<int>(nullable: false),
                    ZIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parties", x => x.PartyId);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AVS = table.Column<string>(nullable: true),
                    Adress = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Phone_number1 = table.Column<string>(nullable: true),
                    Phone_number2 = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    ToolsAsStrings = table.Column<string>(nullable: true),
                    ZIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "stands",
                columns: table => new
                {
                    StandId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeesAsStrings = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ToolsAsStrings = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stands", x => x.StandId);
                });

            migrationBuilder.CreateTable(
                name: "tools",
                columns: table => new
                {
                    ToolId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Descriptif = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tools", x => x.ToolId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "organisateurs");

            migrationBuilder.DropTable(
                name: "parties");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "stands");

            migrationBuilder.DropTable(
                name: "tools");
        }
    }
}
