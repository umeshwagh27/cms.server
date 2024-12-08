namespace cms.service.ViewModel
{
    public class CarViewModel
    {
        public string Brand { get; set; } = default!;     
        public string ModelName { get; set; } = default!;
        public string ModelCode { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DateofManufacturing { get; set; } = default!;
        public long ClassId { get; set; }
        public string ClassName { get; set; } = default!;
        public string Features { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public long? ImageId { get; set; }
        public List<string> ImageUrl=new List<string>();
    }

}
