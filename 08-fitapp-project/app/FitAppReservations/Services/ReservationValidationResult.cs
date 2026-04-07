namespace FitAppReservations.Services;

public sealed record ReservationValidationResult(bool IsValid, IReadOnlyList<string> Errors)
{
    public static ReservationValidationResult Success() => new(true, Array.Empty<string>());

    public static ReservationValidationResult Failure(IReadOnlyList<string> errors) => new(false, errors);
}
