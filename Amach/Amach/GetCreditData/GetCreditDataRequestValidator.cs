using FluentValidation;

namespace Amach.GetCreditData;

public class GetCreditDataRequestValidator : AbstractValidator<GetCreditDataRequest>
{
    private static readonly char Delimiter = '-';
    private static readonly string[] AvailableSsns = new[]
    {
        "424-11-9327",
        "553-25-8346",
        "287-54-7823"
    };

    public GetCreditDataRequestValidator()
    {
        RuleFor(request => request.Ssn).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("ssn.empty")
            .Length(AvailableSsns[0].Length).WithErrorCode("ssn.length.invalid")
            .Must(ssn => IsValidFormat(ssn));
    }

    private static bool IsValidFormat(ReadOnlySpan<char> ssn)
    {
        var isValidFirstNumber = int.TryParse(ssn[..3], out int firstNumber);
        if (!isValidFirstNumber)
            return false;

        var firstDelimiter = ssn.IndexOf(Delimiter);
        if (firstDelimiter == -1)
            return false;

        var isValidSecondNumber = int.TryParse(ssn.Slice(firstDelimiter + 1, 2), out int secondNumber);
        if (!isValidSecondNumber)
            return false;

        var lastDelimiter = ssn.LastIndexOf(Delimiter);
        if (lastDelimiter == -1)
            return false;

        var isValidThirdNumber = int.TryParse(ssn[(lastDelimiter + 1)..], out int thirdNumber);
        return isValidThirdNumber;
    }
}