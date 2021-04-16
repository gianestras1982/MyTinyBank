using Microsoft.AspNetCore.Mvc;
using MyTinyBank.Core;

namespace MyTinyBank.Web.Extensions
{
    public static class ApiResultExtensions
    {
        public static ObjectResult ToActionResult<T>(this ApiResult<T> @this)
        {
            return new ObjectResult(@this.ErrorText) 
            {
                StatusCode = @this.Code
            };
        }
    }
}
