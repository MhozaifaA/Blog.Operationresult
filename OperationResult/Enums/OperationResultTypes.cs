
namespace OperationResult
{
    public enum OperationResultTypes
    {
        Success = 200,
        Exist = 202,
        NotExist = 404,
        Failed = 400,
        Forbidden = 403,
        Exception = 500,
        Unauthorized = 401,
    }
}
