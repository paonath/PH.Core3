﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PH.Core3.TestContext;

namespace PH.Core3.TestContext.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20190827084622_a_new_hope")]
    partial class a_new_hope
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PH.Core3.TestContext.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<long>("CreatedTransactionId")
                        .HasColumnName("CreatedTransactionId");

                    b.Property<bool>("Deleted")
                        .HasColumnName("Deleted");

                    b.Property<long?>("DeletedTransactionId")
                        .HasColumnName("DeletedTransactionId");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("Timestamp");

                    b.Property<long>("UpdatedTransactionId")
                        .HasColumnName("UpdatedTransactionId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedTransactionId");

                    b.HasIndex("DeletedTransactionId");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.HasIndex("UpdatedTransactionId");

                    b.HasIndex("Id", "Deleted", "CreatedTransactionId", "UpdatedTransactionId", "DeletedTransactionId");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("PH.Core3.TestContext.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<long>("CreatedTransactionId")
                        .HasColumnName("CreatedTransactionId");

                    b.Property<bool>("Deleted")
                        .HasColumnName("Deleted");

                    b.Property<long?>("DeletedTransactionId")
                        .HasColumnName("DeletedTransactionId");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnName("Timestamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<long>("UpdatedTransactionId")
                        .HasColumnName("UpdatedTransactionId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CreatedTransactionId");

                    b.HasIndex("DeletedTransactionId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UpdatedTransactionId");

                    b.HasIndex("Id", "Deleted", "CreatedTransactionId", "UpdatedTransactionId", "DeletedTransactionId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PH.UowEntityFramework.EntityFramework.Abstractions.Models.TransactionAudit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("CommitMessage")
                        .HasMaxLength(500);

                    b.Property<double>("MillisecDuration")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0.0);

                    b.Property<string>("Scopes")
                        .HasMaxLength(500);

                    b.Property<string>("StrIdentifier")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<DateTime>("UtcDateTime");

                    b.HasKey("Id");

                    b.HasIndex("Id", "StrIdentifier", "Author", "UtcDateTime", "Timestamp");

                    b.ToTable("transaction_audit");
                });

            modelBuilder.Entity("PH.UowEntityFramework.EntityFramework.Audit.Audit", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("Author")
                        .HasMaxLength(255);

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("KeyValues");

                    b.Property<byte[]>("NewValues");

                    b.Property<byte[]>("OldValues");

                    b.Property<string>("TableName");

                    b.Property<long>("TransactionId");

                    b.HasKey("Id");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("PH.Core3.TestContext.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PH.Core3.TestContext.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PH.Core3.TestContext.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("PH.Core3.TestContext.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PH.Core3.TestContext.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PH.Core3.TestContext.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PH.Core3.TestContext.Role", b =>
                {
                    b.HasOne("PH.UowEntityFramework.EntityFramework.Abstractions.Models.TransactionAudit", "CreatedTransaction")
                        .WithMany()
                        .HasForeignKey("CreatedTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PH.UowEntityFramework.EntityFramework.Abstractions.Models.TransactionAudit", "DeletedTransaction")
                        .WithMany()
                        .HasForeignKey("DeletedTransactionId");

                    b.HasOne("PH.UowEntityFramework.EntityFramework.Abstractions.Models.TransactionAudit", "UpdatedTransaction")
                        .WithMany()
                        .HasForeignKey("UpdatedTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PH.Core3.TestContext.User", b =>
                {
                    b.HasOne("PH.UowEntityFramework.EntityFramework.Abstractions.Models.TransactionAudit", "CreatedTransaction")
                        .WithMany()
                        .HasForeignKey("CreatedTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PH.UowEntityFramework.EntityFramework.Abstractions.Models.TransactionAudit", "DeletedTransaction")
                        .WithMany()
                        .HasForeignKey("DeletedTransactionId");

                    b.HasOne("PH.UowEntityFramework.EntityFramework.Abstractions.Models.TransactionAudit", "UpdatedTransaction")
                        .WithMany()
                        .HasForeignKey("UpdatedTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}