﻿// <auto-generated />
namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;
    using System;

    [DbContext(typeof(StarStuffDbContext))]
    partial class StarStuffDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("PublicationId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PublicationId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Discovery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateMade")
                        .HasColumnType("Date");

                    b.Property<bool>("IsConfirmed");

                    b.Property<string>("StarSystem")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("TelescopeId");

                    b.HasKey("Id");

                    b.HasIndex("StarSystem")
                        .IsUnique();

                    b.HasIndex("TelescopeId");

                    b.ToTable("Discoveries");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Journal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Journals");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LogType");

                    b.Property<string>("TableName");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Observers", b =>
                {
                    b.Property<int>("ObserverId");

                    b.Property<int>("DiscoveryId");

                    b.HasKey("ObserverId", "DiscoveryId");

                    b.HasIndex("DiscoveryId");

                    b.ToTable("Observers");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Pioneers", b =>
                {
                    b.Property<int>("PioneerId");

                    b.Property<int>("DiscoveryId");

                    b.HasKey("PioneerId", "DiscoveryId");

                    b.HasIndex("DiscoveryId");

                    b.ToTable("Pioneers");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Planet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscoveryId");

                    b.Property<double>("Mass");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("DiscoveryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Planets");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<int>("DiscoveryId");

                    b.Property<int>("JournalId");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("Date");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("DiscoveryId");

                    b.HasIndex("JournalId");

                    b.ToTable("Publications");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Star", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscoveryId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Temperature");

                    b.HasKey("Id");

                    b.HasIndex("DiscoveryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Stars");
                });

            modelBuilder.Entity("StarStuff.Data.Models.Telescope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<double>("MirrorDiameter");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Telescopes");
                });

            modelBuilder.Entity("StarStuff.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("Date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<byte[]>("ProfileImage")
                        .HasMaxLength(1048576);

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("SendApplication");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("StarStuff.Data.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("StarStuff.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("StarStuff.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("StarStuff.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.Comment", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Publication", "Publication")
                        .WithMany("Comments")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StarStuff.Data.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.Discovery", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Telescope", "Telescope")
                        .WithMany("Discoveries")
                        .HasForeignKey("TelescopeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.Observers", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Discovery", "Discovery")
                        .WithMany("Observers")
                        .HasForeignKey("DiscoveryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StarStuff.Data.Models.User", "Observer")
                        .WithMany("Observations")
                        .HasForeignKey("ObserverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.Pioneers", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Discovery", "Discovery")
                        .WithMany("Pioneers")
                        .HasForeignKey("DiscoveryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StarStuff.Data.Models.User", "Pioneer")
                        .WithMany("Discoveries")
                        .HasForeignKey("PioneerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.Planet", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Discovery", "Discovery")
                        .WithMany("Planets")
                        .HasForeignKey("DiscoveryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.Publication", b =>
                {
                    b.HasOne("StarStuff.Data.Models.User", "Author")
                        .WithMany("Publications")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("StarStuff.Data.Models.Discovery", "Discovery")
                        .WithMany("Publications")
                        .HasForeignKey("DiscoveryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StarStuff.Data.Models.Journal", "Journal")
                        .WithMany("Publications")
                        .HasForeignKey("JournalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.Star", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Discovery", "Discovery")
                        .WithMany("Stars")
                        .HasForeignKey("DiscoveryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StarStuff.Data.Models.UserRole", b =>
                {
                    b.HasOne("StarStuff.Data.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StarStuff.Data.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
