using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models
{
    class TestOptionAttrModel : TestModel
    {
        [Optional]
        public string Test4 { get; set; }

        [Optional]
        public SubTestModel SubObjectOptional { get; set; }
    }
}
