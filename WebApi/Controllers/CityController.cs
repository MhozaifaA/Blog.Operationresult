using Microsoft.AspNetCore.Mvc;
using OperationResult;
using OperationResult.ExtensionMethods;
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
            var result = cityRepository.GetById(id);
            return ToJsonResult(result);
        }


        [HttpGet]
        public IActionResult Get()
        {
            return cityRepository.Get().ToJsonResult();
        }



        [HttpGet]
        public IActionResult GetWithStatusCode()
        {
            return cityRepository.Get().WithStatusCode(422).ToJsonResult();
        }


        [HttpGet]
        public IActionResult GetOperationBody()
        {
            return cityRepository.Get().Into(o => o).ToJsonResult();
        }

        [HttpGet]
        public IActionResult MultiGet()
        {
            return cityRepository.Get().Collect(cityRepository.Get_use__Operation(),
                cityRepository.Get_use_Implicit())
                .Into((resut1, resut2, resut3) =>new {
                    data1 = resut1.Result,
                    data2 = resut2.Result,
                    data3 = resut3.Result,
                }).ToJsonResult();
        }


        [HttpGet]

        // Take ~3 sec and return success
        public async Task<IActionResult> GetAsync()
        {
            return await cityRepository.Get_4_Async().ToJsonResultAsync();
        }

        [HttpGet]

        //  Take ~4  sec and return exception status
        public async Task<IActionResult> MultiGetAsync()
        {
            return await cityRepository.Get_4_Async().CollectAsync(
                cityRepository.Get_2_Async(),
                cityRepository.Get_1_Async()
                ).IntoAsync((resut1, resut2, resut3) => new {
                    data1 = resut1.Result,
                    data2 = resut2.Result,
                    data3 = resut3.Result,
                }).ToJsonResultAsync();
        }

        [HttpGet]

        //  Take ~7  sec and return exception status
        public async Task<IActionResult> _MultiGetAsync()
        {
            return (await cityRepository.Get_4_Async()).Collect(
                await cityRepository.Get_2_Async(),
                await cityRepository.Get_1_Async()
                ).Into((resut1, resut2, resut3) => new {
                    data1 = resut1.Result,
                    data2 = resut2.Result,
                    data3 = resut3.Result,
                }).ToJsonResult();
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