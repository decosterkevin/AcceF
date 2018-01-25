using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AcceF
{
    public class PartyContext : DbContext
    {
        public DbSet<Party> parties { get; set; }
        public DbSet<Person> employees { get; set; }
        public DbSet<Stand> stands { get; set; }
        public DbSet<Machine> machines { get; set; }
        public DbSet<Structure> structures { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Skill> skills { get; set; }
        public DbSet<File> files { get; set; }
        public DbSet<Organisateur> organisateurs { get; set; }
        public DbSet<Tool> tools { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=acceF.db");
        }
    }
    
    public class Party
    {
        [Key]
        public int PartyId { get; set; }
        public string Name { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime Lastwrote { get; set; }
        public System.DateTime Firstwrote { get; set; }
        public bool Accepted { get; set; }
        public bool AlreadyDone { get; set; } = false;
        public string Adress { get; set; }
        public string City { get; set; }
        public string ZIP { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string EmployeesAsStrings { get; set; }
        public string OrgaAsStrings { get; set; }
        public string FilesAsStrings { get; set; }
        public string StandsAsStrings { get; set; }
        public System.DateTime ToDate { get; set; }
        public string Phone { get; set; }
        public int WriteMonthly { get; set; }
        public int Frequency { get; set; } = 12;

        public string DateText {
            get {
                return Date.ToString("d");
            }
            set { } }
        public string LastwroteText
        {
            get
            {
                return Lastwrote.ToString("d");
            }
            set { }
        }
        public string FirstwroteText
        {
            get
            {
                return Firstwrote.ToString("d");
            }
            set { }
        }
        public string ToDateTextText
        {
            get
            {
                return ToDate.ToString("d");
            }
            set { }
        }
        [NotMapped]
        public List<string> OrgaIDs
        {
            get {
                if (OrgaAsStrings != null)
                {
                    return OrgaAsStrings.Split(',').ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                OrgaAsStrings = string.Join(",", value);
            }
        }
        
        
        [NotMapped]
        public List<string> FilesIDs
        {
            get {
                if (FilesAsStrings != null)
                {
                    return FilesAsStrings.Split(',').ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                FilesAsStrings = string.Join(",", value);
            }
        }
        [NotMapped]
        public List<string> StandsIDs
        {
            get
            {
                if (StandsAsStrings != null)
                {
                    return StandsAsStrings.Split(',').ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                StandsAsStrings = string.Join(",", value);
            }
        }
        public int MonthToGo
        {
            get
            {
                return  this.Date.Month - DateTime.Today.Month;
            }
        }
        

    }

    public class Person
    {
        [Key]
        public int  PersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Phone_number1 { get; set; }
        public string Phone_number2 { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string ZIP { get; set; }
        public string Email { get; set; }
        public string AVS { get; set; }
        public int Age
        {
            get
            {
                // Save today's date.
                var today = System.DateTime.Today;
                // Calculate the age.
                var age = today.Year - this.BirthDate.Year;
                // Go back to the year the person was born in case of a leap year
                if (this.BirthDate.Year > today.AddYears(-age).Year) age--;

                return age;
            }
        }
        public string IBAN { get; set; }
        public string ToolsAsStrings
        {
            get; set;
        }
        [NotMapped]
        public List<string> ToolIDs
        {
            get
            {
                if (ToolsAsStrings != null)
                {
                    return ToolsAsStrings.Split(',').ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                ToolsAsStrings = string.Join(",", value);
            }
        }

    }
    public class Organisateur
    {
        [Key]
        public int OrganisateurId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone_number1 { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string ZIP { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }


    }
    public class Stand
    {
        [Key]
        public int StandId { get; set; }
        public string Name { get; set; }
        public string comment { get; set; }
        public string EmployeesAsStrings { get; set; }
        public string ToolsAsStrings
        {
            get; set;
        }
    
        [NotMapped]
        public List<string> EmployeesIDs
        {
            get
            {
                if (EmployeesAsStrings != null)
                {
                    return EmployeesAsStrings.Split(',').ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                EmployeesAsStrings = string.Join(",", value);
            }
        }
        [NotMapped]
        public List<string> ToolIDs
        {
            get
            {
                if (ToolsAsStrings != null)
                {
                    return ToolsAsStrings.Split(',').ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                ToolsAsStrings = string.Join(",", value);
            }
        }




    }

    public class Tool
    {
        [Key]
        public int ToolId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }
    public class Skill : Tool
    {
        public string Descriptif { get; set; }
    }
    public class Product : Tool
    {
        public string Descriptif { get; set; }
    }
    public class Structure : Tool
    {
        public string Descriptif { get; set; }
    }
    public class Machine : Tool
    {
        public string Descriptif { get; set; }
    }
    public class File
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Comment { get; set; }
    }

}