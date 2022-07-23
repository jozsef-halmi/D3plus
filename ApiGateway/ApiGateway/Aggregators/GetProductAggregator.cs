using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;

namespace ApiGateway.Aggregators
{
    public class GetProductAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var getProductResponse = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
            var getProductPropertiesResponse = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();


            // Basic merge
            //var merge = $"{{ \"Product\": {getProductResponse}, \"ProductProperties\":{getProductPropertiesResponse}}}";

            // More complex merge
            dynamic jsonProductData = JsonConvert.DeserializeObject<dynamic>(getProductResponse);
            dynamic jsonProductPropertiesData = JsonConvert.DeserializeObject<dynamic>(getProductPropertiesResponse);

            // Unwrap the product data from the HATEOAS response (product object is in the _embedded property)
            var productData = jsonProductData._embedded;

            // Attach properties to the product object
            productData.properties = jsonProductPropertiesData.properties;

            // Serialize and return
            var merge = JsonConvert.SerializeObject(productData);

            var headers = responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList();
            return new DownstreamResponse(new StringContent(merge), HttpStatusCode.OK, headers, "OK");
        }
    }
}
