using System.Collections.Generic;

namespace Lykke.SettingsReader.Test.Models
{
    class TestModel : SubTestModel
    {
        public SubTestModel SubObject { get; set; }

        public double TestDouble { get; set; }

        public SubTestModel[] SubArray { get; set; }

        public IEnumerable<SubTestModel> SubArrayGen { get; set; }

        public double SetOnlyProperty
        {
            set
            {
                // do some work
            }
        }

    }
}
