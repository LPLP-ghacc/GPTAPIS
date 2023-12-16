using System.Text.Json;
using System.Text.Json.Serialization;

namespace GPTAPIS.Saving;

public static class Profiling
{
    public static async Task ProfileSaveAsync(GPTAPISProfile profile, string savingPath)
    {
        try
        {
            using var stream = new FileStream(savingPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await JsonSerializer.SerializeAsync(stream, profile);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void ProfileSave(GPTAPISProfile profile, string savingPath)
    {
        var json = JsonSerializer.Serialize(profile);

        try
        {
            File.WriteAllText(savingPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
    }

    public static GPTAPISProfile? LoadProfile(string path)
    {
        GPTAPISProfile? profile = JsonSerializer.Deserialize<GPTAPISProfile>(path);

        if (profile == null)
            return null;

        return profile;
    }
}

public class GPTAPISProfile
{
    public GPTAPISProfile() { }

    public GPTAPISProfile(string apikey, string userName, string proxyUrl, string proxyPassword)
    {
        Apikey = apikey;
        UserName = userName;
        ProxyUrl = proxyUrl;
        ProxyPassword = proxyPassword;
    }

    [JsonPropertyName("apikey")]
    public string Apikey { get; set; }

    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [JsonPropertyName("proxyurl")]
    public string ProxyUrl { get; set; }

    [JsonPropertyName("proxypass")]
    public string ProxyPassword { get; set; }
}

