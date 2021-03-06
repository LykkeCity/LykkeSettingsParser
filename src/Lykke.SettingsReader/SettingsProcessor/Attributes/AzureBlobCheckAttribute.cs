﻿using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for azure blob connection string check
    /// </summary>
    public class AzureBlobCheckAttribute : BaseCheckAttribute
    {
        /// <inheritdoc />
        public override ISettingsFieldChecker GetChecker()
        {
            return new AzureBlobChecker();
        }
    }
}
