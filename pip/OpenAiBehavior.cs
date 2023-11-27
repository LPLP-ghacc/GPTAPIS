using GPTAPIS.Endpoints;
using GPTAPIS.MessageConstruct;
using GPTAPIS.Net.Api;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GPTAPIS
{
    public class OpenAiBehavior
    {
        public static async Task<ChatResponse> test()
        {
            string apikey = "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";
            var api = new APIService();
            api.ApiKey = apikey;
            var client = ClientCreator.GetCredentialClient("46.149.77.121", "3128", new NetworkCredential("kikikaka", "pipapipa"));
            var address = "https://api.openai.com/v1/chat/completions";
            var a = new ChatEndpoint(address, client, api);

            List<Message> messages = new List<Message>
            {
                new Message(Role.user, "hello tell me about cats!")
            };

            var request = new RequestForm(messages, Model.GPT4_1106_Preview, null, null, null, null, null, null, null, true);

            Action<ChatResponse> handler = (res) =>
            {
                Console.Write(res.Choices[0].Delta.Content);
            };

            var result = await a.SendStreamRequestAsync(request, handler);

            Console.WriteLine("\n \n \n" + result.Choices[0].Delta.Content);

            return result;
        }
    }
}
