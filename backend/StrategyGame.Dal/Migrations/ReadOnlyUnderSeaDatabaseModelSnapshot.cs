﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StrategyGame.Dal;

namespace StrategyGame.Dal.Migrations
{
    [DbContext(typeof(ReadOnlyUnderSeaDatabase))]
    partial class ReadOnlyUnderSeaDatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
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
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

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

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.BuildingEffect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BuildingId");

                    b.Property<int?>("EffectId");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("EffectId");

                    b.ToTable("BuildingEffects");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.BuildingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuildTime");

                    b.Property<int>("CostCoral");

                    b.Property<int>("CostPearl");

                    b.Property<int>("MaxCount");

                    b.HasKey("Id");

                    b.ToTable("BuildingTypes");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Command", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ParentCountryId");

                    b.Property<int?>("TargetCountryId");

                    b.HasKey("Id");

                    b.HasIndex("ParentCountryId");

                    b.HasIndex("TargetCountryId");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Corals");

                    b.Property<int>("Pearls");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.CountryBuilding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BuildingId");

                    b.Property<int>("Count");

                    b.Property<int?>("ParentCountryId");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("ParentCountryId");

                    b.ToTable("CountryBuildings");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.CountryResearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count");

                    b.Property<int?>("ParentCountryId");

                    b.Property<int?>("ResearchId");

                    b.HasKey("Id");

                    b.HasIndex("ParentCountryId");

                    b.HasIndex("ResearchId");

                    b.ToTable("CountryResearches");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Division", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count");

                    b.Property<int?>("ParentCommandId");

                    b.Property<int?>("UnitId");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommandId");

                    b.HasIndex("UnitId");

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Effects.AbstractEffect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.ToTable("Effects");

                    b.HasDiscriminator<string>("Discriminator").HasValue("AbstractEffect");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Frontend.BuildingContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<int>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .IsUnique();

                    b.ToTable("BuildingContents");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Frontend.ResearchContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<int>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .IsUnique();

                    b.ToTable("ResearchContents");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Frontend.UnitContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<int>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .IsUnique();

                    b.ToTable("UnitContents");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.GlobalValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PealPerPopulation");

                    b.Property<long>("Round");

                    b.Property<int>("StartingCoral");

                    b.Property<int>("StartingPearls");

                    b.Property<int>("StartingPopulation");

                    b.Property<int>("StartingSoldierCapacity");

                    b.HasKey("Id");

                    b.ToTable("GlobalValues");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.InProgressBuilding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BuildingId");

                    b.Property<int?>("ParentCountryId");

                    b.Property<int>("TimeLeft");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("ParentCountryId");

                    b.ToTable("InProgressBuildings");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.InProgressResearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ParentCountryId");

                    b.Property<int?>("ResearchId");

                    b.Property<int>("TimeLeft");

                    b.HasKey("Id");

                    b.HasIndex("ParentCountryId");

                    b.HasIndex("ResearchId");

                    b.ToTable("InProgressResearches");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.ResearchEffect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EffectId");

                    b.Property<int?>("ResearchId");

                    b.HasKey("Id");

                    b.HasIndex("EffectId");

                    b.HasIndex("ResearchId");

                    b.ToTable("ResearchEffects");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.ResearchType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CostCoral");

                    b.Property<int>("CostPearl");

                    b.Property<int>("MaxCompletedAmount");

                    b.Property<int>("ResearchTime");

                    b.HasKey("Id");

                    b.ToTable("ResearchTypes");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.UnitType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttackPower");

                    b.Property<int>("CostCoral");

                    b.Property<int>("CostPearl");

                    b.Property<int>("DefensePower");

                    b.Property<int>("MaintenanceCoral");

                    b.Property<int>("MaintenancePearl");

                    b.HasKey("Id");

                    b.ToTable("UnitTypes");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

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

                    b.Property<int>("RuledCountryId");

                    b.Property<string>("SecurityStamp");

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

                    b.HasIndex("RuledCountryId")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Effects.DummyEffect", b =>
                {
                    b.HasBaseType("StrategyGame.Model.Entities.Effects.AbstractEffect");

                    b.HasDiscriminator().HasValue("DummyEffect");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StrategyGame.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.BuildingEffect", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.BuildingType", "Building")
                        .WithMany("Effects")
                        .HasForeignKey("BuildingId");

                    b.HasOne("StrategyGame.Model.Entities.Effects.AbstractEffect", "Effect")
                        .WithMany("AffectedBuildings")
                        .HasForeignKey("EffectId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Command", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.Country", "ParentCountry")
                        .WithMany("Commands")
                        .HasForeignKey("ParentCountryId");

                    b.HasOne("StrategyGame.Model.Entities.Country", "TargetCountry")
                        .WithMany("IncomingAttacks")
                        .HasForeignKey("TargetCountryId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.CountryBuilding", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.BuildingType", "Building")
                        .WithMany("CompletedBuildings")
                        .HasForeignKey("BuildingId");

                    b.HasOne("StrategyGame.Model.Entities.Country", "ParentCountry")
                        .WithMany("Buildings")
                        .HasForeignKey("ParentCountryId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.CountryResearch", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.Country", "ParentCountry")
                        .WithMany("Researches")
                        .HasForeignKey("ParentCountryId");

                    b.HasOne("StrategyGame.Model.Entities.ResearchType", "Research")
                        .WithMany("CompletedResearches")
                        .HasForeignKey("ResearchId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Division", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.Command", "ParentCommand")
                        .WithMany("Divisons")
                        .HasForeignKey("ParentCommandId");

                    b.HasOne("StrategyGame.Model.Entities.UnitType", "Unit")
                        .WithMany("ContainingDivisions")
                        .HasForeignKey("UnitId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Frontend.BuildingContent", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.BuildingType", "Parent")
                        .WithOne("Content")
                        .HasForeignKey("StrategyGame.Model.Entities.Frontend.BuildingContent", "ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Frontend.ResearchContent", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.ResearchType", "Parent")
                        .WithOne("Content")
                        .HasForeignKey("StrategyGame.Model.Entities.Frontend.ResearchContent", "ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.Frontend.UnitContent", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.UnitType", "Parent")
                        .WithOne("Content")
                        .HasForeignKey("StrategyGame.Model.Entities.Frontend.UnitContent", "ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.InProgressBuilding", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.BuildingType", "Building")
                        .WithMany("InProgressBuildings")
                        .HasForeignKey("BuildingId");

                    b.HasOne("StrategyGame.Model.Entities.Country", "ParentCountry")
                        .WithMany("InProgressBuildings")
                        .HasForeignKey("ParentCountryId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.InProgressResearch", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.Country", "ParentCountry")
                        .WithMany("InProgressResearches")
                        .HasForeignKey("ParentCountryId");

                    b.HasOne("StrategyGame.Model.Entities.ResearchType", "Research")
                        .WithMany("InProgressResearches")
                        .HasForeignKey("ResearchId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.ResearchEffect", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.Effects.AbstractEffect", "Effect")
                        .WithMany("AffectedResearches")
                        .HasForeignKey("EffectId");

                    b.HasOne("StrategyGame.Model.Entities.ResearchType", "Research")
                        .WithMany("Effects")
                        .HasForeignKey("ResearchId");
                });

            modelBuilder.Entity("StrategyGame.Model.Entities.User", b =>
                {
                    b.HasOne("StrategyGame.Model.Entities.Country", "RuledCountry")
                        .WithOne("ParentUser")
                        .HasForeignKey("StrategyGame.Model.Entities.User", "RuledCountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
