﻿using cms.server.Utility;
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
        private readonly IValidator<AddCarVM> _validator;
        private readonly IValidator<AddCarModelVM> _validatorCarModel;
        private readonly ICar _car;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CarController(ICar car, IValidator<AddCarVM> validator, IValidator<AddCarModelVM> validatorCarMode,IWebHostEnvironment webHostEnvironment) {
            _car = car;
            _validator = validator;
            _validatorCarModel = validatorCarMode;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("getCarList")]
        public IActionResult GetCarList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search= "ModelName", [FromQuery] string sortBy = "Brand", [FromQuery] string sortOrder = "asc")
        {         
            return new JsonResult(_car.GetCarList(pageNumber, pageSize,search,sortBy,sortOrder));
        }


        [HttpPost]
        [Route("addCar")]
        public async Task<JsonResult> AddCar([FromForm] AddCarVM carVM)
        {
            var validationResult = _validator.Validate(carVM);

            if (!validationResult.IsValid)
            {
                return new JsonResult(validationResult.Errors);
            }
            return new JsonResult(await _car.AddCar(carVM));
        }
        [HttpPost]
        [Route("addCarModel")]
        public async Task<JsonResult> AddCarModel([FromForm] AddCarModelVM carModelVM)
        { 
            var validationResult = _validatorCarModel.Validate(carModelVM);

            if (!validationResult.IsValid)
            {
                return new JsonResult(validationResult.Errors);
            }

            if (carModelVM.Images != null)
            {
                List<string> lstImage = new List<string>();
                foreach (var image in carModelVM.Images)
                {
                    var filename = await CommonMethod.WriteFile(_webHostEnvironment.WebRootPath, "CarImg", "Img", image);
                    lstImage.Add(image.FileName);
                }
            }

            return new JsonResult(await _car.AddCarModel(carModelVM,lstImage));
        }
    }
}
