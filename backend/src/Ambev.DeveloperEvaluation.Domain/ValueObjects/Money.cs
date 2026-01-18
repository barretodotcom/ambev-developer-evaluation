namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record Money
{
    public decimal Value { get; }

    private Money()
    {
    }

    private Money(decimal value)
    {
        Value = value;
    }

    public static Money Create(decimal value)
    {
        return new Money(value);
    }
}