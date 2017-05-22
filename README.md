# LykkeSettingsParser
The library allows you to parse JSON string in to object. If any of field won't be filled by json structure an Exception will throw.
## How to use
There is only one static generic function in a library
```
 var model = SettingsProcessor.Process<ModelClass>(jsonString);
```
## Using optional properties
If your model assume to have fields which could be filled or not you can always use the `[Optional]` attribute. In this case if your json string is not contain the field, exception won't be threw.
```
public ModelClass {
//....
 [Optional]
 public string OptionalProperty { get; set; }
//....
}
```
## Types of Exceptions
- ul **JsonStringEmptyException** - Throws when json string null or empty
- ul **IncorrectJsonFormatException** - Throws when json string has incorrect format
- ul **RequaredFieldEmptyException** - Throws when json string miss to fill any field. The Field name stores into the exception.
```

```
