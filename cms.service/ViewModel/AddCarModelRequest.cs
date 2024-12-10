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
        public List<IFormFile>? Images { get; set; }
    }
    public class AddCarModelVMValidator : AbstractValidator<AddCarModelRequest>
    {
        public AddCarModelVMValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Features).NotEmpty().WithMessage("Features is required.");
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            RuleForEach(x => x.Images).ChildRules(images =>
            {
                images.RuleFor(img => img.Length)
                    .LessThanOrEqualTo(maxFileSize)
                    .WithMessage("Each file must be less than 5 MB.");

                images.RuleFor(img => img.FileName)
                    .Must(fileName => allowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
                    .WithMessage("Only .jpg, .jpeg, .png, and .gif files are allowed.");
            });
        }
    }
}
