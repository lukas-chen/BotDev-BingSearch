﻿using System;
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
            var reply = (context.Activity as Activity).CreateReply("查詢結果：");

            foreach (var item in news.Value)

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            context.Wait(MessageReceivedAsync);
        }

        private async Task<News> BingSearch(string q)
        {
            var result = new News();

            using (var client = new HttpClient())

            return result;
        }

        private CardAction BuildCardAction(string url)

    }
}