using GPTAPIS;
using GPTAPIS.Endpoints;
using GPTAPIS.Net.Api;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
// Использование клиента
public class Program
{
    public static async Task Main(string[] args)
    {

        //var client = ClientCreator.GetCredentialClient("46.149.77.121", "3128", new NetworkCredential("kikikaka", "pipapipa"));
        //var api = new APIService();
        //api.ApiKey = "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";
        //ChatEndpoint endpoint = new ChatEndpoint("https://api.openai.com/v1/chat/completions", client, api);
        //
        //var messages = new List<Message>() { new Message(Role.user, "Hello!")  };
        //
        //var req = new RequestForm(GPTAPIS.MessageConstruct.Model.GPT4_1106_Preview, messages);
        //
        //var response = await endpoint.SendRequestAsync(req);
        //
        //Console.WriteLine(response.Choices[0].Message.Content);
        //
        //Console.ReadLine();

        await OpenAiBehavior.test();

        Console.ReadLine();
    }
}