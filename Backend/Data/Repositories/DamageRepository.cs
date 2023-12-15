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
        _damageSet.Add(damage);
        try
        {
            _dbContext.SaveChanges();
            return damage;
        }
        catch (Exception)
        {
            return null;
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