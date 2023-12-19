using System.Collections.ObjectModel;
using System.Windows.Input;
using Frontend.Models;

namespace Frontend.ViewModel;

public class MainPageViewModel : ViewModelBase
{
    private readonly ServiceClient _client;
    private ObservableCollection<Damage> _damages;
    public ObservableCollection<Damage> Damages {
        get => _damages;
        set {
            _damages = value;
            OnPropertyChanged(nameof(Damages));
        }
    }
    public Damage SelectedDamage { get; set; }
    public ICommand AddCommand { get; set; }
    public ICommand DeleteCommand { get; set; }

    public MainPageViewModel(ServiceClient client)
    {
        _client = client;
        Damages = new ObservableCollection<Damage>();
        AddCommand = new Command(OpenCreateDamageWindow);
        DeleteCommand = new Command(DeleteDamage);
        LoadDamages();

        MessagingCenter.Subscribe<AddDamageViewModel, Damage>(this, "AddDamage", (sender, damage) => {
            CreateDamageAsync(damage);
        });
    }
    private void OpenCreateDamageWindow() {
        Shell.Current.GoToAsync(nameof(AddDamagePage));
    }

    public async Task LoadDamages() {
        var damages = await _client.DamageAllAsync();

        Device.BeginInvokeOnMainThread(() => {
            Damages.Clear();
            foreach (var damage in damages) {
                Damages.Add(damage);
            }
        });
    }

    private void DeleteDamage() {
        if (SelectedDamage != null)
        {
            DeleteDamageByIdAsync(SelectedDamage.Id);
        }
    }

    private async Task DeleteDamageByIdAsync(int id) {
        try {
            await _client.DamageDELETEAsync(id);
        } catch (ApiException ex) {
            Console.WriteLine($"Error in DeleteDamageByIdAsync: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        } finally {
            await LoadDamages();
        }
    }

    private async Task CreateDamageAsync(Damage damage)
    {
        try
        {
            await _client.DamagePOSTAsync(damage);
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Error in CreateDamageAsync: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
        finally
        {
            await LoadDamages();
        }
    }

}