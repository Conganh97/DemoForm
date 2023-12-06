using System;

namespace Infrastructure.Shared.SeedWork
{
    public class ApiErrorResult<T> : ApiResult<T> where T : class
    {
        public ApiErrorResult() :
            this("Something wrong happened. Please try again later")
        {
        }

        public ApiErrorResult(string message) :
            base(false, message)
        {
        }

        public ApiErrorResult(List<string> errors) :
            base(false)
        {
        }

        public List<string> Errors { set; get; }
    }
}
