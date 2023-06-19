﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShapeDungeon.Data;

#nullable disable

namespace ShapeDungeon.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.16");

            modelBuilder.Entity("ShapeDungeon.Entities.Enemy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Agility")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentHp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DroppedExp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Shape")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Vigor")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Enemies");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.EnemyRoom", b =>
                {
                    b.Property<Guid>("EnemyId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnemyDefeated")
                        .HasColumnType("INTEGER");

                    b.HasKey("EnemyId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("EnemiesRooms");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("BonusAgility")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BonusStrength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BonusVigor")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RequiredLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Agility")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentExp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentScoutEnergy")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentSkillpoints")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExpToNextLevel")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Shape")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Vigor")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3de35703-1fef-4070-d75d-08db4beac0a7"),
                            Agility = 1,
                            CurrentExp = 0,
                            CurrentScoutEnergy = 1,
                            CurrentSkillpoints = 0,
                            ExpToNextLevel = 100,
                            IsActive = true,
                            Level = 0,
                            Name = "Squary Lvl.8",
                            Shape = 0,
                            Strength = 2,
                            Vigor = 5
                        });
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("CanGoDown")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanGoLeft")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanGoRight")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanGoUp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CoordX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CoordY")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActiveForEdit")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActiveForMove")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActiveForScout")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEndRoom")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnemyRoom")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSafeRoom")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsStartRoom")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("dd54f8ee-349f-4dfd-1a70-08db56fb8a4b"),
                            CanGoDown = true,
                            CanGoLeft = true,
                            CanGoRight = true,
                            CanGoUp = true,
                            CoordX = 0,
                            CoordY = 0,
                            IsActiveForEdit = true,
                            IsActiveForMove = true,
                            IsActiveForScout = true,
                            IsEndRoom = false,
                            IsEnemyRoom = false,
                            IsSafeRoom = false,
                            IsStartRoom = true
                        });
                });

            modelBuilder.Entity("ShapeDungeon.Entities.EnemyRoom", b =>
                {
                    b.HasOne("ShapeDungeon.Entities.Enemy", "Enemy")
                        .WithMany()
                        .HasForeignKey("EnemyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShapeDungeon.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enemy");

                    b.Navigation("Room");
                });
#pragma warning restore 612, 618
        }
    }
}
