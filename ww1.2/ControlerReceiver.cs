using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Telegram.Bot;

namespace revcom_bot
{
    public partial class ControlerReceiver
    {
        BackgroundWorker bw;
        TelegramBotClient bot;
        static object locker = new object();
        ControlerSender controlerSender;

        public ControlerReceiver()
        {
            bot = new Telegram.Bot.TelegramBotClient("533785870:AAEjN0SJJs02eMIO3rgL6IiUWhzz7-NMkeg"); //533785870:AAEjN0SJJs02eMIO3rgL6IiUWhzz7-NMkeg
            controlerSender = new ControlerSender(bot);                                      // test 650877497:AAE5famvf8Hdy6rv7t4v2wStj6Tnl7wLmLs
            this.bw = new BackgroundWorker();
            this.bw.DoWork += bw_DoWork;
            this.bw.RunWorkerAsync();
            Thread myThread = new Thread(RunThreadToCheckWar);
            myThread.Start();
        }

        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                await bot.SetWebhookAsync("");

                bot.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) =>
                {
                    if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return; // в этом блоке нам келлбэки и инлайны не нужны
                    var update = evu.Update;
                    var message = update.Message;
                    if (message == null) return;
                    long userId = message.Chat.Id;
                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                        controlerSender.TextMessage(userId, message);
                };

                // Callback'и от кнопок
                bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                {
                    var message = ev.CallbackQuery.Message;
                    long userId = ev.CallbackQuery.Message.Chat.Id;
                    var data = ev.CallbackQuery.Data;

                    controlerSender.CallbackQuery(userId, data, message);
                };

                bot.StartReceiving();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
               
            }
        }

        private void RunThreadToCheckWar()
        {
            DateTime localDate = DateTime.Now;
            DateTime startReadyWar = new DateTime(2018, 9, 9, 19, 0, 0);
            DateTime endReadyWar = new DateTime(2018, 9, 9, 19, 15, 0);
            if ((localDate.Hour == startReadyWar.Hour && localDate.Minute >= startReadyWar.Minute && localDate.Minute <= endReadyWar.Minute))
            lock (locker)
            {
                    List<Person> peopleNew = new ModelWar().WarStart(out List<Person> peopleOld);
                    foreach (Person personOld in peopleOld)
                        foreach(Person personNew in peopleNew)
                            if(personOld.id == personNew.id && personOld.exp != personNew.exp)
                            {
                                controlerSender.WarSend(personOld, personNew);
                            }
            }
            Thread.Sleep(4500);
        }
    }
}