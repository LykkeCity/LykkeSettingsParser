using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.SettingsReader.Exceptions;
using Lykke.SettingsReader.Test.Models;
using Lykke.SettingsReader.Test.Models.CheckAttributes;
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
            Assert.Equal("Test2", ex.FieldName);
        }

        [Fact]
        public void SubFieldMissJson()
        {
            var ex = Assert.Throws<RequiredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":21,", String.Empty)));
            Assert.Equal("SubObject.Test2", ex.FieldName);
        }

        [Fact]
        public void SubFieldArrayMissJson()
        {
            var ex = Assert.Throws<RequiredFieldEmptyException>(() => SettingsProcessor.Process<TestModel>(_jsonTest.Replace(@"""test2"":24,", String.Empty)));
            Assert.Equal("SubArray.2.Test2", ex.FieldName);
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
            Assert.Equal("testString", model.Test1);
            Assert.Equal(2, model.Test2);
            Assert.Equal(model.Test3, new DateTime(2017, 05, 01, 22, 13, 45));
            Assert.Equal("testString1", model.SubObject.Test1);
            Assert.Equal(21, model.SubObject.Test2);
            Assert.Equal(model.SubObject.Test3, new DateTime(2017, 05, 02, 22, 13, 45));
            Assert.Equal(0.2, model.TestDouble);
            var lst = model.SubArray.ToList();

            Assert.Equal("testString2", lst[0].Test1);
            Assert.Equal(22, lst[0].Test2);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 03, 22, 13, 45));

            Assert.Equal("testString3", lst[1].Test1);
            Assert.Equal(23, lst[1].Test2);
            Assert.Equal(lst[1].Test3, new DateTime(2017, 05, 04, 22, 13, 45));

            Assert.Equal("testString4", lst[2].Test1);
            Assert.Equal(24, lst[2].Test2);
            Assert.Equal(lst[2].Test3, new DateTime(2017, 05, 05, 22, 13, 45));

            Assert.Equal("testString5", lst[3].Test1);
            Assert.Equal(25, lst[3].Test2);
            Assert.Equal(lst[3].Test3, new DateTime(2017, 05, 06, 22, 13, 45));

            lst = model.SubArrayGen.ToList();

            Assert.Equal("testString6", lst[0].Test1);
            Assert.Equal(26, lst[0].Test2);
            Assert.Equal(lst[0].Test3, new DateTime(2017, 05, 07, 22, 13, 45));

            Assert.Equal("testString7", lst[1].Test1);
            Assert.Equal(27, lst[1].Test2);
            Assert.Equal(lst[1].Test3, new DateTime(2017, 05, 08, 22, 13, 45));

            Assert.Equal("testString8", lst[2].Test1);
            Assert.Equal(28, lst[2].Test2);
            Assert.Equal(lst[2].Test3, new DateTime(2017, 05, 09, 22, 13, 45));

            Assert.Equal("testString9", lst[3].Test1);
            Assert.Equal(29, lst[3].Test2);
            Assert.Equal(lst[3].Test3, new DateTime(2017, 05, 10, 22, 13, 45));
        }

        [Fact]
        public void Test_NegativeNumbersAsStrings_IsOk()
        {
            string json = "{'Int': '-1234', " +
                          "'Double': '-10.4', " +
                          "'Float': '-10.4', " +
                          "'Decimal': '-10.2'}";

            var model = SettingsProcessor.Process<TestConvert>(json);
            Assert.Equal(-1234, model.Int);
            Assert.Equal(-10.4, Math.Round(model.Double, 2));
            Assert.Equal(-10.4, Math.Round(model.Float, 2));
            Assert.Equal((decimal)-10.2, model.Decimal);
        }
        
        [Fact]
        public void Test_NegativeNumbers_IsOk()
        {
            string json = "{'Int': -1234, " +
                          "'Double': -10.4, " +
                          "'Float': -10.4, " +
                          "'Decimal': -10.2}";

            var model = SettingsProcessor.Process<TestConvert>(json);
            Assert.Equal(-1234, model.Int);
            Assert.Equal(-10.4, Math.Round(model.Double, 2));
            Assert.Equal(-10.4, Math.Round(model.Float, 2));
            Assert.Equal((decimal)-10.2, model.Decimal);
        }

        [Fact]
        public void HttpCheckAttribute_IsOk()
        {
            var exception = Record.Exception(() =>
            {
                SettingsProcessor.Process<TestHttpCheckModel>(
                    "{'Service': {'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}, 'Url': 'http://assets.lykke-service.svc.cluster.local/', 'Port':5672, 'Num': 1234}");
            });

            Assert.Null(exception);
        }

        [Fact]
        public void HttpCheckAttribute_IsArrayOk()
        {
            var exception = Record.Exception(() =>
            {
                SettingsProcessor.Process<TestHttpCheckArrayModel>("{'Services': [{'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}], " +
                                                                   "'List': [{'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}], " +
                                                                   "'IList': [{'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}], " +
                                                                   "'RoList': [{'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}], " +
                                                                   "'RoCollection': [{'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}], " +
                                                                   "'Enumerable': [{'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}]}");
            });

            Assert.Null(exception);
        }

        [Fact]
        public void HttpCheckAttribute_IsListOk()
        {
            var exception = Record.Exception(() =>
            {
                SettingsProcessor.Process<TestHttpCheckListModel>("{'Services': ['http://assets.lykke-service.svc.cluster.local']," +
                                                                  "'List': ['http://assets.lykke-service.svc.cluster.local']," +
                                                                  "'IList': ['http://assets.lykke-service.svc.cluster.local']," +
                                                                  "'RoList': ['http://assets.lykke-service.svc.cluster.local']," +
                                                                  "'RoCollection': ['http://assets.lykke-service.svc.cluster.local']," +
                                                                  "'Enumerable': ['http://assets.lykke-service.svc.cluster.local']}");
            });

            Assert.Null(exception);
        }
        
        [Fact]
        public void HttpCheckAttribute_IsDictionaryOk()
        {
            var exception = Record.Exception(() =>
            {
                SettingsProcessor.Process<TestHttpCheckDictioinaryModel>("{'Services': {'first': {'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}}," +
                                                                         "'IDict': {'first': {'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}}," +
                                                                         "'RoDict': {'first': {'ServiceUrl': 'http://assets.lykke-service.svc.cluster.local'}}}");
            });

            Assert.Null(exception);
        }

        [Fact]
        public void HttpCheckAttribute_IsInvalidUrl()
        {
            var exception = Record.Exception(() => 
                SettingsProcessor.Process<TestHttpCheckModel>("{'Service': {'ServiceUrl': 'not_url_at_all'}, 'Url': 'http://assets.lykke-service.svc.cluster.local/', 'Port':5672, 'Num': 1234}")
            );

            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Check of the 'ServiceUrl' field value [not_url_at_all] is failed: Invalid url", exception.Message);
        }

        [Fact]
        public void TcpCheckAttribute_IsArrayOk()
        {
            var checkList = new List<(string, string)>
            {
                ("Endpoints", "127.0.0.1:5672"),
                ("List", "127.0.0.1:5672"),
                ("IList", "127.0.0.1:5672"),
                ("RoList", "127.0.0.1:5672"),
                ("RoCollection", "127.0.0.1:5672"),
                ("Enumerable", "127.0.0.1:5672")
            };
            foreach (var pair in checkList)
            {
                var exception = Record.Exception(() =>
                {
                    SettingsProcessor.Process<TestTcpCheckArrayModel>($"{{'{pair.Item1}': [{{'HostPort': '{pair.Item2}'}}] }}");
                });

                Assert.NotNull(exception);
                Assert.Equal($"Check of the 'HostPort' field value [{pair.Item2}] is failed: Dependency is unavailable on tcp://{pair.Item2}", exception.Message);
            }
        }

        [Fact]
        public void TcpCheckAttribute_IsListOk()
        {
            var checkList = new List<(string, string)>
            {
                ("Hosts", "127.0.0.1:5672"),
                ("List", "127.0.0.1:5672"),
                ("IList", "127.0.0.1:5672"),
                ("RoList", "127.0.0.1:5672"),
                ("RoCollection", "127.0.0.1:5672"),
                ("Enumerable", "127.0.0.1:5672")
            };
            foreach (var pair in checkList)
            {
                var exception = Record.Exception(() =>
                {
                    SettingsProcessor.Process<TestTcpCheckListModel>($"{{'{pair.Item1}': ['{pair.Item2}'] }}");
                });

                Assert.NotNull(exception);
                Assert.Equal($"Check of the '{pair.Item1}' field value [{pair.Item2}] is failed: Dependency is unavailable on tcp://{pair.Item2}", exception.Message);
            }
        }

        [Fact]
        public void TcpCheckAttribute_IsDictionaryOk()
        {
            var exception = Record.Exception(() =>
            {
                SettingsProcessor.Process<TestTcpCheckDictionaryModel>("{'Endpoints': {'first': {'HostPort': '127.0.0.1:5672'}}," +
                                                                       "'IDict': {'first': {'HostPort': '127.0.0.1:5672'}}," +
                                                                       "'RoDict': {'first': {'HostPort': '127.0.0.1:5672'}}}");
            });

            Assert.Null(exception);
        }

        [Fact]
        public void TcpCheckAttribute_IsInvalidPort()
        {
            var exception = Record.Exception(() => 
                    SettingsProcessor.Process<TestTcpCheckModel>("{'HostInfo': {'HostPort': '127.0.0.1:zzz'}, 'Host': '127.0.0.1', 'Port': 5672, 'Server': '127.0.0.1'}")
            );
        
            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Check of the 'HostPort' field value [127.0.0.1:zzz] is failed: Invalid port", exception.Message);
        }

        [Fact]
        public void TcpCheckAttribute_IsInvalidPortValue()
        {
            const string host = "127.0.0.1";
            const int port = 5672;

            var exception1 = Record.Exception(() => 
                SettingsProcessor.Process<TestTcpCheckModel>($"{{'HostInfo': {{'HostPort': '{host}:{port}'}} }}")
            );

            Assert.NotNull(exception1);
            Assert.IsType<CheckFieldException>(exception1);
            Assert.Equal($"Check of the 'HostPort' field value [{host}:{port}] is failed: Dependency is unavailable on tcp://{host}:{port}", exception1.Message);

            var exception2 = Record.Exception(() =>
                SettingsProcessor.Process<TestTcpCheckModel>($"{{'Host': '{host}', 'Port': 'not a port'}}")
            );

            Assert.NotNull(exception2);
            Assert.IsType<CheckFieldException>(exception2);
            Assert.Equal($"Check of the 'Host' field value [{host}] is failed: Invalid port value in property 'Port'", exception2.Message);

            var exception3 = Record.Exception(() =>
                SettingsProcessor.Process<TestTcpCheckModel>($"{{'Host': '{host}', 'Port': '{port}'}}")
            );

            Assert.NotNull(exception3);
            Assert.Equal($"Check of the 'Host' field value [{host}] is failed: Dependency is unavailable on tcp://{host}:{port}", exception3.Message);

            var exception4 = Record.Exception(() =>
                SettingsProcessor.Process<TestTcpCheckModel>($"{{'Server': '{host}'}}")
            );

            Assert.NotNull(exception4);
            Assert.IsType<CheckFieldException>(exception4);
            Assert.Equal($"Check of the 'Server' field value [{host}] is failed: Dependency is unavailable on tcp://{host}:{port}", exception4.Message);
        }

        [Fact]
        public void TcpCheckAttribute_WrongPortProperty()
        {
            var exception = Record.Exception(() => 
                SettingsProcessor.Process<WrongTestTcpCheckModel>("{'Host': '127.0.0.1', 'Port': '5672'}")
            );

            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Check of the 'Host' field value [127.0.0.1] is failed: Property 'ServicePort' not found", exception.Message);
        }

        [Fact]
        public void AmqpCheckAttribute_IsOk()
        {
            string json = "{'ConnStr': 'amqp://guest:guest@localhost:5672', 'Rabbit': {'ConnString': 'amqp://lykke.user:123qwe123qwe123@rabbit-registration.lykke-service.svc.cluster.local:5672'}}";
            var exception = Record.Exception(() =>
            {
                SettingsProcessor.Process<TestAmqpCheckModel>(json);
            });

            Assert.Null(exception);
        }

        [Fact]
        public void AmqpCheckAttribute_IsArrayOk()
        {
            var checkList = new List<(string, string)>
            {
                ("Rabbits", "amqp://guest:guest@localhost:5672"),
                ("List", "amqp://guest:guest@localhost:5672"),
                ("IList", "amqp://guest:guest@localhost:5672"),
                ("RoList", "amqp://guest:guest@localhost:5672"),
                ("RoCollection", "amqp://guest:guest@localhost:5672"),
                ("Enumerable", "amqp://guest:guest@localhost:5672")
            };
            foreach (var pair in checkList)
            {
                var exception = Record.Exception(() =>
                {
                    SettingsProcessor.Process<TestAmqpCheckArrayModel>($"{{'{pair.Item1}': [{{'ConnString': '{pair.Item2}'}}] }}");
                });

                Assert.NotNull(exception);
                Assert.Equal($"Check of the 'ConnString' field value [{pair.Item2}] is failed: Dependency is unavailable on amqp://localhost:5672", exception.Message);
            }
        }

        [Fact]
        public void AmqpCheckAttribute_IsListOk()
        {
            var checkList = new List<(string, string)>
            {
                ("Rabbits", "amqp://guest:guest@localhost:5672"),
                ("List", "amqp://guest:guest@localhost:5672"),
                ("IList", "amqp://guest:guest@localhost:5672"),
                ("RoList", "amqp://guest:guest@localhost:5672"),
                ("RoCollection", "amqp://guest:guest@localhost:5672"),
                ("Enumerable", "amqp://guest:guest@localhost:5672")
            };
            foreach (var pair in checkList)
            {
                var exception = Record.Exception(() =>
                {
                    SettingsProcessor.Process<TestAmqpCheckListModel>($"{{'{pair.Item1}': ['{pair.Item2}'] }}");
                });

                Assert.NotNull(exception);
                Assert.Equal($"Check of the '{pair.Item1}' field value [{pair.Item2}] is failed: Dependency is unavailable on amqp://localhost:5672", exception.Message);
            }
        }

        [Fact]
        public void AmqpCheckAttribute_IsDictionaryOk()
        {
            var exception = Record.Exception(() =>
            {
                SettingsProcessor.Process<TestAmqpCheckDictionaryModel>("{'Rabbits': {'first': {'ConnString': 'amqp://guest:guest@localhost:5672'}}," +
                                                                        "'IDict': {'first': {'ConnString': 'amqp://guest:guest@localhost:5672'}}," +
                                                                        "'RoDict': {'first': {'ConnString': 'amqp://guest:guest@localhost:5672'}}}");
            });

            Assert.Null(exception);
        }

        [Fact]
        public void AmqpCheckAttribute_IsInvalidPort()
        {
            var exception = Record.Exception(() => 
                    SettingsProcessor.Process<TestAmqpCheckModel>("{'ConnStr': 'amqp://guest:guest@localhost:5672', 'Rabbit': {'ConnString': 'amqp://lykke.user:123qwe123qwe123@rabbit-registration.lykke-service.svc.cluster.local:zzz'}}")
            );

            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Check of the 'ConnString' field value [amqp://lykke.user:123qwe123qwe123@rabbit-registration.lykke-service.svc.cluster.local:zzz] is failed: Invalid port", exception.Message);
        }

        [Fact]
        public void AmqpCheckAttribute_IsInvalidConnectionString()
        {
            var exception = Record.Exception(() => 
                SettingsProcessor.Process<TestAmqpCheckModel>("{'ConnStr': 'amqp://guest:guest@localhost:5672', 'Rabbit': {'ConnString': 'rabbit-registration.lykke-service.svc.cluster.local:5672'}}")
            );
        
            Assert.NotNull(exception);
            Assert.IsType<CheckFieldException>(exception);
            Assert.Equal("Check of the 'ConnString' field value [rabbit-registration.lykke-service.svc.cluster.local:5672] is failed: Invalid amqp connection string", exception.Message);
        }
    }
}
