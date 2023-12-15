using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Damages")]
public class Damage
{
    [Key]
    public int ID { get; set; }
    public string Description { get; set; }
    public DamageType Type { get; set; }
    public Location Location { get; set; }
}