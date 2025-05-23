using MDIPaint;
using PluginInterface;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;

public static class PluginLoader
{
    public static List<PluginInfo> LoadPlugins(string configPath)
    {
        var plugins = new List<PluginInfo>();
        if (!File.Exists(configPath))
        {
            // Если файла нет — загружаем все DLL из папки
            foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
            {
                var info = TryLoadPlugin(file);
                if (info != null)
                {
                    info.Enabled = true;
                    plugins.Add(info);
                }
            }

            // и сохраняем как дефолтный конфиг
            var config = plugins.Select(p => new PluginConfig { AssemblyPath = Path.GetFileName(p.Path), Enabled = true }).ToList();
            File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));

            return plugins;
        }

        // иначе — читаем конфиг
        var raw = File.ReadAllText(configPath);
        var configList = JsonConvert.DeserializeObject<List<PluginConfig>>(raw);

        foreach (var config in configList)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.AssemblyPath);
            var info = TryLoadPlugin(path);
            if (info != null)
            {
                info.Enabled = config.Enabled;
                plugins.Add(info);
            }
        }

        return plugins;
    }

    public static void SavePluginConfig(List<PluginInfo> plugins, string configPath)
    {
        var config = plugins.Select(p => new PluginConfig
        {
            AssemblyPath = Path.GetFileName(p.Path),
            Enabled = p.Enabled
        }).ToList();

        File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
    }

    private static PluginInfo TryLoadPlugin(string filePath)
    {
        try
        {
            var asm = Assembly.LoadFrom(filePath);
            foreach (var type in asm.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(type))
                {
                    var rawInstance = Activator.CreateInstance(type);
                    var instance = rawInstance as IPlugin;
                    //var instance = (IPlugin)Activator.CreateInstance(type);
                    var attr = type.GetCustomAttribute<VersionAttribute>();
                    return new PluginInfo
                    {
                        Name = instance.Name,
                        Author = instance.Author,
                        Version = attr != null ? $"{attr.Major}.{attr.Minor}" : "N/A",
                        Path = filePath,
                        Enabled = true,
                        Instance = instance
                    };
                }
            }
        }
        catch { }

        return null;
    }
}
