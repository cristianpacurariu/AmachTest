using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Amach.GetCreditData;

public class GetCreditDataRequest
{
    /// <summary> Social security number. 2</summary>
    /// <example>424-11-9328</example>
    [JsonPropertyName("ssn")]
    [FromRoute(Name = "ssn")]
    public string Ssn { get; set; }
}