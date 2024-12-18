﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TicTacAPI.Data;

#nullable disable

namespace TicTacAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241217210133_Game Variable name update")]
    partial class GameVariablenameupdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TicTacAPI.Data.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.PrimitiveCollection<string[]>("GameState")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Player1")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Player2")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Turn")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
