using System.Collections.ObjectModel;
using System.Windows.Input;
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

    private bool _hasErrors = false;
    public bool HasErrors {
        get => _hasErrors;
        set {
            if (value == _hasErrors) return;
            _hasErrors = value;
            OnPropertyChanged();
        }
    }

    public MainPageViewModel(ServiceClient client)
    {
        _client = client;
        Damages = new ObservableCollection<Damage>();
        Errors = new ObservableCollection<string>();

        SetCommands();
        LoadDamages();

        MessagingCenter.Subscribe<AddDamageViewModel, Damage>(this, "AddDamage", async (sender, damage) => {
            await CreateDamageAsync(damage);
            await LoadDamages();
        });
    }

    private void SetCommands() {
        AddCommand = new Command(OpenCreateDamageWindow);
        DeleteCommand = new Command(DeleteDamage);
        ReloadCommand = new Command(async () => await LoadDamages());
        ExportCommand = new Command(ExportDamages);
        ImportCommand = new Command(ImportDamages);
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
                HasErrors = true;
                Errors.Add("An Error has occured during Damage Deletion: " + ex.Message);
            } finally {
                if (HasErrors) HideErrorsWithDelay();
                await LoadDamages();
            }
        }
    }

    private async void ExportDamages()
    {
        var result = await FolderPicker.Default.PickAsync();
        if (!result.IsSuccessful) return;
        XmlConverter.SerializeToXmlFile(Damages, result.Folder.Path + "/DamageExport.xml");
    }

    private async void ImportDamages() {
        try {
            var result = await FilePicker.Default.PickAsync();
            if (result == null) {
                return;
            }

            if (!result.FileName.EndsWith("DamageExport.xml", StringComparison.OrdinalIgnoreCase)) {
                HasErrors = true;
                Errors.Add("The selected file is not a DamageExport.xml file");
                return;
            }

            await ProcessDamagesFile(result.FullPath);
        } catch (Exception ex) {
            HasErrors = true;
            Errors.Add("An Error has occured during Damage Import: " + ex.Message);
        } finally {
            if (HasErrors) HideErrorsWithDelay();
        }
    }

    private async Task ProcessDamagesFile(string filePath) {
        var damages = XmlConverter.DeserializeFromXmlFile(filePath);
        var newDamages = FindNewDamages(Damages, damages);

        foreach (var damage in newDamages) {
            await CreateDamageAsync(damage);
        }

        await LoadDamages();
    }

    private async Task HideErrorsWithDelay() {
        await Task.Delay(15000);
        Errors.Clear();
        HasErrors = false;
    }

    public ObservableCollection<Damage> FindNewDamages(ObservableCollection<Damage> databaseList, ObservableCollection<Damage> importedList) {
        var newDamages = new ObservableCollection<Damage>();

        foreach (var importedDamage in importedList.Select((value, i) => (value, i))) {
            var (result, errorMessage) = ValidatorHelper.ValidateDamage(importedDamage.value);
            if (!result)
            {
                Errors.Add($"XmlEntry[{importedDamage.i}]:\n" + errorMessage);
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
        try {
            await _client.DamagePOSTAsync(damage);
        }
        catch (ApiException ex) {
            HasErrors = true;
            Errors.Add("An Error occured during Damage Report: " + ex.Message);
        } finally {
            if (HasErrors) HideErrorsWithDelay();
        }
    }
}