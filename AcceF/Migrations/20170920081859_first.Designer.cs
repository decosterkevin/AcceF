using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AcceF;

namespace AcceF.Migrations
{
    [DbContext(typeof(PartyContext))]
    [Migration("20170920081859_first")]
    partial class first
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("AcceF.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<string>("Name");

                    b.Property<string>("Path");

                    b.HasKey("FileId");

                    b.ToTable("files");
                });

            modelBuilder.Entity("AcceF.Organisateur", b =>
                {
                    b.Property<int>("OrganisateurId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adress");

                    b.Property<string>("City");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Phone_number1");

                    b.Property<string>("Status");

                    b.Property<string>("Surname");

                    b.Property<string>("ZIP");

                    b.HasKey("OrganisateurId");

                    b.ToTable("organisateurs");
                });

            modelBuilder.Entity("AcceF.Party", b =>
                {
                    b.Property<int>("PartyId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Accepted");

                    b.Property<string>("Adress");

                    b.Property<bool>("AlreadyDone");

                    b.Property<string>("City");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Email");

                    b.Property<string>("EmployeesAsStrings");

                    b.Property<string>("FilesAsStrings");

                    b.Property<DateTime>("Firstwrote");

                    b.Property<int>("Frequency");

                    b.Property<DateTime>("Lastwrote");

                    b.Property<string>("Name");

                    b.Property<string>("OrgaAsStrings");

                    b.Property<string>("Phone");

                    b.Property<string>("StandsAsStrings");

                    b.Property<DateTime>("ToDate");

                    b.Property<string>("Url");

                    b.Property<int>("WriteMonthly");

                    b.Property<string>("ZIP");

                    b.HasKey("PartyId");

                    b.ToTable("parties");
                });

            modelBuilder.Entity("AcceF.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AVS");

                    b.Property<string>("Adress");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("City");

                    b.Property<string>("Email");

                    b.Property<string>("IBAN");

                    b.Property<string>("Name");

                    b.Property<string>("Phone_number1");

                    b.Property<string>("Phone_number2");

                    b.Property<string>("Surname");

                    b.Property<string>("ToolsAsStrings");

                    b.Property<string>("ZIP");

                    b.HasKey("PersonId");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("AcceF.Stand", b =>
                {
                    b.Property<int>("StandId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmployeesAsStrings");

                    b.Property<string>("Name");

                    b.Property<string>("ToolsAsStrings");

                    b.Property<string>("comment");

                    b.HasKey("StandId");

                    b.ToTable("stands");
                });

            modelBuilder.Entity("AcceF.Tool", b =>
                {
                    b.Property<int>("ToolId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.HasKey("ToolId");

                    b.ToTable("tools");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Tool");
                });

            modelBuilder.Entity("AcceF.Machine", b =>
                {
                    b.HasBaseType("AcceF.Tool");

                    b.Property<string>("Descriptif");

                    b.ToTable("Machine");

                    b.HasDiscriminator().HasValue("Machine");
                });

            modelBuilder.Entity("AcceF.Product", b =>
                {
                    b.HasBaseType("AcceF.Tool");

                    b.Property<string>("Descriptif");

                    b.ToTable("Product");

                    b.HasDiscriminator().HasValue("Product");
                });

            modelBuilder.Entity("AcceF.Skill", b =>
                {
                    b.HasBaseType("AcceF.Tool");

                    b.Property<string>("Descriptif");

                    b.ToTable("Skill");

                    b.HasDiscriminator().HasValue("Skill");
                });

            modelBuilder.Entity("AcceF.Structure", b =>
                {
                    b.HasBaseType("AcceF.Tool");

                    b.Property<string>("Descriptif");

                    b.ToTable("Structure");

                    b.HasDiscriminator().HasValue("Structure");
                });
        }
    }
}
