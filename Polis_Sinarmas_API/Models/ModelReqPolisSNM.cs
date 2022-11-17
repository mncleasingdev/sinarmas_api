using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Polis_Sinarmas_API.Models
{
    class ModelReqPolisSNM
    {
        public partial class SinarmasReq
        {
            [JsonProperty("NBWorkPage")]
            public NbWorkPage NbWorkPage { get; set; }
        }

        public partial class NbWorkPage
        {
            [JsonProperty("Quotation")]
            public Quotation Quotation { get; set; }

            [JsonProperty("Customer_P")]
            public CustomerP CustomerP { get; set; }

            [JsonProperty("Policy")]
            public Policy Policy { get; set; }

            [JsonProperty("AddressList")]
            public List<AddressList> AddressList { get; set; }

            [JsonProperty("PersonList")]
            public List<PersonList> PersonList { get; set; }
        }

        public partial class AddressList
        {
            [JsonProperty("ASMAddress")]
            public string AsmAddress { get; set; }

            [JsonProperty("ASMAddressType")]
            public string AsmAddressType { get; set; }

            [JsonProperty("ASMZipCode")]
            public string AsmZipCode { get; set; }

            [JsonProperty("ASMTelfax")]
            public List<AsmTelfax> AsmTelfax { get; set; }
        }

        public partial class AsmTelfax
        {
            [JsonProperty("TelfaxCode")]
            public string TelfaxCode { get; set; }

            [JsonProperty("TelfaxNumber")]
            public string TelfaxNumber { get; set; }

            [JsonProperty("TelfaxType")]
            public string TelfaxType { get; set; }
        }

        public partial class CustomerP
        {
            [JsonProperty("pyFirstName")]
            public string PyFirstName { get; set; }

            [JsonProperty("pyLastName")]
            public string PyLastName { get; set; }

            [JsonProperty("pyCity")]
            public string PyCity { get; set; }

            [JsonProperty("ASMDateOfBirth")]
            public string AsmDateOfBirth { get; set; }

            [JsonProperty("ASMGender")]
            public string AsmGender { get; set; }

            [JsonProperty("ASMIDCard")]
            public string AsmidCard { get; set; }

            [JsonProperty("pyCompany")]
            public string PyCompany { get; set; }
        }

        public partial class PersonList
        {
            [JsonProperty("ASMDateOfBirth")]
            public string AsmDateOfBirth { get; set; }

            [JsonProperty("ASMHeight")]
            public string AsmHeight { get; set; }

            [JsonProperty("ASMIDCard")]
            public string AsmidCard { get; set; }

            [JsonProperty("ASMJobName")]
            public string AsmJobName { get; set; }

            [JsonProperty("ASMLeftHanded")]
            public string AsmLeftHanded { get; set; }

            [JsonProperty("ASMParticipantStatus")]
            public string AsmParticipantStatus { get; set; }

            [JsonProperty("ASMWeight")]
            public string AsmWeight { get; set; }

            [JsonProperty("StartDateTime")]
            public string StartDateTime { get; set; }

            [JsonProperty("EndDateTime")]
            public string EndDateTime { get; set; }

            [JsonProperty("pyFullName")]
            public string PyFullName { get; set; }

            [JsonProperty("ASMCoverage")]
            public List<AsmCoverage> AsmCoverage { get; set; }

            [JsonProperty("ASMHeir")]
            public List<AsmHeir> AsmHeir { get; set; }
        }

        public partial class AsmCoverage
        {
            [JsonProperty("Coverage")]
            public string Coverage { get; set; }

            [JsonProperty("CoverageNote")]
            public string CoverageNote { get; set; }

            [JsonProperty("DiscountPercentage")]
            public string DiscountPercentage { get; set; }

            [JsonProperty("CalculateMethod")]
            public string CalculateMethod { get; set; }

            [JsonProperty("Rate")]
            public string Rate { get; set; }

            [JsonProperty("TSI")]
            public string Tsi { get; set; }
        }

        public partial class AsmHeir
        {
            [JsonProperty("ASMDateOfBirth")]
            public string AsmDateOfBirth { get; set; }

            [JsonProperty("ASMGender")]
            public string AsmGender { get; set; }

            [JsonProperty("ASMHeirPercentage")]
            public string AsmHeirPercentage { get; set; }

            [JsonProperty("ASMRelationName")]
            public string AsmRelationName { get; set; }

            [JsonProperty("pyFullName")]
            public string PyFullName { get; set; }
        }

        public partial class Policy
        {
            [JsonProperty("StartDateTime")]
            public string StartDateTime { get; set; }

            [JsonProperty("EndDateTime")]
            public string EndDateTime { get; set; }

            [JsonProperty("QQName")]
            public string QqName { get; set; }

            [JsonProperty("CustomerType")]
            public string CustomerType { get; set; }

            [JsonProperty("TheInsured")]
            public string TheInsured { get; set; }

            [JsonProperty("IdTransaction")]
            public string IdTransaction { get; set; }

            [JsonProperty("TripType")]
            public string TripType { get; set; }

            [JsonProperty("Currency")]
            public string Currency { get; set; }

            [JsonProperty("TypeOfPacket")]
            public string TypeOfPacket { get; set; }

            [JsonProperty("StatusPenerbitan")]
            public string StatusPenerbitan { get; set; }
        }

        public partial class Quotation
        {
            [JsonProperty("BusinessCode")]
            public string BusinessCode { get; set; }

            [JsonProperty("BusinessName")]
            public string BusinessName { get; set; }

            [JsonProperty("GroupPanel")]
            public string GroupPanel { get; set; }

            [JsonProperty("AccessCode")]
            public string AccessCode { get; set; }
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }
    }
}
