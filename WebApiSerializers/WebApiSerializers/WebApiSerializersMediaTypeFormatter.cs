using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace WebApiSerializers
{
    internal struct AsyncVoid
    {
    }

    public class WebApiSerializersMediaTypeFormatter : JsonMediaTypeFormatter
    {
        protected WebApiSerializersMediaTypeFormatter(JsonMediaTypeFormatter formatter) : base(formatter)
        {
        }

        public WebApiSerializersMediaTypeFormatter()
        {
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            var serializer = SerializersCache.Current.GetSerializerForClass(type);

            if (UseDataContractJsonSerializer || serializer == null)
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }


            try
            {
                Encoding effectiveEncoding = SelectCharacterEncoding(content == null ? null : content.Headers);

                var contractResolver = serializer.GetContractResolver();
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(new StreamWriter(writeStream, effectiveEncoding))
                {
                        CloseOutput = false
                    })
                {
                    if (Indent)
                    {
                        jsonTextWriter.Formatting = Newtonsoft.Json.Formatting.Indented;
                    }
                    var defaultSerializerSettings = CreateDefaultSerializerSettings();
                    defaultSerializerSettings.ContractResolver = contractResolver;

                    JsonSerializer jsonSerializer = JsonSerializer.Create(defaultSerializerSettings);
                    jsonSerializer.Serialize(jsonTextWriter, value);
                    jsonTextWriter.Flush();
                }
                TaskCompletionSource<AsyncVoid> tcs = new TaskCompletionSource<AsyncVoid>();
                tcs.SetResult(default(AsyncVoid));
                return tcs.Task;
            }
            catch (Exception ex)
            {
                var task = new TaskCompletionSource<AsyncVoid>();
                task.SetException(ex);
                return task.Task;
            }
        }
    }
}