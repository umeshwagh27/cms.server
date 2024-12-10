using cms.service.Interface;
using cms.service.ViewModel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace cms.server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController
    {
        private readonly IValidator<AddCarRequest> _validator;
        private readonly IValidator<AddCarModelRequest> _validatorCarModel;
        private readonly ICar _car;
        public CarController(ICar car, IValidator<AddCarRequest> validator, IValidator<AddCarModelRequest> validatorCarMode) {
            _car = car;
            _validator = validator;
            _validatorCarModel = validatorCarMode;
        }

        [HttpGet]
        [Route("getCarList")]
        public IActionResult GetCarList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search= "", [FromQuery] string sortBy = "", [FromQuery] string sortOrder = "asc")
        {         
            return new JsonResult(_car.GetCarList(pageNumber, pageSize,search,sortBy,sortOrder));
        }

        [HttpGet]
        [Route("getCarModelByBrandClassName")]
        public async Task<IActionResult> GetCarModelByBrandClassName([FromQuery] string brand = "", [FromQuery] string className = "")
        {
            return new JsonResult(await _car.GetCarModelByBrandClassName(brand,className));
        }

        [HttpGet]
        [Route("getCarDetailByBrandName")]
        public async Task<IActionResult> GetCarDetailByBrandName([FromQuery] string brand = "Audi")
        {
            return new JsonResult(await _car.GetCarDetailByBrandName(brand));
        }

        [HttpPost]
        [Route("addeditCar")]
        public async Task<JsonResult> AddEditCar([FromForm] AddCarRequest carVM)
        {
            var validationResult = _validator.Validate(carVM);

            if (!validationResult.IsValid)
            {
                ServiceResponse response = new ServiceResponse();
                response.IsSuccess = false;
                response.Message = validationResult.Errors[0].ErrorMessage;
                return new JsonResult(validationResult.Errors);
            }
            return new JsonResult(await _car.AddEditCar(carVM));
        }
        [HttpPost]
        [Route("addeditCarModel")]
        public async Task<JsonResult> AddEditCarModel([FromForm] AddCarModelRequest carModelVM)
        { 
            var validationResult = _validatorCarModel.Validate(carModelVM);

            if (!validationResult.IsValid)
            {
                ServiceResponse response = new ServiceResponse();
                response.IsSuccess = false;
                response.Message = validationResult.Errors[0].ErrorMessage;
                return new JsonResult(response);
            }
            return new JsonResult(await _car.AddEditCarModel(carModelVM));
        }
    }
}
