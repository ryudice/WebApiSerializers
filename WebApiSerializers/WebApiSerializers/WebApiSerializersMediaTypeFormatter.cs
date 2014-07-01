using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (readStream == null)
                throw new ArgumentNullException("readStream");

            var serializer = SerializersCache.Current.GetSerializerForClass(type);

            if (serializer == null)
                return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);


            return TaskHelpers.RunSynchronously<object>(() =>
            {
                HttpContentHeaders contentHeaders = content == null ? null : content.Headers;

                // If content length is 0 then return default value for this type
                if (contentHeaders != null && contentHeaders.ContentLength == 0)
                {
                    return GetDefaultValueForType(type);
                }

                // Get the character encoding for the content
                Encoding effectiveEncoding = SelectCharacterEncoding(contentHeaders);

                try
                {
                        using (JsonTextReader jsonTextReader = new JsonTextReader(new StreamReader(readStream, effectiveEncoding)) { CloseInput = false, MaxDepth = 256 })
                        {

                            var defaultSerializerSettings = CreateDefaultSerializerSettings();
                            defaultSerializerSettings.ContractResolver = serializer.GetContractResolver();

                            JsonSerializer jsonSerializer = JsonSerializer.Create(defaultSerializerSettings);
                            if (formatterLogger != null)
                            {
                                // Error must always be marked as handled
                                // Failure to do so can cause the exception to be rethrown at every recursive level and overflow the stack for x64 CLR processes
                                jsonSerializer.Error += (sender, e) =>
                                {
                                    Exception exception = e.ErrorContext.Error;
                                    formatterLogger.LogError(e.ErrorContext.Path, exception);
                                    e.ErrorContext.Handled = true;
                                };
                            }
                            return jsonSerializer.Deserialize(jsonTextReader, type);
  //                      }
                    }
                }
                catch (Exception e)
                {
                    if (formatterLogger == null)
                    {
                        throw;
                    }
                    formatterLogger.LogError(String.Empty, e);
                    return GetDefaultValueForType(type);
                }
            });
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