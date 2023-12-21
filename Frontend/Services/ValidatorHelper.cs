using System.Text.RegularExpressions;
using System.Text;
using Frontend.Models;

namespace Frontend.Services;

public static class ValidatorHelper
{
    public static (bool result, string errorMessage) ValidateDamage(Damage damage)
    {
        StringBuilder validationErrors = new StringBuilder();

        if (string.IsNullOrWhiteSpace(damage.Description)) {
            validationErrors.AppendLine("Description cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(damage.Location.Street)) {
            validationErrors.AppendLine("Street is required.");
        }

        if (string.IsNullOrWhiteSpace(damage.Location.StreetNumber)) {
            validationErrors.AppendLine("Street number is required.");
        }

        if (string.IsNullOrWhiteSpace(damage.Location.City)) {
            validationErrors.AppendLine("City is required.");
        }

        if (damage.Location.ZipCode == 0) {
            validationErrors.AppendLine("Zip code is required.");
        }

        //TODO: check if zip code is letters

        if (damage.Location.Street != null) {
            if (Regex.IsMatch(damage.Location.Street, @"\d")) {
                validationErrors.AppendLine("Street should not contain numbers.");
            }
        }

        if (damage.Location.StreetNumber != null) {

            if (!Regex.IsMatch(damage.Location.StreetNumber, @"^\d")) {
                validationErrors.AppendLine("Street number must start with a number.");
            }
        }

        if (damage.Location.City != null) {

            if (Regex.IsMatch(damage.Location.City, @"\d")) {
                validationErrors.AppendLine("City should not contain numbers.");
            }
        }

        var ErrorMessage = validationErrors.ToString();

        return (string.IsNullOrEmpty(ErrorMessage), ErrorMessage);
    }
}