using Microsoft.Extensions.Options;

namespace LinFx
{
    public class LinFxOptions : IOptions<LinFxOptions>
    {
        LinFxOptions IOptions<LinFxOptions>.Value { get { return this; } }
    }
}