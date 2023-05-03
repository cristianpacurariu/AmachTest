using Swashbuckle.AspNetCore.Filters;

namespace Amach.GetCreditData.SwaggerExamples
{
    public class CreditDataEmma : IExamplesProvider<CreditData>
    {
        public CreditData GetExamples()
            => new()
            {
                FirstName = "Emma",
                LastName = "Gautrey",
                Address = "09 Westend Terrace",
                AssessedIncome = 60668,
                BalanceOfDebt = 11585,
                Complaints = true
            };
    }
}