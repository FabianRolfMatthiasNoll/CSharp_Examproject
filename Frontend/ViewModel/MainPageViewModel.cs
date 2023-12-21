using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Xml.Serialization;
using CommunityToolkit.Maui.Storage;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.ViewModel;

public class MainPageViewModel : ViewModelBase
{
    private readonly ServiceClient _client;
    private ObservableCollection<Damage> _damages;
    public ObservableCollection<Damage> Damages {
        get => _damages;
        set {
            if (_damages != value) {
                _damages = value;
                OnPropertyChanged(nameof(Damages));
            }
        }
    }

    private ObservableCollection<string> _errors;
    public ObservableCollection<string> Errors {
        get => _errors;
        set {
            if (_errors != value) {
                _errors = value;
                OnPropertyChanged(nameof(Errors));
            }
        }
    }
    public Damage SelectedDamage { get; set; }
    public ICommand AddCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand ReloadCommand { get; set; }
    public ICommand ExportCommand { get; set; }
    public ICommand ImportCommand { get; set; }
    private string _filePath;
    public bool HasErrors { get; set; } = false;

    public MainPageViewModel(ServiceClient client)
    {
        _client = client;

        Damages = new ObservableCollection<Damage>();
        Errors = new ObservableCollection<string>();
        AddCommand = new Command(OpenCreateDamageWindow);
        DeleteCommand = new Command(DeleteDamage);
        ReloadCommand = new Command(async () => await LoadDamages());
        ExportCommand = new Command(ExportDamages);
        ImportCommand = new Command(ImportDamages);
        LoadDamages();

        MessagingCenter.Subscribe<AddDamageViewModel, Damage>(this, "AddDamage", async (sender, damage) => {
            await CreateDamageAsync(damage);
            await LoadDamages();
        });

    }
    private void OpenCreateDamageWindow() {
        Shell.Current.GoToAsync(nameof(AddDamagePage));
    }

    private async void DeleteDamage() {
        if (SelectedDamage != null)
        {
            try {
                await _client.DamageDELETEAsync(SelectedDamage.Id);
            } catch (ApiException ex) {
                Console.WriteLine($"Error in DeleteDamageByIdAsync: {ex.Message}");
                if (ex.InnerException != null) {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            } finally {
                await LoadDamages();
            }
        }
    }

    private async void ExportDamages()
    {
        var result = await FolderPicker.Default.PickAsync();
        if (!result.IsSuccessful) return;
        _filePath = result.Folder.Path;
        XmlConverter.SerializeToXmlFile(Damages, _filePath + "/damages.xml");
    }

    private async void ImportDamages()
    {
        await LoadDamages();
        var result = await FolderPicker.Default.PickAsync();
        if (!result.IsSuccessful) return;
        //TODO: Check what happens if the user selects a folder with no damages.xml file
        _filePath = result.Folder.Path;
        var damages = XmlConverter.DeserializeFromXmlFile<ObservableCollection<Damage>>(_filePath + "/damages.xml");

        var newDamages = FindNewDamages(Damages, damages);

        foreach (var damage in newDamages)
        {
            await CreateDamageAsync(damage);
        }

        await LoadDamages();
    }


    public ObservableCollection<Damage> FindNewDamages(ObservableCollection<Damage> databaseList, ObservableCollection<Damage> importedList) {
        var newDamages = new ObservableCollection<Damage>();

        foreach (var importedDamage in importedList.Select((value, i) => (value, i))) {
            var (result, errorMessage) = ValidatorHelper.ValidateDamage(importedDamage.value);
            if (!result)
            {
                Errors.Add("XmlEntry[{importedDamage.i}]: " + errorMessage);
                HasErrors = true;
                continue;
            }
            if (!databaseList.Contains(importedDamage.value)) {
                newDamages.Add(importedDamage.value);
            }
        }
        return newDamages;
    }

    public async Task LoadDamages() {
        var damages = await _client.DamageAllAsync();
        var tempList = new List<Damage>(damages);
        Damages = new ObservableCollection<Damage>(tempList);
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
    }
}