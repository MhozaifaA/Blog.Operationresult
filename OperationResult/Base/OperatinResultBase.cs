using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult.Base
{
    public class OperationResultBase
    {
        public string Message { get; set; }
        public OperationResultTypes OperationResultType { get; set; }
        public Exception Exception { get; set; }
        public int? StatusCode { get; set; }
    }
}
