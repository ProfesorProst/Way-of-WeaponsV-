using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;
using Telegram.Bot.Types;
using Telegram.Bot;
using wayofweapon.Controler;

namespace wayofweapon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lets start a new journey!");
            ControlerReceiver controlerReceiver = new ControlerReceiver();
            Console.ReadKey();

        }
    }
}

    //https://api.telegram.org/bot650877497:AAE5famvf8Hdy6rv7t4v2wStj6Tnl7wLmLs/setWebhook?url=https://b84f412c.ngrok.io

        /*
    public static class Bot
    {
        public static readonly TelegramBotClient Api = new TelegramBotClient("533785870:AAEjN0SJJs02eMIO3rgL6IiUWhzz7-NMkeg"); //533785870:AAEjN0SJJs02eMIO3rgL6IiUWhzz7-NMkeg
    }                                                                                                                       // test 650877497:AAE5famvf8Hdy6rv7t4v2wStj6Tnl7wLmLs

    public static class Program
    {

        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8081/"))
            {

                Console.WriteLine("Port 8081. Write webhook URL = ");
                String s = Console.ReadLine() + "/webhook/";
                Console.WriteLine(s);
                try
                {
                    Bot.Api.SetWebhookAsync(s).Wait();
                    Console.WriteLine("Server Started.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                Console.ReadLine();
            }
        }
    }

    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();

            configuration.Routes.MapHttpRoute("WebHook", "{controller}");

            app.UseWebApi(configuration);
        }
    }



    public class WebHookController : ApiController
    {
        ControlerSender controlerSender = new ControlerSender(Bot.Api);

        public async Task<IHttpActionResult> Post(Update update)
        {
            try
            {
                if (!(update.Message != null || update.CallbackQuery != null || update.InlineQuery != null)) return Ok();
                if (update.Message != null && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && (update.CallbackQuery == null || update.InlineQuery == null))
                    controlerSender.TextMessage(update.Message.Chat.Id, update.Message);
                
                if (update.CallbackQuery != null || update.InlineQuery != null)
                    controlerSender.CallbackQuery(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Data, update.CallbackQuery.Message);
            }
            catch (Exception e) { }
            return Ok();
        }
    }
}

    */