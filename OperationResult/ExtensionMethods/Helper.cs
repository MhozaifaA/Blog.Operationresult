using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult
{
    internal static class Helper
    {
        public static T NestedIF<T>(this bool value, T trueSide, T falseSide)
        => value ? trueSide : falseSide;

        public static string NestedIF(this bool value, string trueSide, string falseSide)
         => value.NestedIF<string>(trueSide, falseSide);

        public static bool IsNullOrEmpty(this string value)
         => System.String.IsNullOrEmpty(value);

        internal static string ToFullException(this System.Exception exception)
        {
            StringBuilder FullMessage = new StringBuilder();
            return Recursive(exception);
            //local function
            string Recursive(System.Exception deep)
            {
                FullMessage.Append(Environment.NewLine + deep.ToString() + Environment.NewLine + deep.Message);
                if (deep.InnerException is null) return FullMessage.ToString();
                return Recursive(deep.InnerException);
            }
        }
    }
}
