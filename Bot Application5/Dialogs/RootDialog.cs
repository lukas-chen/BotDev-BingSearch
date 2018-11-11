using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Chatbotbingsearch.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

namespace Bot_Application5.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var news = await BingSearch(activity.Text);
            var reply = (context.Activity as Activity).CreateReply("查詢結果：");            List<ThumbnailCard> cards = new List<ThumbnailCard>();

            foreach (var item in news.Value)            {                var card = new ThumbnailCard()                {                    Title = item.Name,                    Text = item.Description,                    Buttons = new List<CardAction>                    {                        BuildCardAction(item.Url)                    }                };                reply.Attachments.Add(card.ToAttachment());            }

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;            await new ConnectorClient(new Uri(reply.ServiceUrl)).Conversations.SendToConversationAsync(reply);

            context.Wait(MessageReceivedAsync);
        }

        private async Task<News> BingSearch(string q)
        {
            var result = new News();            string key = "f187e8855c824dd9bd2585af421beca1";            string baseurl = "https://api.cognitive.microsoft.com/bing/v7.0/news/search?";            baseurl += "q=" + q;            baseurl += "&count=3";

            using (var client = new HttpClient())            {                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);                string response = await client.GetStringAsync(baseurl);                result = JsonConvert.DeserializeObject<News>(response);            }

            return result;
        }

        private CardAction BuildCardAction(string url)        {            return new CardAction            {                Type = ActionTypes.OpenUrl,                Title = "詳情",                Value = url            };        }

    }
}