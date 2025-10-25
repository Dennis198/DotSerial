# DotSerial  

DotSerial is a lightweight cross-platform C# library for serializing and deserializing .NET objects.  
It allows developers to easily mark properties for serialization using a simple attribute, and provides functionality to save and load
objects from files in different data serialization formats.  

If you encounter any errors, you can create an issues at [GitHub](https://github.com/Dennis198/DotSerial/issues),

## ✨ Features  
- 🚀 Simple attribute-based serialization with **`[DSPropertyID]`**  
- 📂 Save objects to a file and load them back in different data serialization formats
- 🔄 Serialize/Deserialize .NET objects  
- 🎯 Developer-friendly and lightweight  

---

## 📦 Installation  
Add **DotSerial** to your project [nuget](https://www.nuget.org/packages/Dennis198.DotSerial).  
Or include the source in your project.  

---

## ⚡ Usage  

### Mark properties with `DSPropertyID`  
```csharp
using DotSerial;

public class Example
{
    [DSPropertyID(0)]
    public bool Boolean { get; set; }

    [DSPropertyID(1)]
    public int Number { get; set; }

    [DSPropertyID(2)]
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

// Serialize (Xml)
var serialized = DotSerialXML.Serialize(obj);

// Deserialize back
Example result = DotSerialXML.Deserialize<Example>(serialized);
```

### Save and Load from File  
```csharp
// Save to file
DotSerialXML.SaveToFile("example.xml", obj);

// Load from file
Example resultLoad = DotSerialXML.LoadFromFile<Example>("example.xml");
```

---

## 🛠️ Attribute Reference  

- **`[DSPropertyID(int id)]`**  
  Assign a unique **ID** to each property you want to serialize.  
  - The ID must be unique within the class.
  - Properties without this attribute will not be serialized.  

Example:
```csharp
[DSPropertyID(0)]
public string Name { get; set; }

[DSPropertyID(1)]
public int Age { get; set; }
```

---

## 📌 Notes   
- Use DotSerialXML for Xml, DotSerialJSON for Json or DotSerialYAML for yaml.
- Properties without **`DSPropertyID`** are ignored.
- ID '-1' should not be used.
- Currently only **XML, Json and Yaml format** is supported. Other formats will be added in the future. 

---

## 📜 License  
MIT License – free to use and modify.  
