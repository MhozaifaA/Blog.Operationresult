using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OperationResult.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult.ExtensionMethods
{
    public static class OpertaionResultExtesnsion
    {

        public static OperationResult<T> ToOperationResult<T>(this T @object)
         => new OperationResult<T>().SetSuccess(@object);
     
        public static OperationResult<T> WithStatusCode<T>(this OperationResult<T> result, int statusCode)
        {
            result.StatusCode = statusCode;
            return result;
        }

        public static JsonResult ToJsonResult<T>(this OperationResult<T> result)
        {
            return result.OperationResultType switch
            {
                OperationResultTypes.Success => result.GetValidResult(result.HasCustomeStatusCode.NestedIF<int>(result.StatusCode ?? 0, StatusCodes.Status200OK), null, true),
                OperationResultTypes.Exist => result.GetValidResult(result.HasCustomeStatusCode.NestedIF<int>(result.StatusCode ?? 0, StatusCodes.Status202Accepted), result.Message),
                OperationResultTypes.NotExist => result.GetValidResult(result.HasCustomeStatusCode.NestedIF<int>(result.StatusCode ?? 0, StatusCodes.Status404NotFound), result.Message),
                OperationResultTypes.Failed => result.GetValidResult(result.HasCustomeStatusCode.NestedIF<int>(result.StatusCode ?? 0, StatusCodes.Status400BadRequest), result.Message),
                OperationResultTypes.Forbidden => result.GetValidResult(result.HasCustomeStatusCode.NestedIF<int>(result.StatusCode ?? 0, StatusCodes.Status403Forbidden), result.Message),
                OperationResultTypes.Unauthorized => result.GetValidResult(result.HasCustomeStatusCode.NestedIF<int>(result.StatusCode ?? 0, StatusCodes.Status401Unauthorized), result.Message),
                OperationResultTypes.Exception => result.GetValidResult(result.HasCustomeStatusCode.NestedIF<int>(result.StatusCode ?? 0, StatusCodes.Status500InternalServerError), result.FullExceptionMessage),
                _ => new JsonResult(string.Empty),
            };
        }

        public static async Task<JsonResult> ToJsonResultAsync<T>(this Task<OperationResult<T>> result) => (await result).ToJsonResult();

        private static JsonResult GetValidResult<T>(this OperationResult<T> result, int statusCode, string jsonMessage = null, bool json = false) =>
            json ? new JsonResult(result.Result) { StatusCode = statusCode } :
                   new JsonResult(jsonMessage.IsNullOrEmpty() ? result.OperationResultType.ToString() : jsonMessage) { StatusCode = statusCode };


        public static OperationResult<TResult1> Collect<TResult1>(this OperationResult<TResult1> result1)
        => (result1);

        public static OperationResult<TResult> Into<TResult1, TResult>(this OperationResult<TResult1> result1,
            Func<OperationResult<TResult1>, TResult> receiver)
        => result1.InOnce(receiver(result1));


        private static OperationResult<TResult> InOnce<TResult1, TResult>(this OperationResult<TResult1> result1, TResult result)
        => OnePriority(result1, result);

        public static async Task<OperationResult<TResult1>> CollectAsync<TResult1>(this Task<OperationResult<TResult1>> result1)
        {
            await Task.WhenAll(result1);
            return (await result1);
        }

        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult>(this Task<OperationResult<TResult1>> result1,
            Func<OperationResult<TResult1>, TResult> receiver)
        => await result1.InOnceAsync(receiver(await result1));


       
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult>(this Task<OperationResult<TResult1>> result1, TResult result)
        => OnePriority(await result1, result);

        public static (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3) Collect<TResult1, TResult2, TResult3>(this OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)
        => (result1, result2, result3);

        public static OperationResult<TResult> Into<TResult1, TResult2, TResult3, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3) results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, TResult> receiver)
        => results.InOnce(receiver(results.result1, results.result2, results.result3));
       
        private static OperationResult<TResult> InOnce<TResult1, TResult2, TResult3, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3) results, TResult result)
        => OncePriority(results, result);

        public static async Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)> CollectAsync<TResult1, TResult2, TResult3>(this Task<OperationResult<TResult1>> result1, Task<OperationResult<TResult2>> result2, Task<OperationResult<TResult3>> result3)
        {
            await Task.WhenAll(result1, result2, result3);
            return (await result1, await result2, await result3);
        }
       
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult2, TResult3, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)> results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, TResult> receiver)
        { var (result1, result2, result3) = await results; return await results.InOnceAsync(receiver(result1, result2, result3)); }


        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult2, TResult3, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)> results, TResult result)
        => OncePriority(await results, result);

      
        private static OperationResult<TResult> OnePriority<TOneResult, TResult>(TOneResult oneResult, TResult result)
        {
            OperationResult<TResult> operation = new();

            OperationResultBase Result = oneResult as OperationResultBase;

            if (Result.OperationResultType == OperationResultTypes.Exception)
                return operation.SetException(Result.Exception);

            if (Result.OperationResultType == OperationResultTypes.Failed || Result.OperationResultType == OperationResultTypes.Forbidden || Result.OperationResultType == OperationResultTypes.Unauthorized)
                return operation.SetFailed(String.Join(",",
                    Result.Message.IsNullOrEmpty().NestedIF($"Result {1} not contain Message or Success", Result.Message)), Result.OperationResultType);

            return operation.SetSuccess(result, String.Join(",",
                  Result.Message.IsNullOrEmpty().NestedIF($"Result {1} not contain Message or Success", Result.Message)));

        }


        private static OperationResult<TResult> OncePriority<TTupleResult, TResult>(TTupleResult results, TResult result) where TTupleResult : ITuple
        {
            OperationResult<TResult> operation = new();

            IEnumerable<OperationResultBase> listResult = Enumerable.Repeat(0, results.Length).Select(index => results[index]).Cast<OperationResultBase>();

            OperationResultBase firstException = listResult.FirstOrDefault(result => result.OperationResultType == OperationResultTypes.Exception);
            if (firstException != null)
                return operation.SetException(firstException.Exception);

            if (listResult.Any(result => result.OperationResultType == OperationResultTypes.Failed || result.OperationResultType == OperationResultTypes.Forbidden || result.OperationResultType == OperationResultTypes.Unauthorized))
                return operation.SetFailed(String.Join(",",
                    listResult.Select((result, iter) => result.Message.IsNullOrEmpty().NestedIF($"Result {iter} not contain Message or Success", result.Message))), listResult.Max(result => result.OperationResultType));

            return operation.SetSuccess(result, String.Join(",",
                    listResult.Select((result, iter) => result.Message.IsNullOrEmpty().NestedIF($"Result {iter} not contain Message or Success", result.Message))));

        }

        
    }
}
