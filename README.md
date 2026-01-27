# DotSerial  

DotSerial is a lightweight cross-platform C# library for serializing and deserializing .NET objects.  
It allows developers to easily mark properties for serialization using a simple attribute, and provides functionality to save and load
objects from files in different data serialization formats.  

If you encounter any errors, you can create an issues at [GitHub](https://github.com/Dennis198/DotSerial/issues),

## Features  
- Simple attribute-based serialization with **`[DotSerialName]`** for custom naming
- Ignore properties with **`[DotSerialIgnore]`**
- Save objects to a file and load them back in different data serialization formats
- Serialize/Deserialize .NET objects  
- Developer-friendly and lightweight  

---

## Installation  
Add **DotSerial** to your project [nuget](https://www.nuget.org/packages/Dennis198.DotSerial).  
Or include the source in your project.  

---

## Usage  

### Mark properties with `DotSerialName`  
```csharp
using DotSerial;

public class Example
{
    [DotSerialName("Example_Boolean")]
    public bool Boolean { get; set; }

    [DotSerialName("Example_Number")]
    public int Number { get; set; }

    [DotSerialName("Example_Text")]
    public string Text { get; set; }
}
```

### Serialize and Deserialize  
```csharp
var obj = new Example
{
    Boolean = true,
    Number = 42,
    Text = "Hello DotSerial!"
};

// Serialize (Json)
var serialized = DotSerialJson.Serialize(obj);
//{
//  "Example_Boolean": "true",
//  "Example_Number": "42",
//  "Example_Text": "Hello DotSerial!"
//}

// Deserialize back
Example result = DotSerialJson.Deserialize<Example>(serialized);
```

### Save and Load from File  
```csharp
// Save to file
DotSerialJson.SaveToFile("example.json", obj);

// Load from file
Example resultLoad = DotSerialJson.LoadFromFile<Example>("example.json");
```

---

## Attribute Reference  

- **`[DotSerialName(string name)]`**  
  - Assign a custom **Name** to each property you want to serialize.  
  - The name must be unique within the class.
  - Properties without this attribute will be named after the propertie. 
- **`[DotSerialName(string name)]`**  
  - Properties with this attribute will be ignored.

Example:
```csharp
[DotSerialName("User_Name")]
public string Name { get; set; }

[DotSerialName("User_Age")]
public int Age { get; set; }

public int Occupation { get; set; }

[DotSerialIgnore]
public int Gender { get; set; }

//{
//  "User_Name": "Randy",
//  "User_Age": "42",
//  "Occupation": "Magician"
//}
```

---

## Notes   
- Use DotSerialXml for Xml, DotSerialJson for Json or DotSerialYaml for yaml.
- Properties without **`DotSerialName`** will be named as the propertie.
- Currently only **XMml Json and Yaml format** is supported. Other formats will be added in the future. 

---

## License  
MIT License – free to use and modify.  
