using System.Collections.Generic;

namespace LinFx.Modules
{
    public class ModuleManager : IModuleConfigurationManager
    {
        const string MODULES_FILE_NAME = "modules.json";

        public virtual IEnumerable<ModuleInfo> GetModules()
        {
            var modules = new List<ModuleInfo>();
            //var modulesPath = Path.Combine(Global.ContentRootPath, MODULES_FILE_NAME);
            //using (var reader = new StreamReader(modulesPath))
            //{
            //    string content = reader.ReadToEnd();
            //    modules = JsonConvert.DeserializeObject<List<ModuleInfo>>(content);
            //}
            return modules;
        }
    }
}
