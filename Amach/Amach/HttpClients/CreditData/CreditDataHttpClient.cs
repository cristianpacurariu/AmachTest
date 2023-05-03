using Amach.HttpClients.CreditData.Dtos;

namespace Amach.HttpClients.CreditData
{
    public class CreditDataHttpClient
    {
        public const string Url = "https://infra.devskills.app/api/credit-data";
        private readonly HttpClient _httpClient;

        public CreditDataHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PersonalDetailsDto> GetPersonalDetailsAsync(string ssn, CancellationToken cancellationToken)
            => await TryGetData<PersonalDetailsDto>($"personal-details/{ssn}", cancellationToken);

        public async Task<AssesedIncomeDto> GetAssesedIncomeAsync(string ssn, CancellationToken cancellationToken)
            => await TryGetData<AssesedIncomeDto>($"assessed-income/{ssn}", cancellationToken);

        public async Task<DebtDto> GetDebtAsync(string ssn, CancellationToken cancellationToken)
            => await TryGetData<DebtDto>($"debt/{ssn}", cancellationToken);

        private async Task<TResult> TryGetData<TResult>(string urlPart, CancellationToken cancellationToken)
            where TResult : class
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<TResult>(
                    urlPart,
                    cancellationToken);

                return result;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }
    }
}