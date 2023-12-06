using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Shared.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        private readonly string _errorMessage;

        public AllowedExtensionsAttribute(
            string errorMessage,
            string[] extensions
        )
        {
            _extensions = extensions;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult
        IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
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
