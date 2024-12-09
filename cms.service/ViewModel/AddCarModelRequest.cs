using FluentValidation;
using Microsoft.AspNetCore.Http;
namespace cms.service.ViewModel
{
    public class AddCarModelRequest
    {
        public string Brand { get; set; } = default!;
        public string ClassName { get; set; } = default!;
        public string Features { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public List<IFormFile> Images { get; set; }
    }
    public class AddCarModelVMValidator : AbstractValidator<AddCarModelRequest>
    {
        public AddCarModelVMValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Features).NotEmpty().WithMessage("Features is required.");
        }
    }
}
