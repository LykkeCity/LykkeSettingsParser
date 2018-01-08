using System.Reflection;

namespace Lykke.SettingsReader
{
    public interface ISettingsFieldChecker
    {
        CheckFieldResult[] CheckField(object model, PropertyInfo property, object value);
    }
}
