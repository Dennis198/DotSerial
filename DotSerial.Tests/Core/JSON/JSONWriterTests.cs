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
            Assert.Equal("{\r\n  \"0\": {\r\n    \"0\": \"1\",\r\n    \"1\": \"42\",\r\n    \"2\": \"Hello DotSerial!\"\r\n  }\r\n}\r\n", result);
        }
    }
}
