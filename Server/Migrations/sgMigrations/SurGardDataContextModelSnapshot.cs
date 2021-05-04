﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server.Models.Data;

namespace Server.Migrations.sgMigrations
{
    [DbContext(typeof(SurGardDataContext))]
    partial class SurGardDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Server.Models.di_codes", b =>
                {
                    b.Property<int>("id_code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id_code");

                    b.ToTable("Codes");

                    b.HasData(
                        new
                        {
                            id_code = 1,
                            code = "303",
                            name = "Линейная алгебра"
                        },
                        new
                        {
                            id_code = 2,
                            code = "403",
                            name = "Нелинейный русский язык"
                        },
                        new
                        {
                            id_code = 3,
                            code = "703",
                            name = "Арифметическая география"
                        });
                });

            modelBuilder.Entity("Server.Models.di_devices", b =>
                {
                    b.Property<int>("id_device")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id_device");

                    b.ToTable("Devices");

                    b.HasData(
                        new
                        {
                            id_device = 1,
                            name = "Дмитрий",
                            number = "01"
                        },
                        new
                        {
                            id_device = 2,
                            name = "Вафлий",
                            number = "02"
                        },
                        new
                        {
                            id_device = 3,
                            name = "Печений",
                            number = "03"
                        });
                });

            modelBuilder.Entity("Server.Models.di_groups", b =>
                {
                    b.Property<int>("id_group")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id_group");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            id_group = 1,
                            code = "01",
                            name = "Г01"
                        },
                        new
                        {
                            id_group = 2,
                            code = "02",
                            name = "Г02"
                        },
                        new
                        {
                            id_group = 3,
                            code = "99",
                            name = "Г03"
                        });
                });

            modelBuilder.Entity("Server.Models.jr_surgard", b =>
                {
                    b.Property<int>("id_surgard")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("date_action")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("di_codesid_code")
                        .HasColumnType("integer");

                    b.Property<int?>("di_devicesid_device")
                        .HasColumnType("integer");

                    b.Property<int?>("di_groupsid_group")
                        .HasColumnType("integer");

                    b.Property<int?>("id_code")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("id_device")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("id_group")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("id_surgard");

                    b.HasIndex("di_codesid_code");

                    b.HasIndex("di_devicesid_device");

                    b.HasIndex("di_groupsid_group");

                    b.ToTable("Journal");
                });

            modelBuilder.Entity("Server.Models.jr_surgard", b =>
                {
                    b.HasOne("Server.Models.di_codes", "di_codes")
                        .WithMany("jr_surgard")
                        .HasForeignKey("di_codesid_code");

                    b.HasOne("Server.Models.di_devices", "di_devices")
                        .WithMany("jr_surgard")
                        .HasForeignKey("di_devicesid_device");

                    b.HasOne("Server.Models.di_groups", "di_groups")
                        .WithMany("jr_surgard")
                        .HasForeignKey("di_groupsid_group");

                    b.Navigation("di_codes");

                    b.Navigation("di_devices");

                    b.Navigation("di_groups");
                });

            modelBuilder.Entity("Server.Models.di_codes", b =>
                {
                    b.Navigation("jr_surgard");
                });

            modelBuilder.Entity("Server.Models.di_devices", b =>
                {
                    b.Navigation("jr_surgard");
                });

            modelBuilder.Entity("Server.Models.di_groups", b =>
                {
                    b.Navigation("jr_surgard");
                });
#pragma warning restore 612, 618
        }
    }
}
