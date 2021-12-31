using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CityController : Controller
    {
        private readonly ICityRepository cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
           var result =  cityRepository.GetById(id);
            return ToJsonResult(result);
        }


        private JsonResult ToJsonResult<T>(OperationResult<T> result)
        {
            switch (result.OperationResultType)
            {
                case OperationResultTypes.Success:
                   return new JsonResult(result.Result) { StatusCode= StatusCodes.Status200OK };
                case OperationResultTypes.Exist:
                    return new JsonResult(result.Message) { StatusCode = StatusCodes.Status202Accepted };
                case OperationResultTypes.NotExist:
                    return new JsonResult(result.Message) { StatusCode = StatusCodes.Status404NotFound };
                case OperationResultTypes.Failed:
                    return new JsonResult(result.Message) { StatusCode = StatusCodes.Status400BadRequest };
                case OperationResultTypes.Exception:
                    return new JsonResult(result.Exception.Message) { StatusCode = StatusCodes.Status500InternalServerError };
                default: 
                    throw new NotImplementedException();
            }
        }
    }
}