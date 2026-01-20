using Ambev.DeveloperEvaluation.Application.Read.Enums;

namespace Ambev.DeveloperEvaluation.Application.Read.Common;

public static class OrderParser
{
    public static OrderBy Parse(string? order, OrderBy defaultOrder)
    {
        if (string.IsNullOrWhiteSpace(order))
            return defaultOrder;

        var parts = order.Split(':', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            return defaultOrder;

        var field = parts[0];
        var direction = parts[1].ToLower() switch
        {
            "asc" => OrderDirection.Asc,
            "desc" => OrderDirection.Desc,
            _ => defaultOrder.Direction
        };

        return new OrderBy
        {
            Field = field, 
            Direction = direction,
        };
    }
}
