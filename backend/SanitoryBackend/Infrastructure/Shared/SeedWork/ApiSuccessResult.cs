using System;
using Infrastructure.Shared.Paging;

namespace Infrastructure.Shared.SeedWork
{
    public class ApiSuccessResult<T> : ApiResult<T> where T : class
    {
        public ApiSuccessResult(PagedResult<T> data) :
            base(true, data, "Success")
        {
        }

        public ApiSuccessResult(T data) :
            base(true, data, "Success")
        {
        }

        public ApiSuccessResult(PagedResult<T> data, string message) :
            base(true, data, message)
        {
        }
    }
}
