﻿// <auto-generated />
using System;
using EFCorePgExercises.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCorePgExercises.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211202072003_V2021_12_02_1049")]
    partial class V2021_12_02_1049
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EFCorePgExercises.Entities.Booking", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"), 0L, 1);

                    b.Property<int>("FacId")
                        .HasColumnType("int");

                    b.Property<int>("MemId")
                        .HasColumnType("int");

                    b.Property<int>("Slots")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("BookId");

                    b.HasIndex("StartTime")
                        .HasDatabaseName("IX_starttime");

                    b.HasIndex("FacId", "StartTime")
                        .HasDatabaseName("IX_facid_starttime");

                    b.HasIndex("MemId", "FacId")
                        .HasDatabaseName("IX_memid_facid");

                    b.HasIndex("MemId", "StartTime")
                        .HasDatabaseName("IX_memid_starttime");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("EFCorePgExercises.Entities.Facility", b =>
                {
                    b.Property<int>("FacId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacId"), 0L, 1);

                    b.Property<decimal>("GuestCost")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("InitialOutlay")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("MemberCost")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("MonthlyMaintenance")
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("FacId");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("EFCorePgExercises.Entities.Member", b =>
                {
                    b.Property<int>("MemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemId"), 0L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RecommendedBy")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("MemId");

                    b.HasIndex("JoinDate")
                        .HasDatabaseName("IX_JoinDate");

                    b.HasIndex("RecommendedBy")
                        .HasDatabaseName("IX_RecommendedBy");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("EFCorePgExercises.Entities.Booking", b =>
                {
                    b.HasOne("EFCorePgExercises.Entities.Facility", "Facility")
                        .WithMany("Bookings")
                        .HasForeignKey("FacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFCorePgExercises.Entities.Member", "Member")
                        .WithMany("Bookings")
                        .HasForeignKey("MemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("EFCorePgExercises.Entities.Member", b =>
                {
                    b.HasOne("EFCorePgExercises.Entities.Member", "Recommender")
                        .WithMany("Children")
                        .HasForeignKey("RecommendedBy");

                    b.Navigation("Recommender");
                });

            modelBuilder.Entity("EFCorePgExercises.Entities.Facility", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("EFCorePgExercises.Entities.Member", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}