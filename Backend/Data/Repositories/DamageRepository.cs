using Backend.Data.Contexts;
using Backend.Data.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories;

public class DamageRepository : IDamageRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<Damage> _damageSet;

    public DamageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _damageSet = _dbContext.Set<Damage>();
    }

    public List<Damage> GetAllDamages()
    {
        return _damageSet.ToList();
    }

    public Damage GetDamageById(int id) {
        var damage = _damageSet.SingleOrDefault(b => b.ID == id);
        return damage;
    }

    public Damage CreateDamage(Damage damage)
    {
        if (damage == null) {
            throw new ArgumentNullException(nameof(damage));
        }

        try {
            _damageSet.Add(damage);
            _dbContext.SaveChanges();
            return damage;
        } catch (Exception ex) {
            throw new InvalidOperationException("Error occurred while creating damage.", ex);
        }
    }

    public Damage UpdateDamage(Damage damage) {
        var existingDamage = _damageSet.Find(damage.ID);
        if (existingDamage == null) {
            throw new InvalidOperationException($"Damage with ID {damage.ID} not found.");
        }

        _dbContext.Entry(existingDamage).CurrentValues.SetValues(damage);
        _dbContext.SaveChanges();

        return existingDamage;
    }

    public bool DeleteDamageById(int Id) {
        var damage = _damageSet.SingleOrDefault(b => b.ID == Id);
        if (damage == null)
        {
            return false;
        }
        _damageSet.Remove(damage);
        try {
            _dbContext.SaveChanges();
            return true;
        } catch (Exception) {
            return false;
        }
    }
}