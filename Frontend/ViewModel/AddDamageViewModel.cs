using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Frontend.ViewModel;

public class AddDamageViewModel : ViewModelBase
{
    private Damage _damage = new Damage();
    public Damage Damage {
        get => _damage;
        set {
            _damage = value;
            OnPropertyChanged(nameof(Damage));
        }
    }
    private string _errorMessage;
    public string ErrorMessage {
        get => _errorMessage;
        set {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
    public ICommand OKCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public AddDamageViewModel() {
        OKCommand = new Command(ExecuteOkCommand);
        CancelCommand = new Command(ExecuteCancelCommand);
        Damage.Location = new Location();
    }

    public void ExecuteOkCommand(object sender) {
        if (!ValidateForm())
        {
            MessagingCenter.Send(this, "ValidationError", ErrorMessage);
            return;
        }
        MessagingCenter.Send(this, "AddDamage", Damage);
        Shell.Current.GoToAsync("..");
    }

    public void ExecuteCancelCommand(object sender) {
        Shell.Current.GoToAsync("..");
    }

    private bool ValidateForm() {
        StringBuilder validationErrors = new StringBuilder();

        if (string.IsNullOrWhiteSpace(Damage.Description)) {
            validationErrors.AppendLine("Description cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(Damage.Location.Street)) {
            validationErrors.AppendLine("Street is required.");
        }

        if (string.IsNullOrWhiteSpace(Damage.Location.StreetNumber)) {
            validationErrors.AppendLine("Street number is required.");
        }

        if (string.IsNullOrWhiteSpace(Damage.Location.City)) {
            validationErrors.AppendLine("City is required.");
        }

        if (Damage.Location.ZipCode == 0) {
            validationErrors.AppendLine("Zip code is required.");
        }

        if (Damage.Location.Street != null)
        {
            if (Regex.IsMatch(Damage.Location.Street, @"\d")) {
                validationErrors.AppendLine("Street should not contain numbers.");
            }
        }

        if (Damage.Location.StreetNumber != null) {

            if (!Regex.IsMatch(Damage.Location.StreetNumber, @"^\d")) {
                validationErrors.AppendLine("Street number must start with a number.");
            }
        }

        if (Damage.Location.City != null) {

            if (Regex.IsMatch(Damage.Location.City, @"\d")) {
                validationErrors.AppendLine("City should not contain numbers.");
            }
        }

        ErrorMessage = validationErrors.ToString();

        return string.IsNullOrEmpty(ErrorMessage);
    }


    public event EventHandler<Damage> OnEmployeeAdded;
}