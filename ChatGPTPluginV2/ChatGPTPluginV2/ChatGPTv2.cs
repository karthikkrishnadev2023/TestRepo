using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using System.Text.Json;
using Newtonsoft.Json;

namespace ChatGPTPluginV2
{
    public class ChatPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingservice = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity Record = null;

            if (context.InputParameters.Contains("Target"))
            {
                Record = (Entity)context.InputParameters["Target"];
            }

            string question = "";
            if (Record.Contains("new_question") && Record["new_question"] != null)
            {
                question = (string)Record["new_question"];
            }

            string API_KEY = "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";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + API_KEY);
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json ");

                client.Timeout = TimeSpan.FromMilliseconds(15000); //15 seconds
                client.DefaultRequestHeaders.ConnectionClose = true; //Set KeepAlive to false

                var json = "{\"model\":\"text-davinci-003\",\"prompt\":\"" + question + "\",\"temperature\":0.9,\"max_tokens\":150,\"top_p\":1,\"frequency_penalty\":0.0,\"presence_penalty\":0.6,\"stop\":[\"Human:\",\"AI:\"]}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("https://api.openai.com/v1/completions", content).Result; //Make sure it is synchonrous
                response.EnsureSuccessStatusCode();

                var responseText = response.Content.ReadAsStringAsync().Result; //Make sure it is synchonrous
                tracingservice.Trace(responseText);
                
                Record["new_answer"] = responseText;
            }


        }
    }

    public class Response1
    {
        public string id { get; set; }
        public string _object { get; set; }
        public string created { get; set; }
        public string model { get; set; }
        public choices choices { get; set; }
        public usage usage { get; set; }
    }

    public class choices
    {
        public string text { get; set; }
        public string index { get; set; }
        public string logprobs { get; set; }
        public string finish_reason { get; set; }
    }

    public class usage
    {
        public string prompt_tokens { get; set; }
        public string completion_tokens { get; set; }
        public string total_tokens { get; set; }
    }
}
