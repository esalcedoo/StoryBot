using Bot.Builder.Community.Adapters.Alexa.Directives;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBot.Services.Alexa
{
    public static class ActivityAlexaExtensions
    {
        internal static IAlexaDirective ToAlexaDirective(this IMessageActivity activity)
        {
            List<ListItem> listItems = new List<ListItem>();
            ListItem listItem;

            IEnumerable<string> choices = activity.SuggestedActions?.Actions.Select(a => a.DisplayText);
            foreach (string choice in choices)
            {
                listItem = new ListItem()
                {
                    Token = choice,
                    TextContent = new TextContent()
                    {
                        PrimaryText = new InnerTextContent()
                        {
                            Text = choice,
                            Type = TextContentType.RichText
                        }
                    }
                };
                listItems.Add(listItem);
            }

            var directive = new DisplayDirective()
            {
                Template = new DisplayRenderListTemplate1()
                {
                    BackButton = BackButtonVisibility.HIDDEN,
                    Title = string.Empty,
                    Token = "string",
                    ListItems = listItems,
                    BackgroundImage = new Image()
                    {
                        ContentDescription = "background",
                        Sources = new ImageSource[]
                    {
                        new ImageSource()
                        {
                            Url = "https://storybotst.blob.core.windows.net/images/welcomeImage.jpg"
                        }
                    }
                    }
                }
            };

            return directive;
        }

    }
}
