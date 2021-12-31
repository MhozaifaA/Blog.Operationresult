using OperationResult;
using WebApi.Models;

namespace WebApi.Repositories
{
    public interface ICityRepository
    {
        OperationResult<City> GetById(int id);
    }

    public class CityRepository : ICityRepository
    {
        List<City> cities;
        public CityRepository()
        {
            cities = Enumerable.Range(1, 10).Select(index=>new City() {
            Id = index,
            Name = $"City NO{index}"
            }).ToList();
        }

        public OperationResult<City> GetById(int id)
        {
            OperationResult<City> operation = new OperationResult<City>();
            try
            {

                if (id > 100_000)
                    throw new IndexOutOfRangeException($"id:{id} out of range 100,000");

                if (id >= 100 && id <= 1000)
                {
                    operation.OperationResultType = OperationResultTypes.Failed;
                    operation.Message = $"Failed to get data has id between 100 and 1000";
                    return operation;
                }


                var result = cities.Where(city => city.Id == id).FirstOrDefault();

                if(result is null)
                {
                    operation.OperationResultType = OperationResultTypes.NotExist;
                    operation.Message = $"CityId {id} not found";
                    return operation;
                }


                operation.OperationResultType = OperationResultTypes.Success;
                operation.Message = $"GetById succeeded";
                operation.Result = result;
                return operation;
            }
            catch (Exception e)
            {
                operation.Exception = e;
                operation.OperationResultType = OperationResultTypes.Exception;
                operation.Message = "error";
                return operation;
            }
        }
    }
}
