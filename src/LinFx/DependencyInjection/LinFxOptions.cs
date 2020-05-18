using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents all the options you can use to configure the identity system.
    /// </summary>
    public class LinFxOptions : IOptions<LinFxOptions>
    {
        LinFxOptions IOptions<LinFxOptions>.Value { get { return this; } }
    }
}