using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult
{
    public class OperationResult<T> : IResult<T>
    {
       
        public T Result { get; set; }

        public string Message { get; set; }

        public OperationResultTypes OperationResultType { get; set; }

        public Exception Exception { get; set; }


        /// <summary>
        /// Check <see cref="OperationResultTypes.Success"/>.
        /// </summary>
        public bool IsSuccess => OperationResultType == OperationResultTypes.Success;

        /// <summary>
        /// Check <see cref="OperationResultTypes.Exception"/>.
        /// </summary>
        public bool HasException => this.OperationResultType == OperationResultTypes.Exception;

    }
}
