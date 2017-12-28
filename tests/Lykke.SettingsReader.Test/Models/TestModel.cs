using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.SettingsReader.Test.Models
{
    class TestModel: SubTestModel
    {
        public SubTestModel SubObject { get; set; }

        public double TestDouble { get; set; }

        public SubTestModel[] SubArray { get; set; }

        public IEnumerable<SubTestModel> SubArrayGen { get; set; }

    }
}
