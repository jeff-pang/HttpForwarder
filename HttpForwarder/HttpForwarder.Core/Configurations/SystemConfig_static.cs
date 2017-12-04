using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HttpForwarder.Core.Configurations
{
    public partial class SystemConfig
    {
        static Dictionary<string, SystemConfig> _configs;

        static SystemConfig()
        {
            _configs = new Dictionary<string, SystemConfig>();
            string baseLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string[] files = Directory.GetFiles(baseLocation, $"*.jcfg");

            for (int x = 0; x < files.Length; x++)
            {
                SystemConfig config = new SystemConfig(files[x]);
                _configs.Add(config._name, config);
            }
        }

        public static T GetConfig<T>(string name)
        {
            if (_configs.ContainsKey(name))
            {
                return _configs[name].Cast<T>();
            }

            return default(T);
        }
    }
}
