using System;
using Infrastructure.Shared.Paging;

namespace Infrastructure.Shared.SeedWork
{
    public class ApiResult<T> where T : class
    {
        public ApiResult()
        {
        }

        public ApiResult(bool isSucceeded, string message = null)
        {
            Message = message;
            IsSucceeded = isSucceeded;
        }

        public ApiResult(bool isSucceeded, T data, string message = null)
        {
            Data = data;
            Message = message;
            IsSucceeded = isSucceeded;
        }

        public ApiResult(
            bool isSucceeded,
            PagedResult<T> data,
            string message = null
        )
        {
            PagedData = data;
            Message = message;
            IsSucceeded = isSucceeded;
        }

        public bool IsSucceeded { get; set; }

        public string Message { get; set; }

        public PagedResult<T> PagedData { get; }

        public T Data { get; }
    }
}
