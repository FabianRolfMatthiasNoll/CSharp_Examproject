namespace Frontend.Data.Interfaces;

public interface IFolderPicker {
    Task<string> PickFolderAsync();
}
