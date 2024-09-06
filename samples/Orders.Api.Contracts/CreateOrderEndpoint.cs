namespace Orders.Api.Contracts;

public static class CreateOrderEndpoint
{
    public static string Path() => "api/v1/orders";
    public static HttpMethod Method => HttpMethod.Post;
}

public class CreateOrderRequest
{
    public string BookId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public record CreateOrderResponse
{
    public CreateOrderResponse(
        string id,
        OrderItem item,
        decimal total)
    {
        Id = id;
        Item = item;
        Total = total;
    }
    
    public string Id { get; }
    public OrderItem Item { get; }
    public decimal Total { get; }
}

public record OrderItem
{
    public string BookId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal { get; set; }
}