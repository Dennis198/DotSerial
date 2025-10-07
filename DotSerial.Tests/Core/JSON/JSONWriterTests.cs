using DotSerial.Core.General;
using DotSerial.Core.JSON;

namespace DotSerial.Tests.Core.JSON
{
    public class JSONWriterTests
    {
        [Fact]
        public void Convert_ExampleClass()
        {
            var tmp = ExampleClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, 0);
            string result = JSONWriter.Convert(node);
            Assert.Equal("{\r\n  \"0\": {\r\n    \"-1\": \"Class\",\r\n    \"0\": \"1\",\r\n    \"1\": \"42\",\r\n    \"2\": \"Hello DotSerial!\"\r\n  }\r\n}\r\n", result);
        }

        [Fact]
        public void Convert_EmptyClass()
        {
            var tmp = new EmptyClass();
            var node = DSSerialize.Serialize(tmp, 0);
            string result = JSONWriter.Convert(node);
            Assert.Equal("{\r\n  \"0\": {\r\n    \"-1\": \"Class\"\r\n  }\r\n}\r\n", result);
        }

        [Fact]
        public void Convert_PrimitiveClass()
        {
            var tmp = PrimitiveClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, 0);
            string result = JSONWriter.Convert(node);
            Assert.Equal("{\r\n  \"0\": {\r\n    \"-1\": \"Class\",\r\n" +
                "    \"0\": \"1\",\r\n    \"1\": \"42\",\r\n" +
                "    \"2\": \"41\",\r\n    \"3\": \"d\",\r\n" +
                "    \"4\": \"40\",\r\n    \"5\": \"39.9\",\r\n" +
                "    \"6\": \"38.8\",\r\n    \"7\": \"37\",\r\n" +
                "    \"8\": \"36\",\r\n    \"9\": \"35\",\r\n" +
                "    \"10\": \"34\",\r\n    \"11\": \"33\",\r\n" +
                "    \"12\": \"32\",\r\n    \"13\": \"31\",\r\n  " +
                "  \"14\": \"30\",\r\n    \"15\": \"HelloWorld\",\r\n " +
                "   \"16\": \"1\"\r\n  }\r\n}\r\n", result);
        }

        [Fact]
        public void Convert_PrimitiveClassIEnumarable()
        {
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, 0);
            string result = JSONWriter.Convert(node);
            Assert.Equal("{\r\n  \"0\": {\r\n    \"-1\": \"Class\",\r\n" +
                "    \"0\": {\r\n      \"-1\": \"List\",\r\n      \"0\":" +
                " [\"1\", \"1\", \"0\", \"1\", \"0\"]\r\n    },\r\n    \"1\":" +
                " {\r\n      \"-1\": \"List\",\r\n      \"1\": [\"0\", \"1\"," +
                " \"0\", \"1\", \"0\"]\r\n    },\r\n    \"2\": {\r\n      \"-1\":" +
                " \"List\",\r\n      \"2\": [\"1\", \"2\", \"3\", \"4\"]\r\n    },\r\n" +
                "    \"3\": {\r\n      \"-1\": \"List\",\r\n      \"3\": [\"4\", \"3\"," +
                " \"2\", \"1\"]\r\n    },\r\n    \"4\": {\r\n      \"-1\":" +
                " \"List\",\r\n      \"4\": [\"1\", \"2\", \"3\"]\r\n    },\r\n  " +
                "  \"5\": {\r\n      \"-1\": \"List\",\r\n      \"5\": [\"3\"," +
                " \"2\", \"1\"]\r\n    },\r\n    \"6\": {\r\n      \"-1\":" +
                " \"List\",\r\n      \"6\": [\"b\", \"x\", \"g\"]\r\n    },\r\n  " +
                "  \"7\": {\r\n      \"-1\": \"List\",\r\n      \"7\": [\"a\", " +
                "\"1\", \"!\"]\r\n    },\r\n    \"8\": {\r\n      \"-1\": \"List\",\r\n " +
                "     \"8\": [\"-44\", \"55\", \"66\"]\r\n    },\r\n    \"9\": {\r\n " +
                "     \"-1\": \"List\",\r\n      \"9\": [\"88\", \"99\", \"111\"]\r\n " +
                "   },\r\n    \"10\": {\r\n      \"-1\": \"List\",\r\n   " +
                "   \"10\": [\"44.5\", \"33\", \"33.2\", \"-888.9\"]\r\n    },\r\n  " +
                "  \"11\": {\r\n      \"-1\": \"List\",\r\n      \"11\": [\"44.5\", " +
                "\"33\", \"33.2\", \"-888.9\"]\r\n    },\r\n    \"12\": {\r\n    " +
                "  \"-1\": \"List\",\r\n      \"12\": [\"4.5\", \"33\", \"33.266\"," +
                " \"-88.9\"]\r\n    },\r\n    \"13\": {\r\n      \"-1\": \"List\",\r\n " +
                "     \"13\": [\"44.5\", \"33\", \"323.2\", \"-888.9\"]\r\n    },\r\n  " +
                "  \"14\": {\r\n      \"-1\": \"List\",\r\n      \"14\": [\"-446\"," +
                " \"55\", \"66\"]\r\n    },\r\n    \"15\": {\r\n      \"-1\": \"List\"" +
                ",\r\n      \"15\": [\"88\", \"996\", \"111\"]\r\n    },\r\n    \"16\":" +
                " {\r\n      \"-1\": \"List\",\r\n      \"16\": [\"446\", \"55\", " +
                "\"66\"]\r\n    },\r\n    \"17\": {\r\n      \"-1\": \"List\",\r\n   " +
                "   \"17\": [\"88\", \"996\", \"111\"]\r\n    },\r\n    \"18\": {\r\n " +
                "     \"-1\": \"List\",\r\n      \"18\": [\"-4426\", \"55\", \"66\"]\r\n" +
                "    },\r\n    \"19\": {\r\n      \"-1\": \"List\",\r\n    " +
                "  \"19\": [\"882\", \"9196\", \"111\"]\r\n    },\r\n   " +
                " \"20\": {\r\n      \"-1\": \"List\",\r\n      \"20\": [\"446\"," +
                " \"545\", \"66\"]\r\n    },\r\n    \"21\": {\r\n      \"-1\":" +
                " \"List\",\r\n      \"21\": [\"88\", \"996\", \"1141\"]\r\n    },\r\n " +
                "   \"22\": {\r\n      \"-1\": \"List\",\r\n      \"22\": [\"-446\", " +
                "\"55\", \"66\", \"999999999\"]\r\n    },\r\n    \"23\": {\r\n   " +
                "   \"-1\": \"List\",\r\n      \"23\": [\"88\", \"996\", \"111\"," +
                " \"90909090909\"]\r\n    },\r\n    \"24\": {\r\n      \"-1\":" +
                " \"List\",\r\n      \"24\": [\"446\", \"55\", \"66\", \"999999999\"]" +
                "\r\n    },\r\n    \"25\": {\r\n      \"-1\": \"List\",\r\n      \"25\":" +
                " [\"88\", \"996\", \"111\", \"90909090909\"]\r\n    },\r\n    \"26\":" +
                " {\r\n      \"-1\": \"List\",\r\n      \"26\": [\"-446\", \"55\"," +
                " \"66\"]\r\n    },\r\n    \"27\": {\r\n      \"-1\": \"List\",\r\n   " +
                "   \"27\": [\"88\", \"996\", \"111\"]\r\n    },\r\n    \"28\": {\r\n " +
                "     \"-1\": \"List\",\r\n      \"28\": [\"446\", \"55\", \"66\"]\r\n  " +
                "  },\r\n    \"29\": {\r\n      \"-1\": \"List\",\r\n      \"29\": " +
                "[\"88\", \"996\", \"111\"]\r\n    },\r\n    \"30\": {\r\n      \"-1\":" +
                " \"List\",\r\n      \"30\": [\"HelloWorld\", \"xxx\", \"\"]\r\n   " +
                " },\r\n    \"31\": {\r\n      \"-1\": \"List\",\r\n      \"31\":" +
                " [\"yyy\", \"hsdasd\", \"\"]\r\n    },\r\n    \"32\": {\r\n     " +
                " \"-1\": \"List\",\r\n      \"32\": [\"1\", \"4\"]\r\n    },\r\n   " +
                " \"33\": {\r\n      \"-1\": \"List\",\r\n      \"33\": [\"0\"," +
                " \"-1\"]\r\n    }\r\n  }\r\n}\r\n", result);
        }

        [Fact]
        public void Convert_IEnumerableClass()
        {
            // Arrange
            var tmp = new IEnumerableClass
            {
                Array = new SimpleClass[10],
                List = [],
                Dic = []
            };
            for (int i = 0; i < 10; i++)
            {
                var d = new SimpleClass
                {
                    Boolean = true
                };

                tmp.Array[i] = d;
                tmp.List.Add(d);
                tmp.Dic.Add(i, d);
            }
            var node = DSSerialize.Serialize(tmp, 0);
            string result = JSONWriter.Convert(node);
            Assert.Equal("{\r\n  \"0\": {\r\n    \"-1\": \"Class\",\r\n" +
                "    \"0\": \"1\",\r\n    \"1\": \"42\",\r\n" +
                "    \"2\": \"41\",\r\n    \"3\": \"d\",\r\n" +
                "    \"4\": \"40\",\r\n    \"5\": \"39.9\",\r\n" +
                "    \"6\": \"38.8\",\r\n    \"7\": \"37\",\r\n" +
                "    \"8\": \"36\",\r\n    \"9\": \"35\",\r\n" +
                "    \"10\": \"34\",\r\n    \"11\": \"33\",\r\n" +
                "    \"12\": \"32\",\r\n    \"13\": \"31\",\r\n  " +
                "  \"14\": \"30\",\r\n    \"15\": \"HelloWorld\",\r\n " +
                "   \"16\": \"1\"\r\n  }\r\n}\r\n", result);
        }

        [Fact]
        public void Convert_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, 0);
            string result = JSONWriter.Convert(node);
            Assert.Equal("{\r\n  \"0\": {\r\n    \"-1\": \"Class\",\r\n" +
                "    \"0\": \"1\",\r\n    \"1\": \"42\",\r\n" +
                "    \"2\": \"41\",\r\n    \"3\": \"d\",\r\n" +
                "    \"4\": \"40\",\r\n    \"5\": \"39.9\",\r\n" +
                "    \"6\": \"38.8\",\r\n    \"7\": \"37\",\r\n" +
                "    \"8\": \"36\",\r\n    \"9\": \"35\",\r\n" +
                "    \"10\": \"34\",\r\n    \"11\": \"33\",\r\n" +
                "    \"12\": \"32\",\r\n    \"13\": \"31\",\r\n  " +
                "  \"14\": \"30\",\r\n    \"15\": \"HelloWorld\",\r\n " +
                "   \"16\": \"1\"\r\n  }\r\n}\r\n", result);
        }
    }
}
