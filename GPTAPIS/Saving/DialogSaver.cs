using GPTAPIS.MessageConstruct;
using GPTAPIS.Net.Api.Text;
using System.Text.Json;

namespace GPTAPIS;

public class DialogSaver
{
    private readonly string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public DialogSaver(string savingPath, APIService service)
    {
        this.SavingPath = savingPath;
        Service = service;
    }

    public DialogSaver(string savingPath, Action onResave, Action onSave, APIService service)
    {
        this.SavingPath = savingPath;
        OnResaved = onResave;
        OnSaved = onSave;
        Service = service;
    }

    public string SavingPath { get; set; }
    public Action? OnResaved { get; set; }
    public Action? OnSaved { get; set; }
    public APIService Service { get; set; }

    /// <summary>
    /// When specifying filename, you don't need to add ".json". 
    /// Make sure that the SavingPath has the form "../"
    /// </summary>
    public string? Saving(string filename)
    {
        List<Message> chat = Service.ChatEndpoint.Messages;
        string json = JsonSerializer.Serialize(chat);

        SavingPath = string.IsNullOrEmpty(SavingPath) ? documentsPath : SavingPath;
        string path = $"{SavingPath}{filename}.json";

        try
        {
            File.WriteAllText(path, json);

            return path;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());

            return null;
        }
    }

    /// <summary>
    /// The developer recommends using this method 
    /// if the user has no ideas for their own filename generator.<br/>
    /// The first sentence in the first message can be used as input.
    /// </summary>
    public static string MakeValidFileName(string input)
    {
        string invalidChars = new string(Path.GetInvalidFileNameChars());
        string replaceChar = "";

        foreach (char invalidChar in invalidChars)
        {
            input = input.Replace(invalidChar.ToString(), replaceChar);
        }

        int maxFileNameLength = 20;
        if (input.Length > maxFileNameLength)
        {
            input = input.Substring(0, maxFileNameLength);
        }

        return input;
    }
}
