using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.SettingsReader.Test.Models
{
    class TestModel: SubTestModel
    {
        public SubTestModel SubObject { get; set; }

        public IEnumerable<SubTestModel> SubArray { get; set; }
    }
}
