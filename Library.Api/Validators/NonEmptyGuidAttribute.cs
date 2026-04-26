using System.ComponentModel.DataAnnotations;

namespace Library.Api.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NonEmptyGuidAttribute : ValidationAttribute
    {
        public NonEmptyGuidAttribute() : base("The {0} field must not be an empty GUID.") { }

        public override bool IsValid(object? value) => value is Guid g && g != Guid.Empty;
    }
}
