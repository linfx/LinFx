namespace LinFx.Extensions.Modularity
{
    public class MissingModuleManifestException : Exception
    {
        public string ModuleName { get; } = string.Empty;

        public MissingModuleManifestException() { }

        public MissingModuleManifestException(string message)
            : base(message) { }

        public MissingModuleManifestException(string message, string moduleName)
            : this(message)
        {
            ModuleName = moduleName;
        }
    }
}