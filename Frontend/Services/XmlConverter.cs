using System.Xml.Serialization;

namespace Frontend.Services;

public static class XmlConverter
{
    public static void SerializeToXmlFile<T>(T obj, string filePath) {
        var serializer = new XmlSerializer(typeof(T));
        using var fileStream = new FileStream(filePath, FileMode.Create);
        serializer.Serialize(fileStream, obj);
    }
    public static T DeserializeFromXmlFile<T>(string filepath) {
        var serializer = new XmlSerializer(typeof(T));
        using var fileStream = new FileStream(filepath, FileMode.Open);
        return (T)serializer.Deserialize(fileStream);
    }
}