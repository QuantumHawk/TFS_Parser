﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TFS_Parser.Models;

namespace TFS_Parser.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20220203121357_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ROOTALTERNATELISTALT", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<string>("NodeEndID")
                        .HasColumnType("text");

                    b.Property<string>("NodeStartID")
                        .HasColumnType("text");

                    b.Property<string>("Num")
                        .HasColumnType("text");

                    b.Property<int?>("TFSID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("TFSID");

                    b.ToTable("ROOTALTERNATELISTALT");
                });

            modelBuilder.Entity("ROOTALTERNATELISTALTITEM", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<string>("ROOTALTERNATELISTALTID")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ROOTALTERNATELISTALTID");

                    b.ToTable("ROOTALTERNATELISTALTITEM");
                });

            modelBuilder.Entity("ROOTMAINLISTTFS", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<string>("NextID")
                        .HasColumnType("text");

                    b.Property<string>("OffsetX")
                        .HasColumnType("text");

                    b.Property<string>("OffsetY")
                        .HasColumnType("text");

                    b.Property<string>("PriorID")
                        .HasColumnType("text");

                    b.Property<string>("StartPointX")
                        .HasColumnType("text");

                    b.Property<string>("StartPointY")
                        .HasColumnType("text");

                    b.Property<int?>("TFSID")
                        .HasColumnType("integer");

                    b.Property<string>("TypeID")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("TFSID");

                    b.ToTable("ROOTMAINLISTTFS");
                });

            modelBuilder.Entity("ROOTMAINLISTTFSTFE", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<string>("ROOTMAINLISTTFSID")
                        .HasColumnType("text");

                    b.Property<string>("TypeID")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ROOTMAINLISTTFSID");

                    b.ToTable("ROOTMAINLISTTFSTFE");
                });

            modelBuilder.Entity("ROOTTYPEDECISION", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<string>("Sovm")
                        .HasColumnType("text");

                    b.Property<int?>("TFSID")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("TFSID");

                    b.ToTable("ROOTTYPEDECISION");
                });

            modelBuilder.Entity("ROOTTYPEDECISIONParams", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<string>("ROOTTYPEDECISIONID")
                        .HasColumnType("text");

                    b.Property<string>("T")
                        .HasColumnType("text");

                    b.Property<string>("TD")
                        .HasColumnType("text");

                    b.Property<string>("V")
                        .HasColumnType("text");

                    b.Property<string>("VD")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ROOTTYPEDECISIONID");

                    b.ToTable("ROOTTYPEDECISIONParams");
                });

            modelBuilder.Entity("ROOTTYPEPARAM", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("text");

                    b.Property<int?>("TFSID")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("TFSID");

                    b.ToTable("ROOTTYPEPARAM");
                });

            modelBuilder.Entity("TFS_Parser.Entities.TFS", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ANCESTORLIST")
                        .HasColumnType("text");

                    b.Property<string>("OGRSOVMLIST")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("TFSes");
                });

            modelBuilder.Entity("ROOTALTERNATELISTALT", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("ALTERNATELIST")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("ROOTALTERNATELISTALTITEM", b =>
                {
                    b.HasOne("ROOTALTERNATELISTALT", null)
                        .WithMany("ITEM")
                        .HasForeignKey("ROOTALTERNATELISTALTID");
                });

            modelBuilder.Entity("ROOTMAINLISTTFS", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("MAINLIST")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("ROOTMAINLISTTFSTFE", b =>
                {
                    b.HasOne("ROOTMAINLISTTFS", null)
                        .WithMany("TFE")
                        .HasForeignKey("ROOTMAINLISTTFSID");
                });

            modelBuilder.Entity("ROOTTYPEDECISION", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("TYPEDECISION")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("ROOTTYPEDECISIONParams", b =>
                {
                    b.HasOne("ROOTTYPEDECISION", null)
                        .WithMany("Params")
                        .HasForeignKey("ROOTTYPEDECISIONID");
                });

            modelBuilder.Entity("ROOTTYPEPARAM", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("TYPEPARAM")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("ROOTALTERNATELISTALT", b =>
                {
                    b.Navigation("ITEM");
                });

            modelBuilder.Entity("ROOTMAINLISTTFS", b =>
                {
                    b.Navigation("TFE");
                });

            modelBuilder.Entity("ROOTTYPEDECISION", b =>
                {
                    b.Navigation("Params");
                });

            modelBuilder.Entity("TFS_Parser.Entities.TFS", b =>
                {
                    b.Navigation("ALTERNATELIST");

                    b.Navigation("MAINLIST");

                    b.Navigation("TYPEDECISION");

                    b.Navigation("TYPEPARAM");
                });
#pragma warning restore 612, 618
        }
    }
}
