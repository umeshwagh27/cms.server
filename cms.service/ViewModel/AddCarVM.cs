using FluentValidation;

namespace cms.service.ViewModel
{
    public class AddCarVM
    {
        public string Brand { get; set; } = default!;
        public string ModelName { get; set; } = default!;
        public string ModelCode { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DateofManufacturing { get; set; } = default!;
    }
    public class AddCarVMValidator : AbstractValidator<AddCarVM>
    {
        public AddCarVMValidator()
        {
            RuleFor(x => x.Brand).NotEmpty().WithMessage("Brand is required.");
            RuleFor(x => x.ModelName).NotEmpty().WithMessage("Model Name is required.");
            RuleFor(x => x.ModelCode).Matches("^[a-zA-Z0-9]{10}$").WithMessage("Model Code must be 10 alphanumeric characters.");
            RuleFor(x => x.DateofManufacturing).NotEmpty().WithMessage("Date of Manufacturing is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        }
    }
}
