using System.ComponentModel;

namespace Ambev.DeveloperEvaluation.Domain.Enums;

public enum SaleStatus 
{
    [Description("None")]
    None = 0,
    [Description("Cancelled")]
    Cancelled = 1,
    [Description("Active")]
    Active = 2,
}