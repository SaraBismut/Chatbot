using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ListPlugin
{
    record PersistentDataStructure(List<string> List);

    public class ListPlugin : IPlugin
    {
        public static string _Id = "list";
        public string Id => _Id;

        public PluginOutput Execute(PluginInput input)
        {
            List<string> list = new();

            if (string.IsNullOrEmpty(input.PersistentData) == false)
            {
                list = JsonSerializer.Deserialize<PersistentDataStructure>(input.PersistentData).List;
            }
            if (input.Message == "")
            {
                input.Callbacks.StartSession();
                return new PluginOutput("List started. Enter 'Add' to add task. Enter 'Delete' to delete task. Enter 'List' to view all list. Enter 'Exit' to stop.", input.PersistentData);
            }
            else if (input.Message.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                input.Callbacks.EndSession();
                return new PluginOutput("List stopped.", input.PersistentData);
            }
            else if (input.Message.StartsWith("add", StringComparison.OrdinalIgnoreCase))
            {
                var str = input.Message.Substring("add".Length).Trim();
                list.Add(str);
                var data = new PersistentDataStructure(list);
                return new PluginOutput($"New task: {str}", JsonSerializer.Serialize(data));
            }
            else if (input.Message.StartsWith("delete", StringComparison.OrdinalIgnoreCase))
            {
                var str=input.Message.Substring("delete".Length).Trim();
                if (string.IsNullOrEmpty(str))
                {
                    if (list.Count > 0)
                    {
                        string lastItem = list[list.Count - 1];
                        list.RemoveAt(list.Count - 1);
                        var data = new PersistentDataStructure(list);
                        return new PluginOutput($"Deleted task: {lastItem}", JsonSerializer.Serialize(data));
                    }
                    else
                    {
                        return new PluginOutput("No task to delete.");
                    }
                }
                else
                {
                    if (list.Remove(str))
                    {
                        var data = new PersistentDataStructure(list);
                        return new PluginOutput($"Deleted task: {str}", JsonSerializer.Serialize(data));
                    }
                    else
                    {
                        return new PluginOutput($"Task not found: {str}");
                    }
                }
            }
            else if (input.Message == "list")
            {
                string listtasks = string.Join("\r\n", list);
                return new PluginOutput($"All list tasks:\r\n{listtasks}", input.PersistentData);
            }
            else
            {
                return new PluginOutput("Error! Enter 'Add' to add task. Enter 'Delete' to delete task. Enter 'List' to view all list. Enter 'Exit' to stop.");
            }
        }
    }
}