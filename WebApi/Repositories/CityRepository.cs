using OperationResult;
using WebApi.Models;

namespace WebApi.Repositories
{
    public interface ICityRepository
    {
        OperationResult<List<City>> Get();
        OperationResult<City> GetById(int id);
        Task<OperationResult<object>> Get_1_Async();
        Task<OperationResult<object>> Get_2_Async();
        Task<OperationResult<object>> Get_4_Async();
        OperationResult<List<City>> Get_use_Implicit();
        OperationResult<List<City>> Get_use__Operation();
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



        public async Task<OperationResult<object>> Get_2_Async()
        {
            await Task.Delay(2000);
            return _Operation.SetException(new NullReferenceException("this is exception"));
        }

        public async Task<OperationResult<object>> Get_4_Async()
        {
            await Task.Delay(4000);
            return new object();
        }

        public async Task<OperationResult<object>> Get_1_Async()
        {
            await Task.Delay(1000);
            return OperationResultTypes.Exist;
        }


        public OperationResult<List<City>> Get()
        {
            OperationResult<List<City>> operation = new();
            if (cities is null)
                return operation.SetFailed("cities are null");

            if (cities.Count == 0)
                return operation.SetContent(OperationResultTypes.NotExist, "cities are empty");

            return operation.SetSuccess(cities);
        }

        public OperationResult<List<City>> Get_use__Operation()
        {
            if (cities is null)
                return _Operation.SetFailed<List<City>>("cities are null");

            if(cities.Count==0)
                return _Operation.SetContent<List<City>>(OperationResultTypes.NotExist, "cities are empty");

            return _Operation.SetSuccess(cities);
        }

        public OperationResult<List<City>> Get_use_Implicit()
        {
            if (cities is null)
                return ("cities are null", OperationResultTypes.Failed);

            if (cities.Count == 0)
                return (OperationResultTypes.NotExist, "cities are empty");

            return cities;
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
