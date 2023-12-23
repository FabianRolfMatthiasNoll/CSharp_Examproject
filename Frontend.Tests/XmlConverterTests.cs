using System.Collections.ObjectModel;
using Location = Frontend.Models.Location;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.Tests;

[TestClass]
public class XmlConverterTests {
    private string testFilePath = "testData.xml";
    private ObservableCollection<Damage> mockDamages;

    [TestInitialize]
    public void Initialize() {
        // Setup mock data for testing
        mockDamages = new ObservableCollection<Damage> {
            new Damage { Id = 1, Description = "Scratch", Type = DamageType._0,
                         Location = new Location { Street = "Main St", StreetNumber = "123", ZipCode = 12345, City = "Testville" } },
            new Damage { Id = 2, Description = "Dent", Type = DamageType._1,
                         Location = new Location { Street = "Second St", StreetNumber = "456", ZipCode = 23456, City = "Sampletown" } }
        };
    }

    [TestCleanup]
    public void Cleanup() {
        if (File.Exists(testFilePath)) {
            File.Delete(testFilePath);
        }
    }

    [TestMethod]
    public void SerializeToXmlFile_ValidData_FileCreated() {
        XmlConverter.SerializeToXmlFile(mockDamages, testFilePath);
        Assert.IsTrue(File.Exists(testFilePath));

        string fileContents = File.ReadAllText(testFilePath);
        Assert.IsTrue(fileContents.Contains("Scratch"));
        Assert.IsTrue(fileContents.Contains("Dent"));
    }

    [TestMethod]
    public void SerializeToXmlFile_EmptyCollection_FileCreatedWithEmptyContent() {
        var emptyDamages = new ObservableCollection<Damage>();
        XmlConverter.SerializeToXmlFile(emptyDamages, testFilePath);
        Assert.IsTrue(File.Exists(testFilePath));

        string fileContents = File.ReadAllText(testFilePath);
        Assert.IsTrue(fileContents.Contains("<ArrayOfDamage"));
    }

    [TestMethod]
    [ExpectedException(typeof(DirectoryNotFoundException))]
    public void SerializeToXmlFile_InvalidPath_ThrowsException() {
        XmlConverter.SerializeToXmlFile(mockDamages, "Z:\\invalidPath\\testData.xml");
    }

    [TestMethod]
    public void DeserializeFromXmlFile_ValidFile_SuccessfulDeserialization() {
        XmlConverter.SerializeToXmlFile(mockDamages, testFilePath);
        var actualDamages = XmlConverter.DeserializeFromXmlFile(testFilePath);
        Assert.AreEqual(mockDamages.Count, actualDamages.Count);

        for (int i = 0; i < mockDamages.Count; i++) {
            Assert.AreEqual(mockDamages[i].Description, actualDamages[i].Description);
            Assert.AreEqual(mockDamages[i].Type, actualDamages[i].Type);
            Assert.AreEqual(mockDamages[i].Location.City, actualDamages[i].Location.City);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void DeserializeFromXmlFile_NonExistentFile_ThrowsFileNotFoundException() {
        XmlConverter.DeserializeFromXmlFile("nonexistent.xml");
    }

    [TestMethod]
    public void DeserializeFromXmlFile_EmptyFile_ReturnsEmptyCollection() {
        var emptyDamages = new ObservableCollection<Damage>();
        XmlConverter.SerializeToXmlFile(emptyDamages, testFilePath);

        var result = XmlConverter.DeserializeFromXmlFile(testFilePath);

        Assert.AreEqual(0, result.Count, "Deserialized collection should be empty");
    }

}

