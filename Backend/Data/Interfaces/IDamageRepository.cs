using Backend.Models;

namespace Backend.Data.Interfaces;

public interface IDamageRepository {
    List<Damage> GetAllDamages();
    Damage GetDamageById(int id);
    Damage CreateDamage(Damage damage);
    Damage UpdateDamage(Damage damage);
    bool DeleteDamageById(int id);
}
