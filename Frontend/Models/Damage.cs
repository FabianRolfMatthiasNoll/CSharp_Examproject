namespace Frontend.Models;

public partial class Damage
{
    public override bool Equals(object obj) {
        if (obj is Damage other) {
            return Description == other.Description
                   /*&& Type == other.Type*/
                   && Location.Equals(other.Location);
        }
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Description, Type, Location);
    }
}