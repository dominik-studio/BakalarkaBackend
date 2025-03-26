using CRMBackend.Domain.AggregateRoots;
using CRMBackend.Domain.Exceptions;
using CRMBackend.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics;
using CRMBackend.Domain.Entities;
using CRMBackend.Domain.ValueObjects;

namespace CRMBackend.Domain.UnitTests.AggregateRoots.ObjednavkaAggregate;

[TestFixture]
public class ObjednavkaTests
{
    private Objednavka _objednavka;
    private CenovaPonuka _platnaPonuka;

    [SetUp]
    public void Setup()
    {
        var firma = new Firma
        {
            Nazov = "Test Firma",
            ICO = "12345678",
            Adresa = new Adresa
            {
                Ulica = "Test Ulica",
                Mesto = "Test Mesto",
                PSC = "12345",
                Krajina = "Test Krajina"
            }
        };
        
        var kontaktnaOsoba = new KontaktnaOsoba
        {
            Id = 1,
            FirmaId = 1,
            Firma = firma,
            Meno = "Test Meno",
            Priezvisko = "Test Priezvisko",
            Telefon = "123456789",
            Email = "test@example.com"
        };

        _platnaPonuka = new CenovaPonuka()
        {
            Objednavka = new Objednavka()
            {
                KontaktnaOsoba = kontaktnaOsoba
            }
        };
        
        _objednavka = new Objednavka
        {
            KontaktnaOsoba = kontaktnaOsoba
        };
    }

    
    [Test]
    public void ZrusObjednavku_Throws_WhenAlreadyZrusene()
    {
        _objednavka.ZrusObjednavku();
        _objednavka.Invoking(o => o.ZrusObjednavku())
            .Should().Throw<DomainValidationException>()
            .WithMessage("Objednávka je už zrušená.");
    }

    [Test]
    public void ZrusObjednavku_Throws_WhenInVybaveneFaza()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        _objednavka.SetFaza(ObjednavkaFaza.VyrobaCaka);
        _objednavka.SetFaza(ObjednavkaFaza.OdoslanieCaka);
        _objednavka.SetFaza(ObjednavkaFaza.PlatbaCaka);
        _objednavka.OznacAkoZaplatene();
        
