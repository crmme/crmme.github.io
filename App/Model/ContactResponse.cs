using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace crmme.Model
{
    public class ScoredEmailAddress
    {
        public string address { get; set; }
        public double relevanceScore { get; set; }
        public string selectionLikelihood { get; set; }
    }

    public class PersonType
    {
        [JsonPropertyName("class")]
        public string personclass { get; set; }
    public string subclass { get; set; }
}

public class Value
{
    public string id { get; set; }
    public string displayName { get; set; }
    public object givenName { get; set; }
    public object surname { get; set; }
    public object birthday { get; set; }
    public object personNotes { get; set; }
    public bool isFavorite { get; set; }
    public object jobTitle { get; set; }
    public object companyName { get; set; }
    public object yomiCompany { get; set; }
    public object department { get; set; }
    public object officeLocation { get; set; }
    public object profession { get; set; }
    public object userPrincipalName { get; set; }
    public object imAddress { get; set; }
    public IList<ScoredEmailAddress> scoredEmailAddresses { get; set; }
    public IList<object> phones { get; set; }
    public PersonType personType { get; set; }
}

public class ContactResponse
{
    [JsonPropertyName("@odata.context")]
    public string odatacontext { get; set; }
    public IList<Value> value { get; set; }
}
}
