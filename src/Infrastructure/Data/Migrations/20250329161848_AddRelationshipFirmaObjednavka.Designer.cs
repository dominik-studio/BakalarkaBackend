﻿// <auto-generated />
using System;
using CRMBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CRMBackend.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250329161848_AddRelationshipFirmaObjednavka")]
    partial class AddRelationshipFirmaObjednavka
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.CenovaPonuka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("FinalnaCena")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ObjednavkaId")
                        .HasColumnType("integer");

                    b.Property<string>("Stav")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ObjednavkaId");

                    b.HasIndex("Stav");

                    b.ToTable("CenovePonuky");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.CenovaPonukaTovar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CenovaPonukaId")
                        .HasColumnType("integer");

                    b.Property<int>("Mnozstvo")
                        .HasColumnType("integer");

                    b.Property<decimal>("PovodnaCena")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("TovarId")
                        .HasColumnType("integer");

                    b.Property<int?>("VariantTovarId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CenovaPonukaId");

                    b.HasIndex("TovarId");

                    b.HasIndex("VariantTovarId");

                    b.ToTable("CenovaPonukaTovary");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Dodavatel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Aktivny")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("NazovFirmy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Poznamka")
                        .HasColumnType("text");

                    b.Property<string>("Telefon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Aktivny");

                    b.ToTable("Dodavatelia");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.KategorieProduktov", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Nazov")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("KategorieProduktov");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.KontaktnaOsoba", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FirmaId")
                        .HasColumnType("integer");

                    b.Property<string>("Meno")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Priezvisko")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Telefon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.HasIndex("FirmaId");

                    b.HasIndex("FirmaId", "Email")
                        .IsUnique();

                    b.ToTable("KontaktneOsoby");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Objednavka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Faza")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FirmaId")
                        .HasColumnType("integer");

                    b.Property<int>("KontaktnaOsobaId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("NaplanovanyDatumVyroby")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("OcakavanyDatumDorucenia")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("PoslednaCenovaPonukaId")
                        .HasColumnType("integer");

                    b.Property<string>("Poznamka")
                        .HasColumnType("text");

                    b.Property<bool>("Zaplatene")
                        .HasColumnType("boolean");

                    b.Property<bool>("Zrusene")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("Faza");

                    b.HasIndex("FirmaId");

                    b.HasIndex("KontaktnaOsobaId");

                    b.HasIndex("NaplanovanyDatumVyroby");

                    b.HasIndex("PoslednaCenovaPonukaId")
                        .IsUnique();

                    b.HasIndex("Zaplatene");

                    b.HasIndex("Zrusene");

                    b.ToTable("Objednavky");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Tovar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Aktivny")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<decimal>("Cena")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<int>("DodavatelId")
                        .HasColumnType("integer");

                    b.Property<string>("Ean")
                        .HasColumnType("text");

                    b.Property<string>("InterneId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("KategoriaId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Nazov")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ObrazokURL")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Aktivny");

                    b.HasIndex("DodavatelId");

                    b.HasIndex("InterneId")
                        .IsUnique();

                    b.HasIndex("KategoriaId");

                    b.HasIndex("Nazov");

                    b.ToTable("Tovary");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.VariantTovar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Aktivny")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<decimal>("Cena")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("FarbaHex")
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)");

                    b.Property<string>("ObrazokURL")
                        .HasColumnType("text");

                    b.Property<int>("TovarId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Aktivny");

                    b.HasIndex("TovarId");

                    b.ToTable("VariantyTovarov");
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.Firma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<decimal>("HodnotaObjednavok")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ICO")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IcDph")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Nazov")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("SkoreSpolahlivosti")
                        .HasColumnType("decimal(3,2)");

                    b.HasKey("Id");

                    b.HasIndex("ICO")
                        .IsUnique();

                    b.HasIndex("Nazov");

                    b.HasIndex("SkoreSpolahlivosti");

                    b.ToTable("Firmy");
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<bool>("Done")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<int>("ListId")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<int?>("Priority")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Reminder")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.TodoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<int>("MaxItems")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.CenovaPonuka", b =>
                {
                    b.HasOne("CRMBackend.Domain.AggregateRoots.Objednavka", "Objednavka")
                        .WithMany("CenovePonuky")
                        .HasForeignKey("ObjednavkaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Objednavka");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.CenovaPonukaTovar", b =>
                {
                    b.HasOne("CRMBackend.Domain.AggregateRoots.CenovaPonuka", "CenovaPonuka")
                        .WithMany("Polozky")
                        .HasForeignKey("CenovaPonukaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRMBackend.Domain.AggregateRoots.Tovar", "Tovar")
                        .WithMany()
                        .HasForeignKey("TovarId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CRMBackend.Domain.AggregateRoots.VariantTovar", "VariantTovar")
                        .WithMany()
                        .HasForeignKey("VariantTovarId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CenovaPonuka");

                    b.Navigation("Tovar");

                    b.Navigation("VariantTovar");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Dodavatel", b =>
                {
                    b.OwnsOne("CRMBackend.Domain.ValueObjects.Adresa", "Adresa", b1 =>
                        {
                            b1.Property<int>("DodavatelId")
                                .HasColumnType("integer");

                            b1.Property<string>("Krajina")
                                .HasColumnType("text");

                            b1.Property<string>("Mesto")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PSC")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Ulica")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("DodavatelId");

                            b1.ToTable("Dodavatelia");

                            b1.WithOwner()
                                .HasForeignKey("DodavatelId");
                        });

                    b.Navigation("Adresa");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.KontaktnaOsoba", b =>
                {
                    b.HasOne("CRMBackend.Domain.Entities.Firma", "Firma")
                        .WithMany("KontaktneOsoby")
                        .HasForeignKey("FirmaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Firma");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Objednavka", b =>
                {
                    b.HasOne("CRMBackend.Domain.Entities.Firma", "Firma")
                        .WithMany("Objednavky")
                        .HasForeignKey("FirmaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRMBackend.Domain.AggregateRoots.KontaktnaOsoba", "KontaktnaOsoba")
                        .WithMany()
                        .HasForeignKey("KontaktnaOsobaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CRMBackend.Domain.AggregateRoots.CenovaPonuka", "PoslednaCenovaPonuka")
                        .WithOne()
                        .HasForeignKey("CRMBackend.Domain.AggregateRoots.Objednavka", "PoslednaCenovaPonukaId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Firma");

                    b.Navigation("KontaktnaOsoba");

                    b.Navigation("PoslednaCenovaPonuka");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Tovar", b =>
                {
                    b.HasOne("CRMBackend.Domain.AggregateRoots.Dodavatel", "Dodavatel")
                        .WithMany()
                        .HasForeignKey("DodavatelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CRMBackend.Domain.AggregateRoots.KategorieProduktov", "Kategoria")
                        .WithMany()
                        .HasForeignKey("KategoriaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Dodavatel");

                    b.Navigation("Kategoria");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.VariantTovar", b =>
                {
                    b.HasOne("CRMBackend.Domain.AggregateRoots.Tovar", "Tovar")
                        .WithMany("Varianty")
                        .HasForeignKey("TovarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("CRMBackend.Domain.ValueObjects.Velkost", "Velkost", b1 =>
                        {
                            b1.Property<int>("VariantTovarId")
                                .HasColumnType("integer");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("VariantTovarId");

                            b1.ToTable("VariantyTovarov");

                            b1.WithOwner()
                                .HasForeignKey("VariantTovarId");
                        });

                    b.Navigation("Tovar");

                    b.Navigation("Velkost");
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.Firma", b =>
                {
                    b.OwnsOne("CRMBackend.Domain.ValueObjects.Adresa", "Adresa", b1 =>
                        {
                            b1.Property<int>("FirmaId")
                                .HasColumnType("integer");

                            b1.Property<string>("Krajina")
                                .HasColumnType("text");

                            b1.Property<string>("Mesto")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PSC")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Ulica")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("FirmaId");

                            b1.ToTable("Firmy");

                            b1.WithOwner()
                                .HasForeignKey("FirmaId");
                        });

                    b.Navigation("Adresa")
                        .IsRequired();
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.TodoItem", b =>
                {
                    b.HasOne("CRMBackend.Domain.Entities.TodoList", "List")
                        .WithMany("Items")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.TodoList", b =>
                {
                    b.OwnsOne("CRMBackend.Domain.ValueObjects.Colour", "Colour", b1 =>
                        {
                            b1.Property<int>("TodoListId")
                                .HasColumnType("integer");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("TodoListId");

                            b1.ToTable("TodoLists");

                            b1.WithOwner()
                                .HasForeignKey("TodoListId");
                        });

                    b.Navigation("Colour")
                        .IsRequired();
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.CenovaPonuka", b =>
                {
                    b.Navigation("Polozky");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Objednavka", b =>
                {
                    b.Navigation("CenovePonuky");
                });

            modelBuilder.Entity("CRMBackend.Domain.AggregateRoots.Tovar", b =>
                {
                    b.Navigation("Varianty");
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.Firma", b =>
                {
                    b.Navigation("KontaktneOsoby");

                    b.Navigation("Objednavky");
                });

            modelBuilder.Entity("CRMBackend.Domain.Entities.TodoList", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
