using System.Text.Json.Serialization;

namespace Amach.HttpClients.CreditData.Dtos
{
    public class DebtDto
    {
        [JsonPropertyName("balance_of_debt")]
        public int BalanceOfDebt { get; set; }

        [JsonPropertyName("complaints")]
        public bool Complaints { get; set; }
    }
}