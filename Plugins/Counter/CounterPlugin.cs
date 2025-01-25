using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;

namespace Counter
{
    public class CounterPlugin : IPlugin
    {
        public static string _Id => "counter";
        public string Id => _Id;

        public PluginOutput Execute(PluginInput input)
        {
            var lastCount = 0;
            bool issucceed = int.TryParse(input.PersistentData, out lastCount);
            var result = (lastCount + 1).ToString();
            return new PluginOutput(result, result);
        }
    }
}
