﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RealEstates.Datase;

namespace RealEstates.Datase.Migrations
{
    [DbContext(typeof(RealEstateDBContext))]
    partial class RealEstateDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PropertyAdPropertyTag", b =>
                {
                    b.Property<int>("PropertyAdsId")
                        .HasColumnType("int");

                    b.Property<int>("PropertyTagsId")
                        .HasColumnType("int");

                    b.HasKey("PropertyAdsId", "PropertyTagsId");

                    b.HasIndex("PropertyTagsId");

                    b.ToTable("PropertyAdPropertyTag");
                });

            modelBuilder.Entity("RealEstates.Modelse.BuildingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BuildingTypes");
                });

            modelBuilder.Entity("RealEstates.Modelse.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("RealEstates.Modelse.PropertyAd", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuildingTypeId")
                        .HasColumnType("int");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<byte?>("Floor")
                        .HasColumnType("tinyint");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<byte?>("TotalFloors")
                        .HasColumnType("tinyint");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("YardSize")
                        .HasColumnType("int");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BuildingTypeId");

                    b.HasIndex("DistrictId");

                    b.HasIndex("TypeId");

                    b.ToTable("PropertyAds");
                });

            modelBuilder.Entity("RealEstates.Modelse.PropertyTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PropertyTags");
                });

            modelBuilder.Entity("RealEstates.Modelse.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("PropertyAdPropertyTag", b =>
                {
                    b.HasOne("RealEstates.Modelse.PropertyAd", null)
                        .WithMany()
                        .HasForeignKey("PropertyAdsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstates.Modelse.PropertyTag", null)
                        .WithMany()
                        .HasForeignKey("PropertyTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RealEstates.Modelse.PropertyAd", b =>
                {
                    b.HasOne("RealEstates.Modelse.BuildingType", "BuildingType")
                        .WithMany("PropertyAds")
                        .HasForeignKey("BuildingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstates.Modelse.District", "District")
                        .WithMany("PropertyAds")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstates.Modelse.Type", "Type")
                        .WithMany("PropertyAds")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BuildingType");

                    b.Navigation("District");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("RealEstates.Modelse.BuildingType", b =>
                {
                    b.Navigation("PropertyAds");
                });

            modelBuilder.Entity("RealEstates.Modelse.District", b =>
                {
                    b.Navigation("PropertyAds");
                });

            modelBuilder.Entity("RealEstates.Modelse.Type", b =>
                {
                    b.Navigation("PropertyAds");
                });
#pragma warning restore 612, 618
        }
    }
}