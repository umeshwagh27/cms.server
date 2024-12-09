namespace cms.service.ViewModel
{
    public class CarModelResponse
    {
        public string Brand { get; set; } = default!;
        public string ClassName { get; set; } = default!;
        public string Features { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public List<string> Images { get; set; } = new List<string>();
    }
}
