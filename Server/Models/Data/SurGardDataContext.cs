using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Server.Models.Data
{
    class SurGardDataContext : DbContext
    {
        public DbSet<di_devices> Devices { get; set; }

        public DbSet<di_codes> Codes { get; set; }

        public DbSet<di_groups> Groups { get; set; }
        
        public DbSet<jr_surgard> Journal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MasterDBPuppets;Username=postgres;Password=adminpassword"); //изменить
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<di_devices>().HasData(
                new di_devices[]
                {
                new di_devices { id_device = 1, name = "Дмитрий", number = "01" },
                new di_devices { id_device = 2, name = "Вафлий", number = "02" },
                new di_devices { id_device = 3, name = "Печений", number = "03" }
                });
            modelBuilder.Entity<di_groups>().HasData(
                new di_groups[]
                {
                    new di_groups { id_group = 1, name = "Г01", code = "01" },
                    new di_groups { id_group = 2, name = "Г02", code = "02" },
                    new di_groups { id_group = 3, name = "Г03", code = "99" }

                });
            modelBuilder.Entity<di_codes>().HasData(
                new di_codes[]
                {
                    new di_codes { id_code = 1, name = "Линейная алгебра", code = "303" },
                    new di_codes { id_code = 2, name = "Нелинейный русский язык", code = "403" },
                    new di_codes { id_code = 3, name = "Арифметическая география", code = "703" }

                });
        }
    }
}
