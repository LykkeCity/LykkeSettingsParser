# LykkeSettingsParser
The library allows you to parse JSON string in to object. If any of field won't be filled by json structure an Exception will throw.
## How to use
There is only one static generic function in a library
```cs
 var model = SettingsProcessor.Process<ModelClass>(jsonString);
```
## Using optional properties
If your model assume to have fields which could be filled or not you can always use the `[Optional]` attribute. In this case if your json string is not contain the field, exception won't be threw.
```cs
public ModelClass {
//....
 [Optional]
 public string OptionalProperty { get; set; }
//....
}
```
## Types of Exceptions
- **JsonStringEmptyException** - Throws when json string null or empty
- **IncorrectJsonFormatException** - Throws when json string has incorrect format
- **RequaredFieldEmptyException** - Throws when json string miss to fill any field. The Field name stores into the exception.

## For Devops
You will see the `The field "{FieldName}" empty in a json file.` message in an exeption. It should help you make a trouble shooting.

[Nuget Package](https://www.nuget.org/packages/Lykke.Pkg.SettingsReader/)

