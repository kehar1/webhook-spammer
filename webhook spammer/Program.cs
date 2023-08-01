using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordWebhookApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("\r\n█▀▄ █ █▀ █▀▀ █▀█ █▀█ █▀▄   █░█░█ █▀▀ █▄▄ █░█ █▀█ █▀█ █▄▀   █▀ █▀█ ▄▀█ █▀▄▀█ █▀▄▀█ █▀▀ █▀█\r\n█▄▀ █ ▄█ █▄▄ █▄█ █▀▄ █▄▀   ▀▄▀▄▀ ██▄ █▄█ █▀█ █▄█ █▄█ █░█   ▄█ █▀▀ █▀█ █░▀░█ █░▀░█ ██▄ █▀▄\r\n\r\n█▀▄▀█ ▄▀█ █▀▄ █▀▀   █▄▄ █▄█   █▄▀ █▀▀ █░█ ▄▀█ █▀█ █\r\n█░▀░█ █▀█ █▄▀ ██▄   █▄█ ░█░   █░█ ██▄ █▀█ █▀█ █▀▄ █");

            Console.WriteLine("Enter your Discord webhook URL:");
            string webhookUrl = Console.ReadLine();

            Console.WriteLine("Do you want to send a message to Discord via webhook? (yes/no)");
            string sendMessageOption = Console.ReadLine().ToLower();

            if (sendMessageOption == "yes")
            {
                Console.WriteLine("Enter the message to send:");
                string messageToSend = Console.ReadLine();

                Console.WriteLine("How many times do you want to send the message? (1-100)");
                int messageCount = Math.Min(100, Math.Max(1, int.Parse(Console.ReadLine())));

                // Send messages to Discord
                await SendMessagesToDiscord(webhookUrl, messageToSend, messageCount);
            }

            Console.WriteLine("Do you want to delete the webhook? (yes/no)");
            string deleteWebhookOption = Console.ReadLine().ToLower();

            if (deleteWebhookOption == "yes")
            {
                // Delete the webhook
                await DeleteWebhook(webhookUrl);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static async Task SendMessagesToDiscord(string webhookUrl, string message, int messageCount)
        {
            var httpClient = new HttpClient();
            var random = new Random();
            var tasks = new List<Task>();

            for (int i = 1; i <= messageCount; i++)
            {
                var content = new { content = message };
                var json = JsonConvert.SerializeObject(content);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                tasks.Add(httpClient.PostAsync(webhookUrl, httpContent));

                // Add a delay between sending messages (optional)
                await Task.Delay(random.Next(50, 200));
            }

            await Task.WhenAll(tasks); // Wait for all messages to be sent

            Console.WriteLine($"Successfully sent {messageCount} messages to Discord!");
        }

        static async Task DeleteWebhook(string webhookUrl)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(webhookUrl);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Webhook deleted successfully!");
            }
            else
            {
                Console.WriteLine("Failed to delete the webhook. Make sure you have the correct URL.");
            }
        }
    }
}
