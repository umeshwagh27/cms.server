using AutoMapper;
using cms.data.Data;
using cms.data.Model;
using cms.service.Interface;
using cms.service.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace cms.service.Service
{
    public class CarService:ICar
    {
        private readonly CMSDbContext _db;
        private readonly IMapper _mapper;
        public CarService(CMSDbContext dbContext, IMapper mapper)
        {
            _db=dbContext;
            _mapper = mapper;
        }

        public DataTableParamModel GetCarList(int pageNumber, int pageSize, string search, string sortBy, string sortOrder)
        {
            DataTableParamModel response = new DataTableParamModel();
            var carList = (from car in _db.Cars
                           join carModel in _db.CarModels on car.Id equals carModel.CarId
                           select new CarViewModel
                           {
                               ClassId= carModel.Id,
                               Brand = car.Brand,
                               Description = car.Description,
                               ModelCode = car.ModelCode,
                               ModelName = car.ModelName,
                               DateofManufacturing = car.DateofManufacturing,
                               Features = carModel.Features,
                               Price = carModel.Price,
                               ImageUrl = _db.CarImages.Where(x => x.ClassId == carModel.Id).Select(img => string.Format(
                       "data:image/jpg;base64,{0}", Convert.ToBase64String(img.Data))).ToList(),                        
                               ClassName = carModel.ClassName
                           }).AsQueryable();
            try
            {
                if (carList != null)
                {
                    if (search != "")
                    {
                        carList = carList.Where(u => u.ModelCode.Contains(search.Trim()) || u.ModelName.Contains(search.Trim()));
                    }
                    switch (sortBy)
                    {
                        case "dateofManufacturing":
                            carList = sortOrder == "asc"
                                ? carList.OrderBy(s => s.DateofManufacturing)
                                : carList.OrderByDescending(s => s.DateofManufacturing);
                            break;
                        default:
                            carList = carList.OrderByDescending(s => s.DateofManufacturing);
                            break;
                    }

                    var totalCount = carList.Count();
                    if (pageSize > 0)
                    {
                        carList = carList.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                    }
                    response.pageNumber = pageNumber;
                    response.pageSize = pageSize;
                    response.totalPage=(int)Math.Ceiling(totalCount / (double)pageSize);
                    response.totalRecords = totalCount;
                    response.carViewModel = carList;
                }
                return response;
            }
            catch (Exception)
            {
                response.totalRecords = 0;
                response.pageSize = 0;
                response.carViewModel = null;
                return response;
            }
        }

        public async Task<CarModelResponse> GetCarModelByBrandClassName(string brand,string className)
        {
              var result = await (from car in _db.Cars
                            join carModel in _db.CarModels on car.Id equals carModel.CarId
                            where car.Brand==brand && carModel.ClassName==className
                            select new CarModelResponse
                            {

                               Brand = car.Brand,
                               Features = carModel.Features,
                               Price = carModel.Price,
                               Images = _db.CarImages.Where(x => x.ClassId == carModel.Id).Select(img => string.Format(
 "data:image/jpg;base64,{0}", Convert.ToBase64String(img.Data))).ToList(),
                               ClassName = carModel.ClassName
                           }).FirstOrDefaultAsync();

            
            return result==null?new CarModelResponse():result;
        }

        public async Task<CarResponse> GetCarDetailByBrandName(string brand)
        {
            var result = await (from car in _db.Cars where brand.Equals(car.Brand) 
                                select new CarResponse
                                {
                                    Brand = car.Brand,                                   
                                    Description = car.Description,
                                    DateofManufacturingString = car.DateofManufacturing.ToString("dd/MM/yyyy"),
                                    ModelCode = car.ModelCode,
                                    ModelName = car.ModelName,
                                }).FirstOrDefaultAsync();

            return result == null ? new CarResponse() : result;
        }

        //Add or edit car
        public async Task<ServiceResponse> AddEditCar(AddCarRequest model)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (model.Brand.ToLower() == "audi" || model.Brand.ToLower() == "jaguar" || model.Brand.ToLower() == "land rover" || model.Brand.ToLower() == "renault")
                {
                    var cars = await _db.Cars.Where(x => x.Brand == model.Brand).FirstOrDefaultAsync();
                    if (cars != null)
                    {
                        cars.Brand = model.Brand;
                        cars.DateofManufacturing = model.DateofManufacturing;
                        cars.ModelName = model.ModelName;
                        cars.ModelCode = model.ModelCode;
                        cars.IsActive = true;
                        _db.Cars.Update(cars);
                        response.IsSuccess = true;
                        response.Message = "Record updated";
                    }
                    else
                    {
                        var newCar = _mapper.Map<Car>(model);
                        newCar.IsActive = true;
                        await _db.Cars.AddAsync(newCar);
                        response.IsSuccess = true;
                        response.Message = "Record added";
                    }
                    await _db.SaveChangesAsync();
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid brand";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        
        //Add or edit car model
        public async Task<ServiceResponse> AddEditCarModel(AddCarModelRequest model)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if (model.ClassName == "A" || model.ClassName == "A" || model.ClassName == "C")
                {
                    var cardId = await _db.Cars.Where(x => x.Brand == model.Brand).Select(x => x.Id).FirstOrDefaultAsync();
                    var carsModels = await _db.CarModels.Where(x => x.CarId == cardId).FirstOrDefaultAsync();
                    if (carsModels != null)
                    {
                        carsModels.Price = model.Price;
                        carsModels.Features = model.Features;
                        _db.CarModels.Update(carsModels);
                        response.IsSuccess = true;
                        response.Message = "Record updated";
                        if (model.Images !=null)
                        {
                            await AddImage(carsModels.Id, model.Images);
                        }
                    }
                    else
                    {
                        CarModelClass carModel = new CarModelClass()
                        {
                            CarId = cardId,
                            ClassName = model.ClassName,
                            Price = model.Price,
                            Features = model.Features
                        };
                        await _db.CarModels.AddAsync(carModel);
                        await _db.SaveChangesAsync();
                        response.IsSuccess = true;
                        response.Message = "Record added";
                        if (model.Images != null)
                        {
                            var getNewId = await _db.CarModels.Where(x => x.CarId == cardId && x.ClassName == model.ClassName).Select(x => x.Id).SingleOrDefaultAsync();
                            await AddImage(getNewId, model.Images);
                            await _db.SaveChangesAsync();
                        }
                    }
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid class";
                    return response;
                }
            }
            catch (Exception ex) {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        //Add new Image
        public async Task AddImage(long Id, List<IFormFile> lstImage)
        {
            var existingImg =await _db.CarImages.Where(x => x.ClassId == Id).ToListAsync();
            _db.CarImages.RemoveRange(existingImg);
            await _db.SaveChangesAsync();
            foreach (var img in lstImage)
            {
                CarImage carImage = new CarImage();
                carImage.Name = img.FileName;
                carImage.ClassId = Id;

                MemoryStream ms = new MemoryStream();
                img.CopyTo(ms);
                carImage.Data = ms.ToArray();

                ms.Close();
                ms.Dispose();

                await _db.AddAsync(carImage);                
            }
            await _db.SaveChangesAsync();
        }
    }
}
