using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server.Models.Data
{
    class UserDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UserDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                new User { User_Id = 1, Login = "Login1", User_Name = "Арбузов Арбуз Арбузович", Password = "password", user_token = String.Empty },
                new User { User_Id = 2, Login = "Login2", User_Name = "Дынев Дынь Дыньевич",  Password = "qwerty", user_token = String.Empty},
                new User { User_Id = 3, Login = "Login3", User_Name = "Кивиев Кивь Кивиевич", Password = "podsolnuh", user_token = String.Empty }
                }); 
        }
    }
}
