﻿// <auto-generated />

using System;
using EmailWebApi.Db.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmailWebApi.Db.Migrations
{
    [DbContext(typeof(EmailContext))]
    internal class EmailContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("EmailWebApi.Db.Entities.Email", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<int?>("ContentId")
                    .HasColumnType("int");

                b.Property<int?>("InfoId")
                    .HasColumnType("int");

                b.Property<int?>("StateId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("ContentId");

                b.HasIndex("InfoId");

                b.HasIndex("StateId");

                b.ToTable("Emails");
            });

            modelBuilder.Entity("EmailWebApi.Db.Entities.EmailBody", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Body")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("Save")
                    .HasColumnType("bit");

                b.HasKey("Id");

                b.ToTable("Bodies");
            });

            modelBuilder.Entity("EmailWebApi.Db.Entities.EmailContent", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Address")
                    .HasColumnType("nvarchar(max)");

                b.Property<int?>("BodyId")
                    .HasColumnType("int");

                b.Property<string>("Title")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("BodyId");

                b.ToTable("Contents");
            });

            modelBuilder.Entity("EmailWebApi.Db.Entities.EmailInfo", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<DateTime>("Date")
                    .HasColumnType("datetime2");

                b.Property<Guid>("UniversalId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.ToTable("Infos");
            });

            modelBuilder.Entity("EmailWebApi.Db.Entities.EmailState", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<int>("Status")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("States");
            });

            modelBuilder.Entity("EmailWebApi.Db.Entities.Email", b =>
            {
                b.HasOne("EmailWebApi.Db.Entities.EmailContent", "Content")
                    .WithMany()
                    .HasForeignKey("ContentId");

                b.HasOne("EmailWebApi.Db.Entities.EmailInfo", "Info")
                    .WithMany()
                    .HasForeignKey("InfoId");

                b.HasOne("EmailWebApi.Db.Entities.EmailState", "State")
                    .WithMany()
                    .HasForeignKey("StateId");

                b.Navigation("Content");

                b.Navigation("Info");

                b.Navigation("State");
            });

            modelBuilder.Entity("EmailWebApi.Db.Entities.EmailContent", b =>
            {
                b.HasOne("EmailWebApi.Db.Entities.EmailBody", "Body")
                    .WithMany()
                    .HasForeignKey("BodyId");

                b.Navigation("Body");
            });
#pragma warning restore 612, 618
        }
    }
}