using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WoT__1_DWP_.config
{
    internal class JsonReader
    {
        public string token {  get; set; }
        public string prefix { get; set; }
        public ulong channel_id { get; set; }


        public async Task ReadJsonConfig()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();

                JSONConfig config = JsonSerializer.Deserialize<JSONConfig>(json);

                this.token = config.token;
                this.prefix = config.prefix;
                this.channel_id = config.channel_id;
            }
        }
    }

    internal sealed class JSONConfig
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public ulong channel_id { get; set; }
    }
}
