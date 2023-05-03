using System.Text.Json.Serialization;

namespace Amach.HttpClients.CreditData.Dtos
{
    public class AssesedIncomeDto
    {
        [JsonPropertyName("assessed_income")]
        public int AssessedIncome { get; set; }
    }
}