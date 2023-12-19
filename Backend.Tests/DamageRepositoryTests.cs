using Backend.Data.Contexts;
using Backend.Data.Interfaces;
using Backend.Data.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Backend.Tests;

public class DamageRepositoryTests : IDisposable {
    private readonly ApplicationDbContext _context;
    private readonly DamageRepository _repository;

    public DamageRepositoryTests() {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new DamageRepository(_context);

        InitializeTestData();
    }

    private void InitializeTestData() {
        _context.Database.EnsureDeleted();

        var testDamages = new List<Damage>
        {
            new Damage
            {
                ID = 1,
                Description = "Broken street lamp",
                Type = DamageType.StreetLamp,
                Location = new Location { Street = "Main St", StreetNumber = "123", ZipCode = 12345, City = "Metropolis" }
            },
            new Damage
            {
                ID = 2,
                Description = "Large pothole",
                Type = DamageType.Pothole,
                Location = new Location { Street = "Second St", StreetNumber = "456", ZipCode = 54321, City = "Gotham" }
            }
        };

        _context.Set<Damage>().AddRange(testDamages);
        _context.SaveChanges();
    }

    // Test for GetAllDamages
    [Fact]
    public void GetAllDamages_ReturnsAllDamages() {
        var results = _repository.GetAllDamages();
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void GetAllDamages_ReturnsEmptyListIfNoDamages() {
        _context.Set<Damage>().RemoveRange(_context.Set<Damage>());
        _context.SaveChanges();

        var results = _repository.GetAllDamages();
        Assert.Empty(results);
    }

    // Test for GetDamageById
    [Fact]
    public void GetDamageById_ReturnsCorrectDamage() {
        var result = _repository.GetDamageById(1);
        Assert.NotNull(result);
        Assert.Equal(1, result.ID);
    }

    [Fact]
    public void GetDamageById_ReturnsNullIfDamageDoesNotExist() {
        var result = _repository.GetDamageById(999);
        Assert.Null(result);
    }

    // Test for CreateDamage
    [Fact]
    public void CreateDamage_AddsNewDamage()
    {
        var newDamage = new Damage
        {
            ID = 3,
            Description = "Larger pothole",
            Type = DamageType.Pothole,
            Location = new Location { Street = "third St", StreetNumber = "789", ZipCode = 78932, City = "Tuttlingen" }
        };
        _repository.CreateDamage(newDamage);

        var damage = _context.Set<Damage>().Find(3);
        Assert.NotNull(damage);
        Assert.Equal(3, _context.Set<Damage>().Count());
    }

    [Fact]
    public void CreateDamage_NullDamage_ThrowsArgumentNullException() {
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(() => _repository.CreateDamage(null));
        Assert.Equal("damage", exception.ParamName);
    }

    // Test for UpdateDamage
    [Fact]
    public void UpdateDamage_UpdatesExistingDamage() {
        var updateDamage = new Damage {
            ID = 1,
            Description = "Fixed street lamp",
            Type = DamageType.Other,
            Location = new Location { Street = "Main St", StreetNumber = "123", ZipCode = 12345, City = "Metropolis" }
        };
        _repository.UpdateDamage(updateDamage);

        var updatedEntity = _context.Set<Damage>().Find(1);

        // Because the Reference of both objects is different, we need to compare the properties
        Assert.NotNull(updatedEntity);
        Assert.Equal(updateDamage.Description, updatedEntity.Description);
        Assert.Equal(updateDamage.Type, updatedEntity.Type);
        Assert.Equal(updateDamage.Location.Street, updatedEntity.Location.Street);
        Assert.Equal(updateDamage.Location.StreetNumber, updatedEntity.Location.StreetNumber);
        Assert.Equal(updateDamage.Location.ZipCode, updatedEntity.Location.ZipCode);
        Assert.Equal(updateDamage.Location.City, updatedEntity.Location.City);
    }

    [Fact]
    public void UpdateDamage_ThrowsExceptionWhenNotFound() {
        var updateDamage = new Damage {
            ID = 999,
            Description = "MissingNo",
            Type = DamageType.Other,
            Location = new Location { Street = "Pokemon St", StreetNumber = "123", ZipCode = 12345, City = "Alabasta" }
        };

        Assert.Throws<InvalidOperationException>(() => _repository.UpdateDamage(updateDamage));
    }

    // Test for DeleteDamageById
    [Fact]
    public void DeleteDamageById_DeletesExistingDamage() {
        var success = _repository.DeleteDamageById(1);
        Assert.True(success);
        Assert.Equal(1, _context.Set<Damage>().Count());
    }

    [Fact]
    public void DeleteDamageById_ReturnsFalseWhenNotFound() {
        var success = _repository.DeleteDamageById(999);
        Assert.False(success);
    }

    public void Dispose() {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
