using System.ComponentModel.DataAnnotations;

namespace CarTest.Attributes;

public class ValidateImageFileAttribute : ValidationAttribute
{
    private readonly string[] _validExtensions = { ".jpg", ".png", ".jpeg", ".gif" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);

            if (!_validExtensions.Contains(extension.ToLower()))
            {
                return new ValidationResult("Please upload a valid image file (JPG, JPEG, PNG or GIF)");
            }
        }

        return ValidationResult.Success;
    }
}