using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.Modeling;

namespace LinFx.Extensions.AuditLogging.EntityFrameworkCore
{
    public class AuditLoggingModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public AuditLoggingModelBuilderConfigurationOptions([NotNull] string tablePrefix, [CanBeNull] string schema)
            : base(tablePrefix, schema)
        {
        }
    }
}