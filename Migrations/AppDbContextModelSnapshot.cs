﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
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

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<int>("Vigor")
                        .HasColumnType("int");

                    b.HasKey("Id");

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

                    b.Property<int>("CurrentScoutEnergy")
                        .HasColumnType("int");

                    b.Property<int>("CurrentSkillpoints")
                        .HasColumnType("int");

                    b.Property<int>("ExpToNextLevel")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

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

                    b.Property<int>("CoordX")
                        .HasColumnType("int");

                    b.Property<int>("CoordY")
                        .HasColumnType("int");

                    b.Property<Guid?>("EnemyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActiveForEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActiveForMove")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActiveForScout")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEndRoom")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEnemyRoom")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSafeRoom")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStartRoom")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("EnemyId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("ShapeDungeon.Entities.Room", b =>
                {
                    b.HasOne("ShapeDungeon.Entities.Enemy", "Enemy")
                        .WithMany()
                        .HasForeignKey("EnemyId");

                    b.Navigation("Enemy");
                });
#pragma warning restore 612, 618
        }
    }
}
