using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Shared.Attributes
{
    public class AllowedSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        private readonly string _errorMessage;

        public AllowedSizeAttribute(
            string errorMessage,
            int maxFileSize
        )
        {
            _maxFileSize = maxFileSize;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult
      IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {

                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }
    }
}