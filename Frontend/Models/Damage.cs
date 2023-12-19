namespace Frontend.Models;

public class Damage
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DamageType Type { get; set; }
    public Location Location { get; set; }
}