using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Owned]
public class Location
{
    [Column("Street")]
    public string Street { get; set; }
    [Column("StreetNo")]
    public string StreetNumber { get; set; }
    [Column("Postcode")]
    public int ZipCode { get; set; }
    [Column("City")]
    public string City { get; set; }

}