using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;

using BookLoanMicroservices.Helpers;

namespace BookLoanMicroservices.Helpers
{
    public class JsonSerializerAdapter : ISerializer
    {
        private JsonSerializer serializer;

        public JsonSerializerAdapter(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Serialize(Stream stream, object graph)
        {
            var writer = new JsonTextWriter(new StreamWriter(stream));
            this.serializer.Serialize(writer, graph);
            // We don’t close the stream as it’s owned by the message.
            writer.Flush();
        }

        public object Deserialize(Stream stream)
        {
            var reader = new JsonTextReader(new StreamReader(stream));
            return this.serializer.Deserialize(reader);
        }
    }


}
