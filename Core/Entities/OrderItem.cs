namespace Core.Entities
{
    public class OrderItem : BaseEntity
    {
        public int productId { get; set; }
        public string? productName { get; set; }
        public int Quantity { get; set; } 
        
        public double UnitPrice { get; set; }
        public double ItemTotal { get; set; }
        public string? ImageName { get; set; }
        public Order? Order { get; set; }    
        public int OrderId { get; set; }
    }
}