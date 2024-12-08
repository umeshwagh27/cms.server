using cms.service.ViewModel;
namespace cms.service.Interface
{
    public interface ICar
    {
        DataTableParamModel GetCarList(int pageNumber,int pageSize, string search,string sortBy,string sortOrder);
        Task<ServiceResponse> AddCar(AddCarVM addCar);
        Task<ServiceResponse> AddCarModel(AddCarModelVM addCarModel, List<string> lstImage);
    }
}
