namespace ComputerStoreApp.Models.Dtos
{
    public class CalculatedDiscountDto
    {
        public double OrderTotalPrice { get; set; }
        public double Discount { get; set; }
        public double OrderDiscountedPrice { get; set; }
        public int NumberOfDiscountedProducts { get; set; }
    }
}
