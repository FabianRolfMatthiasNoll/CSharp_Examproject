using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Frontend.Models;
using Frontend.Services;
using Location = Frontend.Models.Location;

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

    private bool ValidateForm()
    {
        var Validation = ValidatorHelper.ValidateDamage(Damage);
        ErrorMessage = Validation.errorMessage;
        return Validation.result;
    }


    public event EventHandler<Damage> OnEmployeeAdded;
}