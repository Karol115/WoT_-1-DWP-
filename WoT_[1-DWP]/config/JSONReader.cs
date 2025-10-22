using System.Text.Json;

namespace WoT__1_DWP_.config
{
    internal class JsonReader
    {
        public string token {  get; set; }
        public string prefix { get; set; }
        public ulong channelId { get; set; }
        public string[] ignoredRoles { get; set; }

        public async Task ReadJsonConfig()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();

                JSONConfig config = JsonSerializer.Deserialize<JSONConfig>(json);

                this.token = config.token;
                this.prefix = config.prefix;
                this.channelId = config.channel_id;
                this.ignoredRoles = config.ignored_roles;
            }
        }
    }

    internal sealed class JSONConfig
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public ulong channel_id { get; set; }
        public string[] ignored_roles { get; set; }
    }
}
