using OperationResult.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult
{
    public sealed class _Operation
    {
        public static OperationResultBase Operation()
        {
            return new OperationResultBase();
        }

        public static OperationResult<T> Operation<T>()
        {
            return new OperationResult<T>();
        }

        public static OperationResult<T> SetSuccess<T>(T result)
        {
            return new OperationResult<T>().SetSuccess(result);
        }

        public static OperationResultBase SetSuccess(string message)
        {
            return new OperationResultBase() { Message = message, OperationResultType = OperationResultTypes.Success };
        }

        public static OperationResult<T> SetSuccess<T>(T result, string message)
        {
            return new OperationResult<T>().SetSuccess(result, message);
        }


        public static OperationResultBase SetFailed(string message, OperationResultTypes type = OperationResultTypes.Failed)
        {
            if (type != OperationResultTypes.Failed && type != OperationResultTypes.Forbidden && type != OperationResultTypes.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResultBase)} take {type} should use with {OperationResultTypes.Failed}, {OperationResultTypes.Forbidden} or {OperationResultTypes.Unauthorized} .");

            return new OperationResultBase() { Message = message, OperationResultType = type };
        }


        public static OperationResult<T> SetFailed<T>(string message, OperationResultTypes type = OperationResultTypes.Failed)
        {
            return new OperationResult<T>().SetFailed(message, type);
        }

        public static OperationResultBase SetException(Exception exception)
        {
            return new OperationResultBase() { Exception = exception, OperationResultType = OperationResultTypes.Exception };
        }

        public static OperationResult<T> SetException<T>(Exception exception)
        {
            return new OperationResult<T>().SetException(exception);
        }

        public static OperationResultBase SetContent(OperationResultTypes type, string message)
        {
            if (type != OperationResultTypes.Exist && type != OperationResultTypes.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResultBase)} take {type} should use with {OperationResultTypes.Exist} or {OperationResultTypes.NotExist} .");

            return new OperationResultBase() { OperationResultType = type, Message = message };
        }

        public static OperationResult<T> SetContent<T>(OperationResultTypes type, string message)
        {
            return new OperationResult<T>().SetContent(type, message);
        }

    }
}
