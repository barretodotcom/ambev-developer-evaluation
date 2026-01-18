namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed class Percentage
{
    public decimal Value { get; }

    private Percentage(decimal value)
    {
        if (value is < 0m or > 1m)
            throw new DomainException("Percentage must be between 0 and 1.");

        Value = value;
    }

    public static Percentage Create(decimal value)
    {
        return new Percentage(value);

    }

    public static Percentage Zero()
    {
        return new Percentage(decimal.Zero);
    }
}