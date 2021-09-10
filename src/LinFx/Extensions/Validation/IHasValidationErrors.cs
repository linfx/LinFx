using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinFx.Extensions.Validation
{
    public interface IHasValidationErrors
    {
        IList<ValidationResult> ValidationErrors { get; }
    }
}