﻿// <auto-generated />
using System;
using EmailWebApi.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmailWebApi.Migrations
{
    [DbContext(typeof(EmailContext))]
    [Migration("20201229065209_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("EmailWebApi.Objects.Email", b =>
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

            modelBuilder.Entity("EmailWebApi.Objects.EmailBody", b =>
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

            modelBuilder.Entity("EmailWebApi.Objects.EmailContent", b =>
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

            modelBuilder.Entity("EmailWebApi.Objects.EmailInfo", b =>
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

            modelBuilder.Entity("EmailWebApi.Objects.EmailState", b =>
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

            modelBuilder.Entity("EmailWebApi.Objects.ThrottlingState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndPoint")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LastAddressCounter")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ThrottlingStates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Counter = 0,
                            EndPoint = new DateTime(2020, 12, 29, 9, 53, 7, 628, DateTimeKind.Local).AddTicks(852),
                            LastAddress = "",
                            LastAddressCounter = 0
                        });
                });

            modelBuilder.Entity("EmailWebApi.Objects.Email", b =>
                {
                    b.HasOne("EmailWebApi.Objects.EmailContent", "Content")
                        .WithMany()
                        .HasForeignKey("ContentId");

                    b.HasOne("EmailWebApi.Objects.EmailInfo", "Info")
                        .WithMany()
                        .HasForeignKey("InfoId");

                    b.HasOne("EmailWebApi.Objects.EmailState", "State")
                        .WithMany()
                        .HasForeignKey("StateId");

                    b.Navigation("Content");

                    b.Navigation("Info");

                    b.Navigation("State");
                });

            modelBuilder.Entity("EmailWebApi.Objects.EmailContent", b =>
                {
                    b.HasOne("EmailWebApi.Objects.EmailBody", "Body")
                        .WithMany()
                        .HasForeignKey("BodyId");

                    b.Navigation("Body");
                });
#pragma warning restore 612, 618
        }
    }
}
