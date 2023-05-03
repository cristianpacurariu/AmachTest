using System.Text.Json.Serialization;

namespace Amach.GetCreditData;

public class CreditData
{
    /// <example>Emma</example>
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    /// <example>Gautrey</example>
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    /// <example>09 Westend Terrace</example>
    [JsonPropertyName("address")]
    public string Address { get; set; }

    /// <example>60668</example>
    [JsonPropertyName("assessed_income")]
    public int AssessedIncome { get; set; }

    /// <example>11585</example>
    [JsonPropertyName("balance_of_debt")]
    public int BalanceOfDebt { get; set; }

    /// <example>true</example>
    [JsonPropertyName("complaints")]
    public bool Complaints { get; set; }
}