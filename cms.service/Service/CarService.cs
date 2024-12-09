using AutoMapper;
using cms.data.Data;
using cms.data.Model;
using cms.service.Interface;
using cms.service.ViewModel;
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
                               Brand = car.Brand,
                               Description = car.Description,
                               ModelCode = car.ModelCode,
                               ModelName = car.ModelName,
                               DateofManufacturing = car.DateofManufacturing,
                               Features = carModel.Features,
                               Price = carModel.Price,
                               ImageUrl = _db.CarImages.Where(x => x.ClassId == carModel.Id).Select(x => x.ImageUrl).ToList(),
                               ClassName = carModel.ClassName
                           }).AsQueryable();
            try
            {
                if (carList != null)
                {
                    if (search != null)
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
                        carList = carList.Skip(pageNumber).Take(pageSize);
                    }
                    response.pageNumber = pageNumber;
                    response.pageSize = pageSize;
                    response.totalPage=(int)Math.Ceiling(totalCount / (double)pageSize);
                    response.totalRecords = totalCount;
                    response.jsonData = carList;
                }
                return response;
            }
            catch (Exception)
            {
                response.totalRecords = 0;
                response.pageSize = 0;
                response.jsonData = "";
                return response;
            }
        }

        public async Task<CarModelResponse> GetCarModelByBrandName(string brand)
        {
              var carList = await (from car in _db.Cars
                            join carModel in _db.CarModels on car.Id equals carModel.CarId
                            where car.Brand==brand
                            select new CarModelResponse
                            {
                               Brand = car.Brand,
                               Features = carModel.Features,
                               Price = carModel.Price,
                               Images = _db.CarImages.Where(x => x.ClassId == carModel.Id).Select(x => x.ImageUrl).ToList(),
                               ClassName = carModel.ClassName
                           }).FirstOrDefaultAsync();

            return carList;
        }

        public async Task<ServiceResponse> AddCar(AddCarRequest model){
            ServiceResponse response = new ServiceResponse();
            var cars = await _db.Cars.Where(x => x.Brand == model.Brand).FirstOrDefaultAsync();
            if (cars != null)
            {
                var newCar = _mapper.Map<Car>(model);
                _db.Cars.Update(newCar);
                response.IsSuccess = true;
                response.Message = "Record updated";
            }
            else
            {
                var newCar = _mapper.Map<Car>(model);
                await _db.Cars.AddAsync(newCar);
                response.IsSuccess = true;
                response.Message = "Record added";
            }
            await _db.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse> AddCarModel(AddCarModelRequest model, List<string> lstImage)
        {
            ServiceResponse response = new ServiceResponse();
            var cardId = await _db.Cars.Where(x => x.Brand == model.Brand).Select(x=>x.Id).FirstOrDefaultAsync();
            var carsModels = await _db.CarModels.Where(x => x.CarId == cardId).FirstOrDefaultAsync();
            if (carsModels != null)
            {
                carsModels.Price = model.Price;
                carsModels.Features = model.Features;
                _db.CarModels.Update(carsModels);
                response.IsSuccess = true;
                response.Message = "Record updated";
                if (lstImage.Count > 0)
                {
                    await AddImage(carsModels.Id, lstImage);
                }
            }
            else
            {
                carsModels.ClassName = model.ClassName;
                carsModels.Price = model.Price;
                carsModels.Features = model.Features;
                await _db.CarModels.AddAsync(carsModels);
                response.IsSuccess = true;
                response.Message = "Record added";
                if (lstImage.Count > 0)
                {
                    var getNewId = await _db.CarModels.Where(x => x.CarId == cardId && x.ClassName == model.ClassName).Select(x => x.Id).SingleOrDefaultAsync();
                    await AddImage(getNewId, lstImage);
                }
            }
            await _db.SaveChangesAsync();
            return response;
        }
        public async Task AddImage(long Id, List<string> lstImage)
        {
            foreach(var img in lstImage)
            {
                await _db.AddAsync(new CarImage { ClassId = Id, ImageUrl = img });                
            }
            await _db.SaveChangesAsync();
        }
    }
}
