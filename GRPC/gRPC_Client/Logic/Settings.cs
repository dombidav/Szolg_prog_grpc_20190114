using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace gRPC_Client
{
    static class Settings
    {
        private const string configFile = "config.yaml";

        public static string ChannelProtocol { get; private set; }
        public static string ChannelHost { get; private set; }
        public static int ChannelPort { get; private set; }

        public static void Load()
        {
            if (File.Exists(configFile))
            {
                var file = File.ReadAllLines(configFile).Where(x=>!string.IsNullOrWhiteSpace(x) && !x.StartsWith("--")).Select(x=>x.Trim());
                foreach (var line in file)
                {
                    var rx = Regex.Match(line, @"^\t*(.+?) *\t*= *\t*(.+)");
                    switch (rx.Groups[1].Value.ToLower().Trim())
                    {
                        case "channelprotocol":
                            ChannelProtocol = rx.Groups[1].Value.Trim();
                            break;
                        case "channelhost":
                            ChannelHost = rx.Groups[1].Value.Trim();
                            break;
                        case "channelport":
                            if (int.TryParse(rx.Groups[1].Value.Trim(), out var x))
                                ChannelPort = x;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                ChannelProtocol = "https";
                ChannelHost = "localhost";
                ChannelPort = 5001;
                Save();
            }
        }

        public static void Save() 
            => File.WriteAllText(configFile, 
                    $"--Config {DateTime.Now}--\n" +
                    $"\tchannelprotocol = {ChannelProtocol}" +
                    $"\tchannelhost = {ChannelHost}" +
                    $"\tchannelport = {ChannelPort}"
                );

    }
}
