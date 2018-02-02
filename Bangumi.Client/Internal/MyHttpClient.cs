using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.Web.Http.Filters;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;
using IHttpAsyncOperation = Windows.Foundation.IAsyncOperationWithProgress<Windows.Web.Http.HttpResponseMessage, Windows.Web.Http.HttpProgress>;
using IHttpStringAsyncOperation = Windows.Foundation.IAsyncOperationWithProgress<string, Windows.Web.Http.HttpProgress>;
using IHttpBufferAsyncOperation = Windows.Foundation.IAsyncOperationWithProgress<Windows.Storage.Streams.IBuffer, Windows.Web.Http.HttpProgress>;
using IHttpInputStreamAsyncOperation = Windows.Foundation.IAsyncOperationWithProgress<Windows.Storage.Streams.IInputStream, Windows.Web.Http.HttpProgress>;
using IHttpDocumentAsyncOperation = Windows.Foundation.IAsyncOperationWithProgress<HtmlAgilityPack.HtmlDocument, Windows.Web.Http.HttpProgress>;
using Windows.ApplicationModel;
using Newtonsoft.Json;

namespace Bangumi.Client.Internal
{
    internal static class MyHttpClient
    {
        static MyHttpClient()
        {
            var filter = new HttpBaseProtocolFilter();
            inner = new HttpClient(filter);
            filter.CookieUsageBehavior = HttpCookieUsageBehavior.Default;
            CookieManager = filter.CookieManager;
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Mozilla", "5.0"));
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Windows NT 10.0; Win64; x64"));
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("AppleWebKit", "537.36"));
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("KHTML, like Gecko"));
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Chrome", "58.0.3029.110"));
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Safari", "537.36"));
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Edge", "16.16299"));
            var version = Package.Current.Id.Version;
            DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue(Package.Current.DisplayName, $"{version.Major}.{version.Minor}.{version.Revision}"));
        }

        private static readonly HttpClient inner = new HttpClient();

        public static HttpCookieManager CookieManager { get; }

        public static HttpRequestHeaderCollection DefaultRequestHeaders => inner.DefaultRequestHeaders;

        private static void reformUri(ref Uri uri)
        {
            if (!uri.IsAbsoluteUri)
            {
                uri = new Uri(Config.RootUri, uri);
            }
        }

        private static IHttpStringAsyncOperation loadStringAsync(IHttpAsyncOperation operation)
        {
            return Run<string, HttpProgress>(async (token, progress) =>
            {
                token.Register(operation.Cancel);
                operation.Progress = (t, p) => progress.Report(p);
                var response = await operation;
                return await response.Content.ReadAsStringAsync();
            });
        }

        private static IAsyncOperationWithProgress<T, HttpProgress> loadJsonAsync<T>(IHttpAsyncOperation operation)
        {
            return Run<T, HttpProgress>(async (token, progress) =>
            {
                token.Register(operation.Cancel);
                operation.Progress = (t, p) => progress.Report(p);
                var response = await operation;
                var s = await response.Content.ReadAsStringAsync();
                var r = JsonConvert.DeserializeObject<T>(s, ResponseObject.JsonSettings);
                if (r is ResponseObject ro)
                    ResponseObject.Check(ro);
                return r;
            });
        }
        private static IAsyncActionWithProgress<HttpProgress> loadJsonAsync<T>(IHttpAsyncOperation operation, T obj)
        {
            return Run<HttpProgress>(async (token, progress) =>
            {
                token.Register(operation.Cancel);
                operation.Progress = (t, p) => progress.Report(p);
                var response = await operation;
                var s = await response.Content.ReadAsStringAsync();
                JsonConvert.PopulateObject(s, obj, ResponseObject.JsonSettings);
                if (obj is ResponseObject ro)
                    ResponseObject.Check(ro);
            });
        }

        private static IHttpBufferAsyncOperation loadBufferAsync(IHttpAsyncOperation operation)
        {
            return Run<IBuffer, HttpProgress>(async (token, progress) =>
            {
                token.Register(operation.Cancel);
                operation.Progress = (t, p) => progress.Report(p);
                var response = await operation;
                return await response.Content.ReadAsBufferAsync();
            });
        }

        private static IHttpInputStreamAsyncOperation loadInputStreamAsync(IHttpAsyncOperation operation)
        {
            return Run<IInputStream, HttpProgress>(async (token, progress) =>
            {
                token.Register(operation.Cancel);
                operation.Progress = (t, p) => progress.Report(p);
                var response = await operation;
                return await response.Content.ReadAsInputStreamAsync();
            });
        }

        private static IHttpDocumentAsyncOperation loadDocumentAsync(IHttpAsyncOperation operation)
        {
            return Run<HtmlDocument, HttpProgress>(async (token, progress) =>
            {
                token.Register(operation.Cancel);
                operation.Progress = (t, p) => progress.Report(p);
                var response = await operation;
                using (var stream = (await response.Content.ReadAsInputStreamAsync()).AsStreamForRead())
                {
                    var doc = new HtmlDocument();
                    doc.Load(stream);
                    return doc;
                }
            });
        }

        private static IHttpAsyncOperation sendAsync(IHttpAsyncOperation operation, HttpCompletionOption completionOption, bool checkStatusCode)
        {
            return Run<HttpResponseMessage, HttpProgress>(async (token, progress) =>
            {
                token.Register(operation.Cancel);
                operation.Progress = (t, p) => progress.Report(p);
                var response = await operation;
                if (checkStatusCode)
                    response.EnsureSuccessStatusCode();
                if (completionOption == HttpCompletionOption.ResponseHeadersRead)
                    return response;
                var buffer = response.Content.BufferAllAsync();
                if (!response.Content.TryComputeLength(out var length))
                {
                    var contentLength = response.Content.Headers.ContentLength;
                    if (contentLength.HasValue)
                        length = contentLength.Value;
                    else
                        length = ulong.MaxValue;
                }
                buffer.Progress = (t, p) =>
                {
                    progress.Report(new HttpProgress
                    {
                        TotalBytesToReceive = length,
                        BytesReceived = p,
                        Stage = HttpProgressStage.ReceivingContent
                    });
                };
                await buffer;
                return response;
            });
        }

        public static IHttpAsyncOperation GetAsync(Uri uri, HttpCompletionOption completionOption, bool checkStatusCode)
        {
            reformUri(ref uri);
            var request = inner.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            return sendAsync(request, completionOption, checkStatusCode);
        }

        public static IHttpAsyncOperation GetAsync(Uri uri)
            => GetAsync(uri, HttpCompletionOption.ResponseContentRead, true);

        public static IHttpBufferAsyncOperation GetBufferAsync(Uri uri)
            => loadBufferAsync(GetAsync(uri));

        public static IHttpInputStreamAsyncOperation GetInputStreamAsync(Uri uri)
            => loadInputStreamAsync(GetAsync(uri));

        public static IHttpStringAsyncOperation GetStringAsync(Uri uri)
            => loadStringAsync(GetAsync(uri));

        public static IAsyncOperationWithProgress<T, HttpProgress> GetJsonAsync<T>(Uri uri)
            => loadJsonAsync<T>(GetAsync(uri));

        public static IAsyncActionWithProgress<HttpProgress> GetJsonAsync<T>(Uri uri, T obj)
            => loadJsonAsync(GetAsync(uri), obj);

        public static IHttpDocumentAsyncOperation GetDocumentAsync(Uri uri)
            => loadDocumentAsync(GetAsync(uri));

        public static IHttpAsyncOperation PostAsync(Uri uri, IHttpContent content)
        {
            reformUri(ref uri);
            var request = inner.PostAsync(uri, content);
            return sendAsync(request, HttpCompletionOption.ResponseContentRead, true);
        }

        public static IHttpAsyncOperation PostAsync(Uri uri, string content)
            => PostAsync(uri, content == null ? null : new HttpStringContent(content));

        public static IHttpAsyncOperation PostAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> content)
            => PostAsync(uri, content == null ? null : new HttpFormUrlEncodedContent(content));

        public static IHttpStringAsyncOperation PostStringAsync(Uri uri, IHttpContent content)
            => loadStringAsync(PostAsync(uri, content));

        public static IHttpStringAsyncOperation PostStringAsync(Uri uri, string content)
            => loadStringAsync(PostAsync(uri, content));

        public static IHttpStringAsyncOperation PostStringAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> content)
            => loadStringAsync(PostAsync(uri, content));

        public static IAsyncOperationWithProgress<T, HttpProgress> PostJsonAsync<T>(Uri uri, IHttpContent content)
            => loadJsonAsync<T>(PostAsync(uri, content));

        public static IAsyncOperationWithProgress<T, HttpProgress> PostJsonAsync<T>(Uri uri, string content)
            => loadJsonAsync<T>(PostAsync(uri, content));

        public static IAsyncOperationWithProgress<T, HttpProgress> PostJsonAsync<T>(Uri uri, IEnumerable<KeyValuePair<string, string>> content)
            => loadJsonAsync<T>(PostAsync(uri, content));

        public static IAsyncActionWithProgress<HttpProgress> PostJsonAsync<T>(Uri uri, IHttpContent content, T obj)
            => loadJsonAsync(PostAsync(uri, content), obj);

        public static IAsyncActionWithProgress<HttpProgress> PostJsonAsync<T>(Uri uri, string content, T obj)
            => loadJsonAsync(PostAsync(uri, content), obj);

        public static IAsyncActionWithProgress<HttpProgress> PostJsonAsync<T>(Uri uri, IEnumerable<KeyValuePair<string, string>> content, T obj)
            => loadJsonAsync(PostAsync(uri, content), obj);

        public static IHttpBufferAsyncOperation PostBufferAsync(Uri uri, IHttpContent content)
            => loadBufferAsync(PostAsync(uri, content));

        public static IHttpBufferAsyncOperation PostBufferAsync(Uri uri, string content)
            => loadBufferAsync(PostAsync(uri, content));

        public static IHttpBufferAsyncOperation PostBufferAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> content)
            => loadBufferAsync(PostAsync(uri, content));

        public static IHttpInputStreamAsyncOperation PostInputStreamAsync(Uri uri, IHttpContent content)
            => loadInputStreamAsync(PostAsync(uri, content));

        public static IHttpInputStreamAsyncOperation PostInputStreamAsync(Uri uri, string content)
            => loadInputStreamAsync(PostAsync(uri, content));

        public static IHttpInputStreamAsyncOperation PostInputStreamAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> content)
            => loadInputStreamAsync(PostAsync(uri, content));

        public static IHttpDocumentAsyncOperation PostDocumentAsync(Uri uri, IHttpContent content)
            => loadDocumentAsync(PostAsync(uri, content));

        public static IHttpDocumentAsyncOperation PostDocumentAsync(Uri uri, string content)
            => loadDocumentAsync(PostAsync(uri, content));

        public static IHttpDocumentAsyncOperation PostDocumentAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> content)
            => loadDocumentAsync(PostAsync(uri, content));
    }
}