        _objednavka.Invoking(o => o.ZrusObjednavku())
            .Should().Throw<DomainValidationException>()
            .WithMessage("Hotovú objednávku nemožno zrušiť.");
    }

    
    [Test]
    public void OznacAkoZaplatene_Throws_WhenAlreadyZaplatene()
    {
        _objednavka.OznacAkoZaplatene();
        _objednavka.Invoking(o => o.OznacAkoZaplatene())
            .Should().Throw<DomainValidationException>()
            .WithMessage("Objednávka je už zaplatená.");
    }

    [Test]
    public void OznacAkoZaplatene_SetsVybavene_WhenInPlatbaCaka()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        
        _objednavka.SetFaza(ObjednavkaFaza.VyrobaCaka);
        _objednavka.SetFaza(ObjednavkaFaza.OdoslanieCaka);
        _objednavka.SetFaza(ObjednavkaFaza.PlatbaCaka);
        
        _objednavka.OznacAkoZaplatene();
        
        _objednavka.Zaplatene.Should().BeTrue();
        _objednavka.Faza.Should().Be(ObjednavkaFaza.Vybavene);
    }

    
    [Test]
    public void AddCenovaPonuka_Throws_WhenNotInNacenenie()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        
        _objednavka.Invoking(o => o.AddCenovaPonuka(_platnaPonuka))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Cenovú ponuku možno pridať iba vo fáze Nacenenie.");
    }

    [Test]
    public void AddCenovaPonuka_Throws_WhenLastPonukaNotZrusene()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.Invoking(o => o.AddCenovaPonuka(_platnaPonuka))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Nemôžete pridať novú cenovú ponuku, pokiaľ posledná cenová ponuka nie je zrušená.");
    }

    
    [Test]
    public void PoslanaCenovaPonuka_Throws_WhenNoPonuka()
    {
        _objednavka.Invoking(o => o.PoslanaCenovaPonuka())
            .Should().Throw<DomainValidationException>()
            .WithMessage("Žiadna cenová ponuka na označenie ako poslaná.");
    }

    [Test]
    public void PoslanaCenovaPonuka_SetsCorrectStav()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        
        _objednavka.PoslanaCenovaPonuka();

        _objednavka.PoslednaCenovaPonuka.Should().NotBeNull();
        _objednavka.PoslednaCenovaPonuka!.Stav.Should().Be(StavCenovejPonuky.Poslane);
        _objednavka.Faza.Should().Be(ObjednavkaFaza.NacenenieCaka);
    }

    
    [Test]
    public void SchvalitCenovuPonuku_Throws_WhenNotPoslane()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.Invoking(o => o.SchvalitCenovuPonuku())
            .Should().Throw<DomainValidationException>()
            .WithMessage("Cenová ponuka musí byť v stave Poslane, aby sa dala schváliť.");
    }

    [Test]
    public void SchvalitCenovuPonuku_SetsCorrectFaza()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        
        _objednavka.SchvalitCenovuPonuku();
        
        _objednavka.PoslednaCenovaPonuka.Should().NotBeNull();
        _objednavka.PoslednaCenovaPonuka!.Stav.Should().Be(StavCenovejPonuky.Schvalene);
        _objednavka.Faza.Should().Be(ObjednavkaFaza.VyrobaNeriesene);
    }

    
    [Test]
    public void SetFaza_Throws_WhenInvalidTransitionFromVyrobaNeriesene()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        
        _objednavka.Invoking(o => o.SetFaza(ObjednavkaFaza.OdoslanieCaka))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Z fázy 'VyrobaNeriesene' môžete prejsť iba do fázy 'VyrobaNemozna' alebo 'VyrobaCaka'.");
    }

    [Test]
    public void SetFaza_AllowsValidTransitionFromVyrobaNemozna()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        _objednavka.SetFaza(ObjednavkaFaza.VyrobaNemozna);
        
        _objednavka.SetFaza(ObjednavkaFaza.VyrobaNeriesene);
        _objednavka.Faza.Should().Be(ObjednavkaFaza.VyrobaNeriesene);
    }

    [Test]
    public void SetFaza_Throws_WhenSettingSameFaza()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        
        _objednavka.Invoking(o => o.SetFaza(ObjednavkaFaza.VyrobaNeriesene))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Nemôžete nastaviť rovnakú fázu, v ktorej sa objednávka už nachádza.");
    }

    
    [Test]
    public void SetNaplanovanyDatumVyroby_Throws_WhenInvalidFaza()
    {
        
        _objednavka.Invoking(o => o.SetNaplanovanyDatumVyroby(DateTime.Now))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Dátum výroby sa dá nastaviť iba vo fázach Vyroba.");
    }

    [Test]
    public void SetNaplanovanyDatumVyroby_SetsFazaToVyrobaCaka()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        
        _objednavka.SetNaplanovanyDatumVyroby(DateTime.Now);
        
        _objednavka.NaplanovanyDatumVyroby.Should().NotBeNull();
        _objednavka.Faza.Should().Be(ObjednavkaFaza.VyrobaCaka);
    }

    
    [Test]
    public void ZrusCenovuPonuku_ResetsFazaToNacenenie()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        
        _objednavka.ZrusCenovuPonuku();
        
        _objednavka.Faza.Should().Be(ObjednavkaFaza.Nacenenie);
        _objednavka.PoslednaCenovaPonuka.Should().BeNull();
    }

    [Test]
    public void ZrusCenovuPonuku_Throws_WhenSchvalene()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        
        _objednavka.Invoking(o => o.ZrusCenovuPonuku())
            .Should().Throw<DomainValidationException>()
            .WithMessage("Schválenú cenovú ponuku nemožno zrušiť*");
    }

    
    [Test]
    public void SetFaza_Throws_WhenInvalidTransitionFromVyrobaNemozna()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        _objednavka.SetFaza(ObjednavkaFaza.VyrobaNemozna);
        
        _objednavka.Invoking(o => o.SetFaza(ObjednavkaFaza.VyrobaCaka))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Z fázy 'VyrobaNemozna'*");
    }
    
    
    [Test]
    public void SetFaza_SkipsToVybavene_WhenAlreadyPaid()
    {
        
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        _objednavka.SetFaza(ObjednavkaFaza.VyrobaCaka);
        _objednavka.SetFaza(ObjednavkaFaza.OdoslanieCaka);
        
        
        _objednavka.OznacAkoZaplatene();
        _objednavka.SetFaza(ObjednavkaFaza.Vybavene);
        
        _objednavka.Faza.Should().Be(ObjednavkaFaza.Vybavene);
    }

    
    [Test]
    public void SetFaza_Throws_WhenOrderIsCancelled()
    {
        _objednavka.ZrusObjednavku();
        
        _objednavka.Invoking(o => o.SetFaza(ObjednavkaFaza.VyrobaCaka))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Zrušená objednávka nemôže meniť fázu.");
    }

    
    [Test]
    public void SetFaza_Throws_WhenInvalidTransitionFromVyrobaCaka()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        _objednavka.SetNaplanovanyDatumVyroby(DateTime.Now);
        
        _objednavka.Invoking(o => o.SetFaza(ObjednavkaFaza.PlatbaCaka))
            .Should().Throw<DomainValidationException>()
            .WithMessage("Z fázy 'VyrobaCaka' môžete prejsť iba do fázy 'OdoslanieCaka' alebo 'VyrobaNemozna'.");
    }

    [Test]
    public void SetFaza_AllowsTransitionFromVyrobaCakaToVyrobaNemozna()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.SchvalitCenovuPonuku();
        _objednavka.SetNaplanovanyDatumVyroby(DateTime.Now);
        
        _objednavka.SetFaza(ObjednavkaFaza.VyrobaNemozna);
        
        _objednavka.Faza.Should().Be(ObjednavkaFaza.VyrobaNemozna);
    }

    
    [Test]
    public void SetOcakavanyDatumDorucenia_WorksInAnyState()
    {
        var testDate = DateTime.Now.AddDays(10);
        
        _objednavka.SetOcakavanyDatumDorucenia(testDate);
        _objednavka.OcakavanyDatumDorucenia.Should().Be(testDate);
    }

    
    [Test]
    public void ZrusCenovuPonuku_AllowsNewPonukaAfter()
    {
        _objednavka.AddCenovaPonuka(_platnaPonuka);
        _objednavka.PoslanaCenovaPonuka();
        _objednavka.ZrusCenovuPonuku();
        
        _objednavka.Invoking(o => o.AddCenovaPonuka(_platnaPonuka))
            .Should().NotThrow();
    }

    
    [Test]
    public void SetPoznamka_UpdatesWithoutRestrictions()
    {
        const string testNote = "Testovacia poznámka";
        
        _objednavka.SetPoznamka(testNote);
        _objednavka.Poznamka.Should().Be(testNote);
    }
} 
