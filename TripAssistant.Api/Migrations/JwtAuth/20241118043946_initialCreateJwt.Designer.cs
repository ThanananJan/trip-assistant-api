﻿// <auto-generated />
using System;
using JWTAuthentication.Library.Model.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace TripAssistant.Api.Migrations.JwtAuth
{
    [DbContext(typeof(JwtAuthDbContext))]
    [Migration("20241118043946_initialCreateJwt")]
    partial class initialCreateJwt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("JWTAuthentication.Library.Model.DB.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<DateTime>("DtmCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DtmUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("IdSub")
                        .HasColumnType("char(36)");

                    b.Property<string>("IdentityProviderName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("NamUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("IdUser");

                    b.ToTable("User");
                });

            modelBuilder.Entity("JWTAuthentication.Library.Model.DB.UserToken", b =>
                {
                    b.Property<int>("IdToken")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdToken"));

                    b.Property<DateTime>("DtmRefreshTokenExpired")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdToken");

                    b.HasIndex("IdUser");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("JWTAuthentication.Library.Model.DB.UserToken", b =>
                {
                    b.HasOne("JWTAuthentication.Library.Model.DB.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("JWTAuthentication.Library.Model.DB.User", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
