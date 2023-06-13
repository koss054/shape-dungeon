﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShapeDungeon.Data;

#nullable disable

namespace ShapeDungeon.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230503145719_UpdatedPlayerAndRoom")]
    partial class UpdatedPlayerAndRoom
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ShapeDungeon.Entities.Enemy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Agility")
                        .HasColumnType("int");

                    b.Property<int>("DroppedExp")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<int>("Vigor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Enemies");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BonusAgility")
                        .HasColumnType("int");

                    b.Property<int>("BonusStrength")
                        .HasColumnType("int");

                    b.Property<int>("BonusVigor")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequiredLevel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Agility")
                        .HasColumnType("int");

                    b.Property<int>("CurrentExp")
                        .HasColumnType("int");

                    b.Property<int>("CurrentSkillpoints")
                        .HasColumnType("int");

                    b.Property<int>("ExpToNextLevel")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Shape")
                        .HasColumnType("int");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<int>("Vigor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanGoDown")
                        .HasColumnType("bit");

                    b.Property<bool>("CanGoLeft")
                        .HasColumnType("bit");

                    b.Property<bool>("CanGoRight")
                        .HasColumnType("bit");

                    b.Property<bool>("CanGoUp")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEndRoom")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEnemyRoom")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSafeRoom")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStartRoom")
                        .HasColumnType("bit");

                    b.Property<Guid>("NextRoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PreviousRoomId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Enemy", b =>
                {
                    b.HasOne("ShapeDungeon.Entities.Room", "Room")
                        .WithMany("Enemies")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Room", b =>
                {
                    b.Navigation("Enemies");
                });
#pragma warning restore 612, 618
        }
    }
}
