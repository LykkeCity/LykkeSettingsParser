namespace Lykke.SettingsReader
{
    public interface ISettingsFieldChecker
    {
        CheckFieldResult CheckField(object model, string propertyName, string value);
    }
}
