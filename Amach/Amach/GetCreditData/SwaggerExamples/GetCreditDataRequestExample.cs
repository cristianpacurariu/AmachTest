using Swashbuckle.AspNetCore.Filters;

namespace Amach.GetCreditData.SwaggerExamples
{
    public class GetCreditDataRequestExample : IExamplesProvider<GetCreditDataRequest>
    {
        public GetCreditDataRequest GetExamples()
            //=> new("424-11-9327");
            => new() { Ssn = "424-11-9327" };
    }
}