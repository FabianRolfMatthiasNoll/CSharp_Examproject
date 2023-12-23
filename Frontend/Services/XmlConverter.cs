using Frontend.Models;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Frontend.Services;

public static class XmlConverter
{
    public static void SerializeToXmlFile(ObservableCollection<Damage> damageList, string filePath) {
        var serializer = new XmlSerializer(typeof(ObservableCollection<Damage>));
        using var fileStream = new FileStream(filePath, FileMode.Create);
        serializer.Serialize(fileStream, damageList);
    }
    public static ObservableCollection<Damage> DeserializeFromXmlFile(string filepath) {
        var serializer = new XmlSerializer(typeof(ObservableCollection<Damage>));
        using var fileStream = new FileStream(filepath, FileMode.Open);
        return (ObservableCollection<Damage>)serializer.Deserialize(fileStream);
    }
}