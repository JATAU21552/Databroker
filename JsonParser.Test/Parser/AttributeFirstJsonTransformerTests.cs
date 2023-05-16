using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JsonParser.Parser;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests
{
    public class AttributeFirstJsonTransformerTests
    {

        string simpleJson = "{\"secretIdentity\": \"Unknown\"}";

        string littlecomplexjson = "{\"name\": \"Eternal Flame\", \"age\": 1000000, \"secretIdentity\": \"Unknown\", \"powers\": [\"Immortality\", \"Heat Immunity\", \"Inferno\",  \"Teleportation\", \"Interdimensional travel\" ] }";

        [Test]
        public void WhenEmptyObjectThenEmptyObject()
        {
            // Arrange
            var input = new MemoryStream(Encoding.ASCII.GetBytes("{}"));

            // Act
            var output = AttributeFirstJsonTransformer.Transform(input);
            var actual = new StreamReader(output).ReadToEnd();

            // Assert
            Assert.AreEqual("{}", actual);
        }

        [Test]
        public void ReturnSameSimpleJson()
        {
            
            var input = JObject.Parse(simpleJson);

            var Output = AttributeFirstJsonTransformer.IterateOverJsonDynamically(simpleJson);

            Assert.AreEqual(input.ToString(), Output);
        }

        [Test]
        public void TestSameNumberofChildNotes()
        {

           JObject _testinputObject = JObject.Parse(littlecomplexjson);
            
            var input  = AttributeFirstJsonTransformer.IterateOverJsonDynamically(littlecomplexjson);
            JObject _testOutputObject = JObject.Parse(input);

            Assert.AreEqual(_testinputObject.Children().Count(), _testOutputObject.Children().Count());
        }

        [Test]
        public void TestTwoChildernJsonOnePrimitiveAnotherNonPrimitive()
        {
            string twopropertyJson = "{\"powers\": [\"Million tonne punch\", \"Damage resistance\", \"Superhuman reflexes\" ],\"secretIdentity\": \"Jane Wilson\"}";
            JObject _testinputObject = JObject.Parse(twopropertyJson);

            var input = AttributeFirstJsonTransformer.IterateOverJsonDynamically(twopropertyJson);
            JObject _testOutputObject = JObject.Parse(input);

            Assert.AreEqual(_testinputObject.Children().First(), _testOutputObject.Children().Last());
        }

        [Test]
        public void TestSimpleJsonField()
        {

            var jobject = JToken.Parse(littlecomplexjson);
            var firstObject = jobject.First();
            bool isComplexFiled = AttributeFirstJsonTransformer.IsComplexType(firstObject as JProperty);

            Assert.AreEqual(isComplexFiled, false);

        }

        [Test]
        public void TestSimpleJsonFieldtoFail()
        {
            var jobject = JToken.Parse(simpleJson);
            var firstObject = jobject.First();

            bool isComplexFiled = AttributeFirstJsonTransformer.IsComplexType(firstObject as JProperty);

            Assert.AreNotEqual(isComplexFiled, true);

        }
    }
}