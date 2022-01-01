using OperationResult.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult
{
    public class OperationResult<T> : OperationResultBase, IResult<T>
    {
        public T Result { get; set; }
        public string Message { get; set; }
        public OperationResultTypes OperationResultType { get; set; }
        public bool IsSuccess => OperationResultType == OperationResultTypes.Success;
        public bool HasException => this.OperationResultType == OperationResultTypes.Exception;
        public Exception Exception { get; set; }
        public string FullExceptionMessage => Exception?.ToFullException();
        public bool HasCustomeStatusCode => StatusCode > 0;

        public OperationResult<T> SetSuccess(T result)
        {
            Result = result;
            OperationResultType = OperationResultTypes.Success;
            return this;
        }
        public OperationResult<T> SetSuccess(string message)
        {
            Message = message;
            OperationResultType = OperationResultTypes.Success;
            return this;
        }
        public OperationResult<T> SetSuccess(T result, string message)
        {
            Result = result;
            Message = message;
            OperationResultType = OperationResultTypes.Success;
            return this;
        }

        public OperationResult<T> SetFailed(string message, OperationResultTypes type = OperationResultTypes.Failed)
        {
            if (type != OperationResultTypes.Failed && type != OperationResultTypes.Forbidden && type != OperationResultTypes.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResult<T>)} take {type} should use with {OperationResultTypes.Failed}, {OperationResultTypes.Forbidden} or {OperationResultTypes.Unauthorized} .");

            Message = message;
            OperationResultType = type;
            return this;
        }

        public OperationResult<T> SetException(Exception exception)
        {
            Exception = exception;
            OperationResultType = OperationResultTypes.Exception;
            return this;
        }

        public OperationResult<T> SetContent(OperationResultTypes type, string message)
        {
            if (type != OperationResultTypes.Exist && type != OperationResultTypes.NotExist)
                throw new ArgumentException($"Directly  return {nameof(OperationResult<T>)} take {type} should use with {OperationResultTypes.Exist} or {OperationResultTypes.NotExist} .");

            Message = message;
            OperationResultType = type;
            return this;
        }

        public static implicit operator OperationResult<T>((OperationResultTypes type, string message) type_message)
        {
            if (type_message.type != OperationResultTypes.Exist && type_message.type != OperationResultTypes.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResult<T>)} take {type_message.type} should use with {OperationResultTypes.Exist} or {OperationResultTypes.NotExist} .");

            return new OperationResult<T>() { OperationResultType = type_message.type, Message = type_message.message };
        }

        public static implicit operator OperationResult<T>(T result)
        {
            return new OperationResult<T>().SetSuccess(result);
        }

        public static implicit operator OperationResult<T>((T result, string message) result_message)
        {
            return new OperationResult<T>().SetSuccess(result_message.result, result_message.message);
        }

    }
}
