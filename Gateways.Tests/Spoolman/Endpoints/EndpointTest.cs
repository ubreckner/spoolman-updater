using RichardSzalay.MockHttp;
using System.Reflection;

namespace Gateways.Tests
{
    internal class EndpointTest<TSpoolmanEndpoint> where TSpoolmanEndpoint : class
    {
        protected HttpClient HttpClient;
        protected TSpoolmanEndpoint Endpoint;

        [SetUp]
        public void Setup()
        {
            var mockHandler = new MockHttpMessageHandler();

            SetupHttpClient(mockHandler);

            HttpClient = mockHandler.ToHttpClient();
            HttpClient.BaseAddress = new Uri($"http://localhost:8080/api/v1/");

            Endpoint = (TSpoolmanEndpoint)typeof(TSpoolmanEndpoint)
                .GetConstructor(new Type[] { typeof(SpoolmanConfiguration) })?
                .Invoke(new object[] { new SpoolmanConfiguration() { Url = "http://localhost:8080" } });

            var field = Endpoint.GetType().GetField("HttpClient", BindingFlags.NonPublic | BindingFlags.Instance);

            typeof(FieldInfo).GetField("m_flags", BindingFlags.NonPublic | BindingFlags.Instance)?
                .SetValue(field, (int)field.Attributes & ~(int)FieldAttributes.InitOnly);

            // Set a new value for the field
            field.SetValue(Endpoint, HttpClient);
        }

        public virtual void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {

        }

        [TearDown]
        public void TearDown()
        {
            HttpClient.Dispose();
        }
    }
}