// <auto-generated />
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
    [Migration("20220525184603_update")]
    partial class update
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("TFS_Parser.Entities.ROOTALTERNATELISTALT", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
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

            modelBuilder.Entity("TFS_Parser.Entities.ROOTALTERNATELISTALTITEM", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("ROOTALTERNATELISTALTID")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ROOTALTERNATELISTALTID");

                    b.ToTable("ROOTALTERNATELISTALTITEM");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFS", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
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

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFSTFE", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("ROOTMAINLISTTFSID")
                        .HasColumnType("text");

                    b.Property<string>("TypeID")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ROOTMAINLISTTFSID");

                    b.ToTable("ROOTMAINLISTTFSTFE");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFSTFEPARAMSParamAlt", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("A1_B_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_K00_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_K11_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_P00_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_P11_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_P_EL_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_TD_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_TF_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_T_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_VD_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_VF_F")
                        .HasColumnType("text");

                    b.Property<string>("A1_V_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_B_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_K00_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_K11_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_P00_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_P11_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_P_EL_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_TD_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_TF_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_T_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_VD_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_VF_F")
                        .HasColumnType("text");

                    b.Property<string>("A2_V_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_B_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_K00_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_K11_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_P00_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_P11_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_P_EL_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_TD_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_TF_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_T_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_VD_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_VF_F")
                        .HasColumnType("text");

                    b.Property<string>("A3_V_F")
                        .HasColumnType("text");

                    b.Property<string>("B")
                        .HasColumnType("text");

                    b.Property<string>("B_F1B")
                        .HasColumnType("text");

                    b.Property<string>("B_F1N")
                        .HasColumnType("text");

                    b.Property<string>("B_F2B")
                        .HasColumnType("text");

                    b.Property<string>("B_F2N")
                        .HasColumnType("text");

                    b.Property<string>("B_F3B")
                        .HasColumnType("text");

                    b.Property<string>("B_F3N")
                        .HasColumnType("text");

                    b.Property<string>("ELEMENT")
                        .HasColumnType("text");

                    b.Property<string>("ELEM_DIAGN")
                        .HasColumnType("text");

                    b.Property<string>("FUNCTION")
                        .HasColumnType("text");

                    b.Property<string>("K00_F1B")
                        .HasColumnType("text");

                    b.Property<string>("K00_F1N")
                        .HasColumnType("text");

                    b.Property<string>("K00_F2B")
                        .HasColumnType("text");

                    b.Property<string>("K00_F2N")
                        .HasColumnType("text");

                    b.Property<string>("K00_F3B")
                        .HasColumnType("text");

                    b.Property<string>("K00_F3N")
                        .HasColumnType("text");

                    b.Property<string>("K11_F1B")
                        .HasColumnType("text");

                    b.Property<string>("K11_F1N")
                        .HasColumnType("text");

                    b.Property<string>("K11_F2B")
                        .HasColumnType("text");

                    b.Property<string>("K11_F2N")
                        .HasColumnType("text");

                    b.Property<string>("K11_F3B")
                        .HasColumnType("text");

                    b.Property<string>("K11_F3N")
                        .HasColumnType("text");

                    b.Property<string>("K_00")
                        .HasColumnType("text");

                    b.Property<string>("K_11")
                        .HasColumnType("text");

                    b.Property<string>("NAME")
                        .HasColumnType("text");

                    b.Property<string>("NUMBER")
                        .HasColumnType("text");

                    b.Property<string>("P00_F1B")
                        .HasColumnType("text");

                    b.Property<string>("P00_F1N")
                        .HasColumnType("text");

                    b.Property<string>("P00_F2B")
                        .HasColumnType("text");

                    b.Property<string>("P00_F2N")
                        .HasColumnType("text");

                    b.Property<string>("P00_F3B")
                        .HasColumnType("text");

                    b.Property<string>("P00_F3N")
                        .HasColumnType("text");

                    b.Property<string>("P11_F1B")
                        .HasColumnType("text");

                    b.Property<string>("P11_F1N")
                        .HasColumnType("text");

                    b.Property<string>("P11_F2B")
                        .HasColumnType("text");

                    b.Property<string>("P11_F2N")
                        .HasColumnType("text");

                    b.Property<string>("P11_F3B")
                        .HasColumnType("text");

                    b.Property<string>("P11_F3N")
                        .HasColumnType("text");

                    b.Property<string>("PREDICAT")
                        .HasColumnType("text");

                    b.Property<string>("PRED_ISTOR")
                        .HasColumnType("text");

                    b.Property<string>("P_00")
                        .HasColumnType("text");

                    b.Property<string>("P_11")
                        .HasColumnType("text");

                    b.Property<string>("P_DIAGN_EL")
                        .HasColumnType("text");

                    b.Property<string>("P_EL_F1B")
                        .HasColumnType("text");

                    b.Property<string>("P_EL_F1N")
                        .HasColumnType("text");

                    b.Property<string>("P_EL_F2B")
                        .HasColumnType("text");

                    b.Property<string>("P_EL_F2N")
                        .HasColumnType("text");

                    b.Property<string>("P_EL_F3B")
                        .HasColumnType("text");

                    b.Property<string>("P_EL_F3N")
                        .HasColumnType("text");

                    b.Property<string>("ROOTMAINLISTTFSTFEID")
                        .HasColumnType("text");

                    b.Property<string>("SOSTAV")
                        .HasColumnType("text");

                    b.Property<string>("SOVM")
                        .HasColumnType("text");

                    b.Property<string>("SOVM0")
                        .HasColumnType("text");

                    b.Property<string>("SOVM1")
                        .HasColumnType("text");

                    b.Property<string>("T")
                        .HasColumnType("text");

                    b.Property<string>("TD_F1B")
                        .HasColumnType("text");

                    b.Property<string>("TD_F1N")
                        .HasColumnType("text");

                    b.Property<string>("TD_F2B")
                        .HasColumnType("text");

                    b.Property<string>("TD_F2N")
                        .HasColumnType("text");

                    b.Property<string>("TD_F3B")
                        .HasColumnType("text");

                    b.Property<string>("TD_F3N")
                        .HasColumnType("text");

                    b.Property<string>("TF_F1B")
                        .HasColumnType("text");

                    b.Property<string>("TF_F1N")
                        .HasColumnType("text");

                    b.Property<string>("TF_F2B")
                        .HasColumnType("text");

                    b.Property<string>("TF_F2N")
                        .HasColumnType("text");

                    b.Property<string>("TF_F3B")
                        .HasColumnType("text");

                    b.Property<string>("TF_F3N")
                        .HasColumnType("text");

                    b.Property<string>("TYPE")
                        .HasColumnType("text");

                    b.Property<string>("T_D")
                        .HasColumnType("text");

                    b.Property<string>("T_F")
                        .HasColumnType("text");

                    b.Property<string>("T_F1B")
                        .HasColumnType("text");

                    b.Property<string>("T_F1N")
                        .HasColumnType("text");

                    b.Property<string>("T_F2B")
                        .HasColumnType("text");

                    b.Property<string>("T_F2N")
                        .HasColumnType("text");

                    b.Property<string>("T_F3B")
                        .HasColumnType("text");

                    b.Property<string>("T_F3N")
                        .HasColumnType("text");

                    b.Property<string>("V")
                        .HasColumnType("text");

                    b.Property<string>("VD_F1B")
                        .HasColumnType("text");

                    b.Property<string>("VD_F1N")
                        .HasColumnType("text");

                    b.Property<string>("VD_F2B")
                        .HasColumnType("text");

                    b.Property<string>("VD_F2N")
                        .HasColumnType("text");

                    b.Property<string>("VD_F3B")
                        .HasColumnType("text");

                    b.Property<string>("VD_F3N")
                        .HasColumnType("text");

                    b.Property<string>("VF_F1B")
                        .HasColumnType("text");

                    b.Property<string>("VF_F1N")
                        .HasColumnType("text");

                    b.Property<string>("VF_F2B")
                        .HasColumnType("text");

                    b.Property<string>("VF_F2N")
                        .HasColumnType("text");

                    b.Property<string>("VF_F3B")
                        .HasColumnType("text");

                    b.Property<string>("VF_F3N")
                        .HasColumnType("text");

                    b.Property<string>("V_D")
                        .HasColumnType("text");

                    b.Property<string>("V_F")
                        .HasColumnType("text");

                    b.Property<string>("V_F1B")
                        .HasColumnType("text");

                    b.Property<string>("V_F1N")
                        .HasColumnType("text");

                    b.Property<string>("V_F2B")
                        .HasColumnType("text");

                    b.Property<string>("V_F2N")
                        .HasColumnType("text");

                    b.Property<string>("V_F3B")
                        .HasColumnType("text");

                    b.Property<string>("V_F3N")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("ROOTMAINLISTTFSTFEID");

                    b.ToTable("ROOTMAINLISTTFSTFEPARAMSParamAlt");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTTYPEDECISION", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
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

            modelBuilder.Entity("TFS_Parser.Entities.ROOTTYPEDECISIONParams", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
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

            modelBuilder.Entity("TFS_Parser.Entities.ROOTTYPEPARAM", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd()
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

            modelBuilder.Entity("TFS_Parser.Entities.ROOTALTERNATELISTALT", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("ALTERNATELIST")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTALTERNATELISTALTITEM", b =>
                {
                    b.HasOne("TFS_Parser.Entities.ROOTALTERNATELISTALT", null)
                        .WithMany("ITEM")
                        .HasForeignKey("ROOTALTERNATELISTALTID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFS", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("MAINLIST")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFSTFE", b =>
                {
                    b.HasOne("TFS_Parser.Entities.ROOTMAINLISTTFS", null)
                        .WithMany("TFE")
                        .HasForeignKey("ROOTMAINLISTTFSID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFSTFEPARAMSParamAlt", b =>
                {
                    b.HasOne("TFS_Parser.Entities.ROOTMAINLISTTFSTFE", null)
                        .WithMany("PARAMS")
                        .HasForeignKey("ROOTMAINLISTTFSTFEID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTTYPEDECISION", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("TYPEDECISION")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTTYPEDECISIONParams", b =>
                {
                    b.HasOne("TFS_Parser.Entities.ROOTTYPEDECISION", null)
                        .WithMany("Params")
                        .HasForeignKey("ROOTTYPEDECISIONID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTTYPEPARAM", b =>
                {
                    b.HasOne("TFS_Parser.Entities.TFS", null)
                        .WithMany("TYPEPARAM")
                        .HasForeignKey("TFSID");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTALTERNATELISTALT", b =>
                {
                    b.Navigation("ITEM");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFS", b =>
                {
                    b.Navigation("TFE");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTMAINLISTTFSTFE", b =>
                {
                    b.Navigation("PARAMS");
                });

            modelBuilder.Entity("TFS_Parser.Entities.ROOTTYPEDECISION", b =>
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
