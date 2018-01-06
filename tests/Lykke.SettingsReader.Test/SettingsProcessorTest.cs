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
{""test1"":""testString5"",""test2"":25,""test3"":""2017-05-06 22:13:45""}],
  ""subArrayGen"":[{""test1"":""testString6"",""test2"":26,""test3"":""2017-05-07 22:13:45""},
{""test1"":""testString7"",""test2"":27,""test3"":""2017-05-08 22:13:45""},
{""test1"":""testString8"",""test2"":28,""test3"":""2017-05-09 22:13:45""},
{""test1"":""testString9"",""test2"":29,""test3"":""2017-05-10 22:13:45""}],
""TestDouble"" : 0.2
    }";


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
            var ex = Assert.Throws<RequiredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":2,", String.Empty)));
            Assert.Equal(ex.FieldName, "Test2");
        }

        [Fact]
        public void SubFieldMissJson()
        {
            var ex = Assert.Throws<RequiredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":21,", String.Empty)));
            Assert.Equal(ex.FieldName, "SubObject.Test2");
        }

        [Fact]
        public void SubFieldArrayMissJson()
        {
            var ex = Assert.Throws<RequiredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":24,", String.Empty)));
            Assert.Equal(ex.FieldName, "SubArray.2.Test2");
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
            Assert.Equal(model.TestDouble, 0.2);
            var lst = model.SubArray.ToList();

            Assert.Equal(lst[0].Test1, "testString2");
            Assert.Equal(lst[0].Test2, 22);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 03, 22, 13, 45));

            Assert.Equal(lst[1].Test1, "testString3");
            Assert.Equal(lst[1].Test2, 23);
            Assert.Equal(lst[1].Test3, new DateTime(2017, 05, 04, 22, 13, 45));

            Assert.Equal(lst[2].Test1, "testString4");
            Assert.Equal(lst[2].Test2, 24);
            Assert.Equal(lst[2].Test3, new DateTime(2017, 05, 05, 22, 13, 45));

            Assert.Equal(lst[3].Test1, "testString5");
            Assert.Equal(lst[3].Test2, 25);
            Assert.Equal(lst[3].Test3, new DateTime(2017, 05, 06, 22, 13, 45));

            lst = model.SubArrayGen.ToList();

            Assert.Equal(lst[0].Test1, "testString6");
            Assert.Equal(lst[0].Test2, 26);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 07, 22, 13, 45));

            Assert.Equal(lst[1].Test1, "testString7");
            Assert.Equal(lst[1].Test2, 27);
            Assert.Equal(lst[1].Test3, new DateTime(2017, 05, 08, 22, 13, 45));

            Assert.Equal(lst[2].Test1, "testString8");
            Assert.Equal(lst[2].Test2, 28);
            Assert.Equal(lst[2].Test3, new DateTime(2017, 05, 09, 22, 13, 45));

            Assert.Equal(lst[3].Test1, "testString9");
            Assert.Equal(lst[3].Test2, 29);
            Assert.Equal(lst[3].Test3, new DateTime(2017, 05, 10, 22, 13, 45));

          
        }

        [Fact]
        public void HttpCheckAttribute_IsOk()
        {
            SettingsProcessor.Process<TestHttpCheckModel>("{'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local/', 'Port':5672, 'Num': 1234}");
        }
        
        [Fact]
        public void HttpCheckAttribute_Error()
        {
            var exception = Record.Exception(() => 
                SettingsProcessor.Process<TestHttpCheckModel>("{'ServiceUrl': 'not_url_at_all', 'Port':5672, 'Num': 1234}")
            );

            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Wrong url", exception.Message);
        }
        
        [Fact]
        public void TcpCheckAttribute_IsOk()
        {
            SettingsProcessor.Process<TestTcpCheckModel>("{'HostPort': '127.0.0.1:5672', 'Host': '127.0.0.1', 'Port': 5672, 'Server': '127.0.0.1'}");
        }
        
        [Fact]
        public void TcpCheckAttribute_IsWrongPortValue()
        {
            var exception = Record.Exception(() => 
                SettingsProcessor.Process<TestTcpCheckModel>("{'HostPort': '127.0.0.1:5672', 'Host': '127.0.0.1', 'Port': 'not a port', 'Server': '127.0.0.1'}")
            );

            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Wrong port value in property 'Port'", exception.Message);
        }
        
        [Fact]
        public void TcpCheckAttribute_WrongPortProperty()
        {
            var exception = Record.Exception(() => 
                SettingsProcessor.Process<WrongTestTcpCheckModel>("{'Host': '127.0.0.1', 'Port': '5672'}")
            );
        
            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Property 'ServicePort' not found", exception.Message);
        }
        
        [Fact]
        public void AmqpCheckAttribute_IsOk()
        {
            SettingsProcessor.Process<TestAmqpCheckModel>("{'Rabbit': 'amqp://lykke.user:123qwe123qwe123@rabbit-registration.lykke-service.svc.cluster.local:5672'}");
        }
        
        [Fact]
        public void AmqpCheckAttribute_IsError()
        {
            var exception = Record.Exception(() => 
                SettingsProcessor.Process<TestAmqpCheckModel>("{'Rabbit': 'rabbit-registration.lykke-service.svc.cluster.local:5672'}")
            );
        
            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Wrong amqp connection string", exception.Message);
        }
    }
}
