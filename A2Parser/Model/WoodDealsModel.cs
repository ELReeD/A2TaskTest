using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2Parser.Model
{
    public class WoodDealsModel
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public SearchReportWoodDeal SearchReportWoodDeal { get; set; }
    }   

    public class SearchReportWoodDeal
    {
        public int Total { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
        public double OverallBuyerVolume { get; set; }
        public double OverallSellerVolume { get; set; }
        public List<Content> Content { get; set; }
        public string __typename { get; set; }

    }

    public class Content
    {
        public string DealNumber { get; set; } 
        public string SellerName { get; set; } 
        public string SellerInn { get; set; }
        public string BuyerName { get; set; }  
        public string BuyerInn { get; set; } 
        public string DealDate { get; set; } 
        public double? WoodVolumeBuyer { get; set; } 
        public double? WoodVolumeSeller { get; set; } 
        public string __typename { get; set; }
    }
}
