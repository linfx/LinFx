using LinFx.Logging;

namespace LinFx.Modules
{
    public abstract class Module
    {
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
    }
}
