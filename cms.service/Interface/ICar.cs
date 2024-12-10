using cms.service.ViewModel;
namespace cms.service.Interface
{
    public interface ICar
    {
        Task<CarModelResponse> GetCarModelByBrandClassName(string brand,string className);
        Task<CarResponse> GetCarDetailByBrandName(string brand);        
        DataTableParamModel GetCarList(int pageNumber,int pageSize, string search,string sortBy,string sortOrder);
        Task<ServiceResponse> AddEditCar(AddCarRequest addCar);
        Task<ServiceResponse> AddEditCarModel(AddCarModelRequest addCarModel);
    }
}
