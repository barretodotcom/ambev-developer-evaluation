using System.ComponentModel;

namespace Ambev.DeveloperEvaluation.Application.Enums;

/// <summary>
/// Defines the possible operations that can be performed on a sale item.
/// </summary>
public enum UpdateSaleItemOperation
{
    /// <summary>
    /// Deletes an existing sale item from the sale.
    /// </summary>
    Cancel = 0,

    /// <summary>
    /// Updates an existing sale item in the sale.
    /// </summary>
    Update = 1,

    /// <summary>
    /// Adds a new sale item to the sale.
    /// </summary>
    Create = 2
}
