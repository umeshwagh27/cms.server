using cms.service.ViewModel;
namespace cms.service.Interface
{
    public interface ICar
    {
        Task<CarModelResponse> GetCarModelByBrandName(string brand);
        DataTableParamModel GetCarList(int pageNumber,int pageSize, string search,string sortBy,string sortOrder);
        Task<ServiceResponse> AddCar(AddCarRequest addCar);
        Task<ServiceResponse> AddCarModel(AddCarModelRequest addCarModel, List<string> lstImage);
    }
}
