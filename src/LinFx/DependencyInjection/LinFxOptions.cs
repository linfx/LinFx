using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public class LinFxOptions : IOptions<LinFxOptions>
    {
        LinFxOptions IOptions<LinFxOptions>.Value { get { return this; } }
    }
}