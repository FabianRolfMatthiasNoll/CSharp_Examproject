namespace Frontend.Models;

public partial class Location
{
    public override bool Equals(object obj) {
        if (obj is Location other) {
            return Street == other.Street
                   && StreetNumber == other.StreetNumber
                   && ZipCode == other.ZipCode
                   && City == other.City;
        }
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Street, StreetNumber, ZipCode, City);
    }
}