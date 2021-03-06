﻿// <auto-generated />
using System;
using CodeFirstExamFromMSSQL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeFirstExamFromMSSQL.Migrations
{
    [DbContext(typeof(BitBuckerContext))]
    [Migration("20210223141101_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Commits", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContributorId")
                        .HasColumnType("int");

                    b.Property<int>("IssueId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContributorId");

                    b.HasIndex("IssueId");

                    b.HasIndex("RepositoryId");

                    b.ToTable("Commits");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Files", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommitId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<decimal>("Size")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CommitId");

                    b.HasIndex("ParentId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Issues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssigneeId")
                        .HasColumnType("int");

                    b.Property<string>("IssueStatus")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("RepositoryId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Repositories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.RepositoriesContributors", b =>
                {
                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.Property<int>("ContributorId")
                        .HasColumnType("int");

                    b.HasKey("RepositoryId", "ContributorId");

                    b.HasIndex("ContributorId");

                    b.ToTable("RepositoriesContributors");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Username")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Commits", b =>
                {
                    b.HasOne("CodeFirstExamFromMSSQL.Models.Users", "User")
                        .WithMany("Commits")
                        .HasForeignKey("ContributorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CodeFirstExamFromMSSQL.Models.Issues", "Issue")
                        .WithMany("Commits")
                        .HasForeignKey("IssueId");

                    b.HasOne("CodeFirstExamFromMSSQL.Models.Repositories", "Repository")
                        .WithMany("Commits")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Issue");

                    b.Navigation("Repository");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Files", b =>
                {
                    b.HasOne("CodeFirstExamFromMSSQL.Models.Commits", "Commit")
                        .WithMany("Files")
                        .HasForeignKey("CommitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeFirstExamFromMSSQL.Models.Files", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("Commit");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Issues", b =>
                {
                    b.HasOne("CodeFirstExamFromMSSQL.Models.Users", "User")
                        .WithMany("Issues")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeFirstExamFromMSSQL.Models.Repositories", "Repository")
                        .WithMany("Issues")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.RepositoriesContributors", b =>
                {
                    b.HasOne("CodeFirstExamFromMSSQL.Models.Users", "User")
                        .WithMany("RepositoriesContributors")
                        .HasForeignKey("ContributorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CodeFirstExamFromMSSQL.Models.Repositories", "Repository")
                        .WithMany("RepositoriesContributors")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Commits", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Issues", b =>
                {
                    b.Navigation("Commits");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Repositories", b =>
                {
                    b.Navigation("Commits");

                    b.Navigation("Issues");

                    b.Navigation("RepositoriesContributors");
                });

            modelBuilder.Entity("CodeFirstExamFromMSSQL.Models.Users", b =>
                {
                    b.Navigation("Commits");

                    b.Navigation("Issues");

                    b.Navigation("RepositoriesContributors");
                });
#pragma warning restore 612, 618
        }
    }
}
