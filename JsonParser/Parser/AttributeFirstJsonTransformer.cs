using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonParser.Parser
{
    public static class AttributeFirstJsonTransformer
    {
        public static Stream Transform(Stream source)
        {
            return source;
        }


        public static Stream RearrangeJson(string originaljson)
        {
            var _string = IterateOverJsonDynamically(originaljson);

            return ConvertToStream(_string);
        }

        public static string IterateOverJsonDynamically(string json)
        {
           
                var jsonObjects = JToken.Parse(json);
                var ListCount = jsonObjects.Children().Count();
                var rearrangedArray = new JToken[ListCount];

                var frontpoitner = 0;
                var backpointer = ListCount - 1;
                foreach (var data in jsonObjects)
                {

                    if (IsComplexType((JProperty)data))
                    {
                        rearrangedArray[backpointer] = data;
                        backpointer--;
                    }
                    else
                    {
                        rearrangedArray[frontpoitner] = data;
                        frontpoitner++;
                    }
                }
             
                JObject _jObject = new JObject();
                foreach (var data in rearrangedArray) 
                {
                    _jObject.Add(data);
                }
                return _jObject.ToString();
            
           
        }

        public static bool IsComplexType(JProperty token)
        {
            bool isNested = false;

            if (token.Value.Type == JTokenType.Object)
            {
                isNested = true;
            }
            else if (token.Value.Type == JTokenType.Array)
            {
                isNested = true;
            }

            return isNested;
        }

        private static Stream ConvertToStream(string output)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(output);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;

          
        }
    }
}