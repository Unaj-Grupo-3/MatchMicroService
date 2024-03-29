﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230629165742_Add_Column_ProposedUserId_Dates")]
    partial class Add_Column_ProposedUserId_Dates
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Date", b =>
                {
                    b.Property<int>("DateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DateId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<int>("ProposedUserId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("DateId");

                    b.HasIndex("MatchId");

                    b.ToTable("Dates");
                });

            modelBuilder.Entity("Domain.Entities.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MatchId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<int>("User1Id")
                        .HasColumnType("int")
                        .HasColumnName("User1");

                    b.Property<int>("User2Id")
                        .HasColumnType("int")
                        .HasColumnName("User2");

                    b.HasKey("MatchId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Domain.Entities.UserMatch", b =>
                {
                    b.Property<int>("UserMatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserMatchId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("LikeUser1")
                        .HasColumnType("int");

                    b.Property<int>("LikeUser2")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("User1")
                        .HasColumnType("int");

                    b.Property<int>("User2")
                        .HasColumnType("int");

                    b.HasKey("UserMatchId");

                    b.ToTable("UserMatches");
                });

            modelBuilder.Entity("Domain.Entities.Date", b =>
                {
                    b.HasOne("Domain.Entities.Match", "Match")
                        .WithMany("Date")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("Domain.Entities.Match", b =>
                {
                    b.Navigation("Date");
                });
#pragma warning restore 612, 618
        }
    }
}
