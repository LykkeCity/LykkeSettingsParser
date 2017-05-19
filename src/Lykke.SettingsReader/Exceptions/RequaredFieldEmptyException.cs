using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.SettingsReader.Exceptions
{
    public class RequaredFieldEmptyException : SettingsReaderException
    {
        public string FieldName { get; set; }
    }
}
