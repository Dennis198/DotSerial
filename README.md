# DotSerial

DotSerial is a lightweight cross-platform C# library for serializing and deserializing .NET objects.
It allows developers to easily mark properties for serialization using a simple attribute, and provides functionality to save and load objects from files in different data serialization formats.

If you encounter any errors, you can create an issue at [GitHub](https://github.com/Dennis198/DotSerial/issues).

## Features

- Serialize and deserialize .NET objects to **JSON**, **XML**, **YAML**, and **Toon**
- Simple attribute-based customization with **`[DotSerialName]`** and **`[DotSerialIgnore]`**
- Save objects to a file and load them back
- Navigate and manipulate serialized data trees with the **`DSNode`** API
- Developer-friendly and lightweight

---

## Installation

Add **DotSerial** to your project via [NuGet](https://www.nuget.org/packages/Dennis198.DotSerial).
Or include the source in your project.

---

## Usage

### Define a class

```csharp
using DotSerial.Attributes;

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

### Serialize and Deserialize with `DSConverter`

`DSConverter` provides static methods for converting objects to and from serialized strings and files. Use `SerializeStrategy` to choose the format.

```csharp
using DotSerial;

var obj = new Example
{
    Boolean = true,
    Number = 42,
    Text = "Hello DotSerial!"
};

// Serialize to a JSON string
string json = DSConverter.Serialize(obj, SerializeStrategy.Json);
// {
//   "Example_Boolean": "true",
//   "Example_Number": "42",
//   "Example_Text": "Hello DotSerial!"
// }

// Deserialize back to an object
Example result = DSConverter.Deserialize<Example>(json, SerializeStrategy.Json);

// Serialize to TOON
string toon = DSConverter.Serialize(obj, SerializeStrategy.Toon);

// Deserialize from YAML
Example fromYaml = DSConverter.Deserialize<Example>(yamlString, SerializeStrategy.Yaml);
```

### Save and Load from File

```csharp
// Save to file
DSConverter.SaveToFile("example.json", obj, SerializeStrategy.Json);

// Load from file
Example loaded = DSConverter.LoadFromFile<Example>("example.json", SerializeStrategy.Json);
```

### Working with `DSNode`

`DSNode` represents a node in the DotSerial document tree. It implements `IDictionary<string, DSNode>`, so you can navigate, read, and modify serialized data without mapping it to a class.

#### Parse a string into a node tree

```csharp
DSNode root = DSNode.FromString(json, SerializeStrategy.Json);
// or via DSConverter:
DSNode root = DSConverter.ToNodeFromString(json, SerializeStrategy.Json);
```

#### Convert an object to a node tree

```csharp
DSNode node = DSNode.ToNode(obj, SerializeStrategy.Json);
// or via DSConverter:
DSNode node = DSConverter.ToNode(obj, SerializeStrategy.Json);
```

#### Read node values

```csharp
// Access a child by key
DSNode child = root["Example_Text"];

// Get the raw string value of a leaf node
string? value = child.GetNodeValue();

// Check the node type (Object, Value, or Array)
NodeType type = root.GetNodeType();
```

#### Modify nodes

```csharp
// Set a leaf value (overloads for string, int, bool, double, DateTime, Guid, etc.)
child.SetNodeValue("Updated text");
child.SetNodeValue(100);
child.SetNodeValue(true);

// Add a new child node
DSNode newChild = DSNode.ToNode("new value", SerializeStrategy.Json, "NewKey");
root.Add("NewKey", newChild);

// Remove a child
root.Remove("Example_Number");

// Clear all children
root.Clear();
```

#### Iterate over children

```csharp
foreach (var (key, childNode) in root)
{
    Console.WriteLine($"{key}: {childNode.GetNodeValue()}");
}
```

#### Serialize a node tree back to a string

```csharp
string output = root.Stringify();
```

#### Deserialize a string directly into an object

```csharp
Example obj = DSNode.ToObject<Example>(json, SerializeStrategy.Json);
```

---

## Serialization Strategies

Use the `SerializeStrategy` enum to choose a format:

| Strategy                    | Format |
|-----------------------------|--------|
| `SerializeStrategy.Json`    | JSON   |
| `SerializeStrategy.Xml`     | XML    |
| `SerializeStrategy.Yaml`    | YAML   |
| `SerializeStrategy.Toon`    | Toon   |

---

## Attribute Reference

- **`[DotSerialName(string name)]`**
  - Assign a custom name to a property for serialization.
  - The name must be unique within the class.
  - Properties without this attribute use the property name as-is.
- **`[DotSerialIgnore]`**
  - Properties with this attribute are excluded from serialization.

```csharp
[DotSerialName("User_Name")]
public string Name { get; set; }

[DotSerialName("User_Age")]
public int Age { get; set; }

public string Occupation { get; set; }

[DotSerialIgnore]
public string Gender { get; set; }

// JSON output:
// {
//   "User_Name": "Randy",
//   "User_Age": "42",
//   "Occupation": "Magician"
// }
```

---

## API Overview

### `DSConverter` (static)

| Method | Description |
|--------|-------------|
| `Serialize(object?, SerializeStrategy)` | Serialize an object to a string |
| `Deserialize<U>(ReadOnlySpan<char>, SerializeStrategy)` | Deserialize a string to an object |
| `SaveToFile(string, object?, SerializeStrategy)` | Serialize and save to a file |
| `LoadFromFile<U>(string, SerializeStrategy)` | Load and deserialize from a file |
| `ToNode(object?, SerializeStrategy)` | Convert an object to a `DSNode` tree |
| `ToNodeFromString(ReadOnlySpan<char>, SerializeStrategy)` | Parse a string into a `DSNode` tree |

### `DSNode`

| Method / Property | Description |
|-------------------|-------------|
| `FromString(ReadOnlySpan<char>, SerializeStrategy)` | Parse a string into a `DSNode` tree (static) |
| `Stringify(object?, SerializeStrategy)` | Serialize an object to a string (static) |
| `ToNode(object?, SerializeStrategy, string?)` | Convert an object to a `DSNode` (static) |
| `ToObject<U>(ReadOnlySpan<char>, SerializeStrategy)` | Deserialize a string to an object (static) |
| `Stringify()` | Serialize this node tree to a string |
| `GetNodeType()` | Get the type of the node (`Value`, `Object`, or `Array`) |
| `GetNodeValue()`/`TryGetNodeValue()` | Get the raw string value of a leaf node |
| `SetNodeValue(...)` | Set the value of a leaf node (many type overloads) |
| `this[key]` | Get or set a child node by key |
| `Add(key, value)`/`TryAdd(key, value)` | Add a child node |
| `Remove(key)` | Remove a child node |
| `ContainsKey(key)` | Check if a child with the key exists |
| `Count` | Number of child nodes |
| `Keys` / `Values` | Child keys and nodes |
| `IsQuoted` | Is true, if the value must be quoted in the strategy |

---

## License

MIT License – free to use and modify.
