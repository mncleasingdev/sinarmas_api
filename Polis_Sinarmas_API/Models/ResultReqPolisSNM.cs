using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Polis_Sinarmas_API.Models
{
    public class ResultReqPolisSNM
    {
        public string PolicyNo { get; set; }
        public string CaseID { get; set; }
        public decimal Discount { get; set; }
        public string PDF64 { get; set; }
        public string HTTPCode { get; set; }
        public decimal Premium { get; set; }
        public decimal AdminFee { get; set; }
        public string StampReceipts { get; set; }
        public string FeedbackMessage { get; set; }
        public string IdTransaction { get; set; }
        public string PDF64_Attachment { get; set; }
        public string StatusPenerbitan { get; set; }
        public decimal Commision { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal SumPaymentTotal { get; set; }
        public string ConID { get; set; }
        public string StampPolicy { get; set; }
    }
}
