using System;

namespace Lykke.SettingsReader {

    [Obsolete("Will be deleted. Have to use IConfiguration.LoadSettings extension method.")]
    public class SettingsReader {
        public static T ReadGeneralSettings<T>(Uri url) {
            var reloadingManager = new SettingsServiceReloadingManager<T>(url.ToString());
            return reloadingManager.CurrentValue;
        }

        public static T ReadGeneralSettings<T>(string path) {
            var reloadingManager = new LocalSettingsReloadingManager<T>(path);
            return reloadingManager.CurrentValue;
        }
    }
}