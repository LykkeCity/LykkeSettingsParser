using System;
using System.Linq;
using Lykke.SettingsReader.Exceptions;
using Lykke.SettingsReader.Test.Models;
using Xunit;

namespace Lykke.SettingsReader.Test
{
    public class SettingsProcessorTest
    {
        private readonly string _jsonTest = @"{""test1"":""testString"",""test2"":2,""test3"":""2017-05-01 22:13:45"", ""subObject"":{""test1"":""testString1"",""test2"":21,""test3"":""2017-05-02 22:13:45""},
""subArray"":[{""test1"":""testString2"",""test2"":22,""test3"":""2017-05-03 22:13:45""},
{""test1"":""testString3"",""test2"":23,""test3"":""2017-05-04 22:13:45""},
{""test1"":""testString4"",""test2"":24,""test3"":""2017-05-05 22:13:45""},
{""test1"":""testString5"",""test2"":25,""test3"":""2017-05-06 22:13:45""}]}";


        [Fact]
        public void EmptyJson()
        {
            Assert.Throws<JsonStringEmptyException>(() => SettingsProcessor.Process<TestModel>(string.Empty));
        }

        [Fact]
        public void IncorrectJson()
        {
            Assert.Throws<IncorrectJsonFormatException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Substring(10)));
        }

        [Fact]
        public void FieldMissJson()
        {
            var ex = Assert.Throws<RequaredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":2,", String.Empty)));
            Assert.Equal(ex.FieldName, "Test2");
        }

        [Fact]
        public void SubFieldMissJson()
        {
            var ex = Assert.Throws<RequaredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":21,", String.Empty)));
            Assert.Equal(ex.FieldName, "SubArray.Test2");
        }

        [Fact]
        public void SubFieldArrayMissJson()
        {
            var ex = Assert.Throws<RequaredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":24,", String.Empty)));
            Assert.Equal(ex.FieldName, "SubArray.Test2");
        }

        [Fact]
        public void OkJson()
        {
            var model = SettingsProcessor.Process<TestModel>(_jsonTest);
            CheckModel(model);
        }

        [Fact]
        public void OkWithOptionalJson()
        {
            var model = SettingsProcessor.Process<TestOptionAttrModel>(_jsonTest);
            CheckModel(model);
            Assert.Null(model.Test4);
            Assert.Null(model.SubObjectOptional);
        }

        private void CheckModel(TestModel model)
        {
            Assert.Equal(model.Test1, "testString");
            Assert.Equal(model.Test2, 2);
            Assert.Equal(model.Test3, new DateTime(2017, 05, 01, 22, 13, 45));
            Assert.Equal(model.SubObject.Test1, "testString1");
            Assert.Equal(model.SubObject.Test2, 21);
            Assert.Equal(model.SubObject.Test3, new DateTime(2017, 05, 02, 22, 13, 45));
            var lst = model.SubArray.ToList();

            Assert.Equal(lst[0].Test1, "testString2");
            Assert.Equal(lst[0].Test2, 22);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 03, 22, 13, 45));

            Assert.Equal(lst[0].Test1, "testString3");
            Assert.Equal(lst[0].Test2, 23);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 04, 22, 13, 45));

            Assert.Equal(lst[0].Test1, "testString4");
            Assert.Equal(lst[0].Test2, 24);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 05, 22, 13, 45));

            Assert.Equal(lst[0].Test1, "testString5");
            Assert.Equal(lst[0].Test2, 25);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 06, 22, 13, 45));
        }
    }
}
