# LykkeSettingsParser
The library allows you to parse JSON string in to object. If any of field won't be filled by json structure an Exception will throw.
## How to use
First of all, ```SettingsUrl``` environment variable should contains URL or path to the file which contains the settings Json.
Next, there is the only one static generic method, which you need to load the settings, pass function to return slack notification parameters
to send failed depencencies checks:
```cs
 var model = Configuration.LoadSettings<ModelClass>(settings => (settings.SlackNotifications.ConnString, settings.SlackNotifications.QueueName, "SenderName"));
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

## Dependencies check on startup
Attributes **HttpCheck**, **TcpCheck**, **AmqpCheck** can be used to check connections to the services during startup.
HttpCheck attribute is used to make a GET call. TcpCheck establishes a TCP connection for the check. AmqpCheck checks connection to rabbit mq. 

**HttpCheck example**:
```csharp
[HttpCheck("/api/isalive")]
public string ServiceUrl { get; set; } // http://some.service
```

**TcpCheck examples**:
```csharp
[TcpCheck]
public string Host{ get; set; } // 127.0.0.1:8888

[TcpCheck("Port")]
public string Host { get; set; } // 127.0.0.1
public int Port{ get; set; } // 8888

[TcpCheck(8888)]
public string Host { get; set; } // 127.0.0.1
```

**AmqpCheck example**:
```csharp
[AmqpCheck]
public string RabbitMq { get; set; } // amqp://user:pass@localhost:5672
```

**AzureTableCheck example**:
```csharp
[AzureTableCheck]
public string TableConnectionString { get; set; } // valid table token
```

**AzureBlobCheck example**:
```csharp
[AzureBlobCheck]
public string BlobConnectionString { get; set; } // valid blob token
```

**AzureQueueCheck example**:
```csharp
[AzureQueueCheck]
public string QueueConnectionString { get; set; } // valid queue token
```

**SqlCheck example**:
```csharp
[SqlCheck]
public string SqlConnectionString { get; set; } // valid sql connection string
```

## Types of Exceptions
- **JsonStringEmptyException** - Throws when json string null or empty
- **IncorrectJsonFormatException** - Throws when json string has incorrect format
- **RequaredFieldEmptyException** - Throws when json string miss to fill any field. The Field name stores into the exception.

## For Devops
You will see the `The field "{FieldName}" empty in a json file.` message in an exeption. It should help you make a trouble shooting.

[Nuget Package](https://www.nuget.org/packages/Lykke.SettingsReader/)

