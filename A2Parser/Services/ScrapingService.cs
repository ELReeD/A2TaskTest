using A2Parser.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace A2Parser.Services
{
    class ScrapingService
    {
        RestClient RestClient;
        public ScrapingService()
        {
            RestClient = new RestClient("https://www.lesegais.ru/open-area/graphql"); 
        }

        public WoodDealsModel GetData(int size, int pageNumber)
        {
            try
            {
                var body = $@"{{""query"":""query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {{\n  searchReportWoodDeal(filter: $filter, pageable: {{number: $number, size: $size}}, orders: $orders) {{\n    content {{\n      sellerName\n      sellerInn\n      buyerName\n      buyerInn\n      woodVolumeBuyer\n      woodVolumeSeller\n      dealDate\n      dealNumber\n      __typename\n    }}\n    __typename\n  }}\n}}\n"",""variables"":{{""size"":{size},""number"":{pageNumber},""filter"":null,""orders"":null}},""operationName"":""SearchReportWoodDeal""}}";

                var result = SendRequest(body);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public int GetTotal()
        {
            try
            {
                var body = @"{""query"":""query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\n    total\n    number\n    size\n    overallBuyerVolume\n    overallSellerVolume\n    __typename\n  }\n}\n"",""variables"":{""size"":20,""number"":0,""filter"":null},""operationName"":""SearchReportWoodDealCount""}";

                var result = SendRequest(body);

                return result.Data.SearchReportWoodDeal.Total;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private WoodDealsModel SendRequest(string body)
        {
            var request = new RestRequest();
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8,ru;q=0.7");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Origin", "https://www.lesegais.ru");
            request.AddHeader("Referer", "https://www.lesegais.ru/open-area/deal");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("sec-ch-ua", "\".Not/A)Brand\";v=\"99\", \"Google Chrome\";v=\"103\", \"Chromium\";v=\"103\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            return RestClient.Post<WoodDealsModel>(request);
        }
    }
}