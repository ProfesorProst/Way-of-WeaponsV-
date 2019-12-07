using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace revcom_bot
{
    class ControlerSender
    {
        ModelPerson _modelPerson;
        ModelGuild modelGuild;
        ModelTavern modelTavern;
        View view;
        ViewTown viewTown;
        ViewGuild viewGuild;
        public Telegram.Bot.TelegramBotClient bot;

        public ControlerSender(Telegram.Bot.TelegramBotClient bot)
        {
            _modelPerson = new ModelPerson();
            modelGuild = new ModelGuild();
            modelTavern = new ModelTavern();
            view = new View();
            viewTown = new ViewTown();
            viewGuild = new ViewGuild();  
            this.bot = bot;
        }

        public void TextMessage(long userId, Telegram.Bot.Types.Message message)
        {
            if (message.Text == "/start")
                SendOneInlineMessage(userId, view.createNewUser, view.keyboardStart);
            else
            if (_modelPerson.TryIfExist(userId))
            {
                switch (message.Text)
                {
                    case "/me":
                    case "\U0001F482 Hero":
                    case "Back":
                        {
                            Person person = _modelPerson.GetMe(userId);
                            String s1 = view.States(person);
                            SendOneMessage(userId, s1, view.keyboardHome);
                            if (person.lvl >= 5 && person.fraction == null)
                                SendOneInlineMessage(userId, view.chooseFraction, view.keyboardChouseFraction);
                        }
                        break;
                    case "/work":
                    case "\U0001F528 Work \U0001F33E":
                        {
                            Person person = _modelPerson.Work(userId, out int goldOld, out int expOld, out bool lvlUp);
                            String[] work = view.Work(person, goldOld, expOld, lvlUp);
                            SendOneMessage(userId, work[0], view.keyboardHome);
                            if (work[1] != null)
                                SendOneInlineMessage(userId, view.chooseStates, view.keyboardLvlUp);
                        }
                        break;
                    case "/town":
                    case "\U0001F3F0 Town":
                        {
                            SendOneMessage(userId, viewTown.goToTown, viewTown.keyboardTowne);
                        }
                        break;
                    case var text1T when new Regex(@"T$").IsMatch(message.Text):
                    case var text2T when new Regex(@"^/T").IsMatch(message.Text):
                        Tavern(message.Text, userId);
                        break;
                    case var text1G when new Regex(@"^/G").IsMatch(message.Text):
                    case var text2G when new Regex(@"G$").IsMatch(message.Text):
                        Guild(message.Text, userId);
                        break;
                    case var text1s when new Regex(@"^/S").IsMatch(message.Text):
                    case var text2s when new Regex(@"S$").IsMatch(message.Text):
                        Shop(message.Text, userId);
                        break;
                    case "\U0001F4E6 Inventory":
                        string z = viewTown.GetInventory(_modelPerson.GetInventory(userId));
                        SendOneMessage(userId, z, viewTown.keyboardTowne);
                        break;

                    case var t when new Regex(@"^/M").IsMatch(message.Text):
                        {
                            if (userId == 413595040) SendMessagAll(t.Substring(2));
                        }
                        break;

                    default:
                        SendOneMessage(userId, view.errorMessage, view.keyboardHome);
                        break;
                }
            }
            else SendOneMessage(userId, "What? I dont understand. Write /start", null);

        }

        public void CallbackQuery(long userId, string data, Telegram.Bot.Types.Message message)
        {
            String s1;
            if (_modelPerson.TryIfExist(userId))
            {
                if (data == "Elf" || data == "Orc" || data == "Person" || data == "Gnome")
                {
                    s1 = "Gods do not want you to change the race!";
                    SendOneMessage(userId, s1, view.keyboardHome);
                }
                else
                if (data == "attack" || data == "def")
                {
                    int attackOrDef = 0;
                    if (data == "def") attackOrDef = 1;
                    if (_modelPerson.LvlUp(userId, attackOrDef)) s1 = view.upStatesOK;
                    else s1 = view.upStatesFalse;
                    SendOneMessage(userId, s1, view.keyboardHome);
                }
                else
                if ((data == "Alliance" || data == "Republic") && _modelPerson.GetMe(userId).fraction == null)
                {
                    int allianceOrRepublic = 0;
                    if (data == "Republic") allianceOrRepublic = 1;
                    if (_modelPerson.SetFraction(userId, allianceOrRepublic)) s1 = view.setFractionSuccses;
                    else s1 = view.setFractionFail;
                    SendOneMessage(userId, s1, view.keyboardHome);
                }
                else
                if (new Regex(@"^Accept ").IsMatch(data))
                {
                    Guild guild = modelGuild.GuildJoinOut(userId, Convert.ToInt64(message.Text.Split(' ')[1]));
                    if (guild != null)
                    {
                        SendOneMessage(userId, viewGuild.GetGuild(guild), viewGuild.keyboardGuild);
                    }
                }
            }

            else
            if (data == "Elf" || data == "Orc" || data == "Person" || data == "Gnome")
            {
                if (message.Chat.Username == "" || message.Chat.Username == null) SendOneMessage(userId, view.createNewUserEmptyUsername, view.keyboardHome);
                else
                {
                    Person person = _modelPerson.CreateNewUser(userId, message.Chat.Username, data);
                    s1 = "You hav choosen " + data;
                    String s2 = view.States(person);
                    SendTwoMessages(userId, s1, s2, view.keyboardHome);
                }
            }
        }

        public void WarSend(Person personOld, Person personNew)
        {
            SendOneMessage(personOld.id, view.GetChangesAfterWar(personOld, personNew), view.keyboardHome);
        }

        private void Shop(string text, long userId)
        {
            switch (text)
            {
                case "Shop S":
                    SendOneMessage(userId, viewTown.goToShop, viewTown.keyboardShop);
                    break;
                case "Armor S":
                    SendOneMessage(userId, viewTown.shop, viewTown.keyboardShopArmor);
                    break;
                case "Weapon S":
                    SendOneMessage(userId, viewTown.shop, viewTown.keyboardShopWeapon);
                    break;
                case "Head S":
                    SendOneMessage(userId, viewTown.armorHead, viewTown.keyboardShopArmor);
                    break;
                case "Body S":
                    SendOneMessage(userId, viewTown.armorBody, viewTown.keyboardShopArmor);
                    break;
                case "Hands S":
                    SendOneMessage(userId, viewTown.armorHands, viewTown.keyboardShopArmor);
                    break;
                case "Legs S":
                    SendOneMessage(userId, viewTown.armorLegs, viewTown.keyboardShopArmor);
                    break;
                case "Swords S":
                    SendOneMessage(userId, viewTown.weaponSwords, viewTown.keyboardShopWeapon);
                    break;
                case "Bones S":
                    SendOneMessage(userId, viewTown.weaponBones, viewTown.keyboardShopWeapon);
                    break;
                case "Axes S":
                    SendOneMessage(userId, viewTown.weaponAxe, viewTown.keyboardShopWeapon);
                    break;
                case "Maces S":
                    SendOneMessage(userId, viewTown.weaponMace, viewTown.keyboardShopWeapon);
                    break;
                case var text1 when new Regex(@"^/Sby_\d{1,2}$").IsMatch(text):
                    if (_modelPerson.ByItem(userId, Convert.ToInt32(text.Split('_')[1])))
                        SendOneMessage(userId, viewTown.byOk, viewTown.keyboardShop);
                    else SendOneMessage(userId, viewTown.byBad, viewTown.keyboardShop);
                    break;
                case var text2 when new Regex(@"^/Ssell_\d{1,2}$").IsMatch(text):
                    if (_modelPerson.SellItem(userId, Convert.ToInt32(text.Split('_')[1])))
                        SendOneMessage(userId, viewTown.sellOk, viewTown.keyboardShop);
                    else SendOneMessage(userId, viewTown.sellBad, viewTown.keyboardTowne);
                    break;
                case var text3 when new Regex(@"^/Sequip_\d{1,2}$").IsMatch(text):
                    _modelPerson.EquipItem(userId, Convert.ToInt32(text.Split('_')[1]));
                    SendOneMessage(userId, viewTown.equipped, viewTown.keyboardTowne);
                    break;
            }
        }

        private void Guild(string text, long userId)
        {
            switch (text)
            {
                case "Guild G":
                case "/Gguild":
                    {
                        Guild guild = modelGuild.GuildGet(userId);
                        if (guild != null)
                        {
                            if (modelGuild.TryIfGuildMaster(userId))
                            {
                                SendOneMessage(userId, viewGuild.GetGuild(guild), viewGuild.GetGuldReplyKeyboard(true));
                            }
                            else SendOneMessage(userId, viewGuild.GetGuild(guild), viewGuild.GetGuldReplyKeyboard(false));
                        }
                        else
                        {
                            List<Guild> guilds = modelGuild.GetOpenGuild(userId);
                            SendOneMessage(userId, viewGuild.GetOpenGuilds(guilds), viewGuild.keyboardCreateChoose);
                        }
                    }
                    break;
                case "/Gwork":
                case "Work G":
                    {
                        SendOneMessage(userId, viewGuild.workGuild, viewGuild.keyboardWorkGuild);
                    }
                    break;
                case "Chat G":
                    {
                        String s = modelGuild.GetChatLink(userId);
                        SendOneInlineMessage(userId, viewGuild.GetChatUrl(s), viewGuild.GetChatUrlKeyboard(s));
                    }
                    break;
                case "War G":
                    SendOneMessage(userId, viewGuild.readyToWar, viewGuild.keyboardWar);
                    break;
                case "Attack G":
                case "Def G":
                    {
                        bool ifTimeIsRight;
                        if (text == "Attack G")
                            ifTimeIsRight = modelGuild.WarIsStarting(userId, true);
                        else ifTimeIsRight = modelGuild.WarIsStarting(userId, false);
                        if (ifTimeIsRight)
                            SendOneMessage(userId, viewGuild.readyToWarSuccess, view.keyboardHome);
                        else SendOneMessage(userId, viewGuild.doSomeThingWrong, view.keyboardHome);

                    }
                    break;
                case var text1 when new Regex(@"⚡\) G$").IsMatch(text):
                    {
                        Person person = modelGuild.Work(userId, Convert.ToInt32(text.Substring(0, 1)) + 1, out int goldOld, out int expOld, out bool lvlUp); // number start from 1, by it use 2 energy
                        String[] work = viewGuild.Work(person, goldOld, expOld, lvlUp);
                        SendOneMessage(userId, work[0], viewGuild.keyboardGuild);
                        if (work[1] != null) //if have lvl up
                            SendOneInlineMessage(userId, view.chooseStates, view.keyboardLvlUp);
                    }
                    break;
                case var text1 when modelGuild.TryIfGuildMaster(userId):
                    GuildMaster(text, userId);
                    break;
                case "Create G":
                    SendOneMessage(userId, viewGuild.createGuild, viewGuild.keyboardCreateChoose);
                    break;
                case var text2 when new Regex(@"^/GCreate_[a-z]{1,10}$", RegexOptions.IgnoreCase).IsMatch(text):
                    {
                        string nameGuild = text.Split('_')[1];
                        Guild guildNew = modelGuild.GuildCreate(userId, nameGuild, out bool lvlIsSmall, out bool notEnouthGold);
                        if (lvlIsSmall) nameGuild = viewGuild.createGuildFalseLvl;
                        else if(notEnouthGold) nameGuild = viewGuild.createGuildFalseGold;
                            else if (guildNew == null) nameGuild = viewGuild.createGuildFalseName;
                                else nameGuild = viewGuild.createGuildOk;
                        SendOneMessage(userId, nameGuild, view.keyboardHome);
                        break;
                    }
                case "Change G":
                case "/GChange":
                    List<Guild> guildsOpen = modelGuild.GetOpenGuild(userId);
                    SendOneMessage(userId, viewGuild.GetOpenGuilds(guildsOpen), viewGuild.keyboardCreateChoose);
                    break;
                case "/GLeave":
                    string guildLeave = viewGuild.doSomeThingWrong;
                    if (modelGuild.GuildLeave(userId)) guildLeave = viewGuild.leaveGuildSucces;
                    SendOneMessage(userId, guildLeave, view.keyboardHome);
                    break;
            }
        }

        private void GuildMaster(string text, long userId)
        { 
            switch (text)
            {
                case "/GInvite":
                case "Invite G":
                    SendOneMessage(userId, viewGuild.invite, viewGuild.GetGuldReplyKeyboard(true));
                    break;
                case "/GExclude":
                case "Exclude G":
                    SendOneMessage(userId, viewGuild.exclude, viewGuild.GetGuldReplyKeyboard(true));
                    break;
                case "Change chat G":
                    SendOneMessage(userId, viewGuild.chatUrl, viewGuild.GetGuldReplyKeyboard(true));
                    break;
                case var textInvite when new Regex(@"^/GInvite_", RegexOptions.IgnoreCase).IsMatch(text):
                    {
                        Guild guild = new revcom_bot.Guild();
                        Person person = modelGuild.InvitePerson(userId, text.Substring(9), out guild);
                        String stringForView = "";
                        if (person == null)
                        {
                            stringForView = viewGuild.inviteFaileByName;
                        }
                        else
                        if (person.guild != null)
                            stringForView = viewGuild.inviteFaileByGuild;
                        else
                        {
                            stringForView = viewGuild.inviteSucces;
                            SendOneInlineMessage(person.id, viewGuild.GetMessageForInvite(guild), viewGuild.keyboardInvite);
                        }
                        SendOneMessage(userId, stringForView, viewGuild.GetGuldReplyKeyboard(true));
                    }
                    break;
                case var textInvite when new Regex(@"^/GExlude_", RegexOptions.IgnoreCase).IsMatch(text):
                    {
                        Person person = modelGuild.ExcludePerson(userId, text.Substring(9), out bool gildMaster);
                        String stringForView = "";
                        if (person == null)
                            stringForView = (gildMaster)? "You cant kik your self!" : viewGuild.inviteFaileByName;
                        else 
                        {
                            stringForView = viewGuild.excludeSucces;
                            SendOneMessage(person.id, viewGuild.excludeSuccesMessageToPlayer, view.keyboardHome);
                        }
                        SendOneMessage(userId, stringForView, viewGuild.GetGuldReplyKeyboard(true));
                    }
                    break;
                case var textInvite when new Regex(@"^/GSetChatUrl_", RegexOptions.IgnoreCase).IsMatch(text):
                    {
                        String s = "";
                        if (modelGuild.SetChatLink(userId, text.Substring(13)))
                        {
                            s = viewGuild.chatUrlSucces;
                        }
                        else
                            s = viewGuild.chatUrlFail;
                        SendOneMessage(userId, s, viewGuild.GetGuldReplyKeyboard(true));
                    }
                    break;
                case "/GSetting":
                case "Setting G":
                    SendOneMessage(userId, viewGuild.settingGuildMaster, viewGuild.keyboardGuildMasterSetting);
                    break;
                case "Change Hire status G":
                    {
                        modelGuild.ChangeHire(userId);
                        SendOneMessage(userId, viewGuild.changeHire, viewGuild.keyboardGuildMasterSetting);
                    }
                    break;
                case "All members G":
                    {
                        List<Person> people = modelGuild.GetMembers(userId); 
                        SendOneMessage(userId, viewGuild.GetMembersOfGuild(people), viewGuild.keyboardGuildMasterSetting);
                    }
                    break;
            }
        }

        private void Tavern(string text, long userId)
        {
            switch (text)
            {
                case "/tavern":
                case "\U0001F3DB Tavern T":
                    SendOneMessage(userId, viewTown.goToTavern, viewTown.keyboardTavern);
                    break;
                case "Drink(-50 \U0001F4B0) T":
                    {
                        String s = viewTown.goToTavernDrinkFail;
                        if (modelTavern.Drink(userId, out bool haveWinInFight)) s = viewTown.goToTavernDrinkOk;
                        SendOneMessage(userId, s, viewTown.keyboardTavern);
                        if (haveWinInFight) SendOneMessage(userId, viewTown.goToTavernDrinkOkAndFight, viewTown.keyboardTavern);
                    }
                    break;
                case "Play a coin(+- 100 \U0001F4B0) T":
                    {
                        String s = viewTown.goToTavernPlayLose;
                        bool state = modelTavern.FlipCoin(userId, out bool ifHaveEnoufMoney);
                        if (!ifHaveEnoufMoney) s = viewTown.goToTavernDrinkFail;
                        else
                        if (state) s = viewTown.goToTavernPlayWin;
                        SendTwoMessages(userId, viewTown.goToTavernPlay, s, viewTown.keyboardTavern);
                    }
                    break;
            }
        }

        private void SendMessagToMaster(String s)
        {
            SendOneMessage(413595040, s, null);
        }

        private void SendOneMessage(long id, String s, Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardMarkup)
        {
            try
            {
                bot.SendTextMessageAsync(id, s, true, true, 0, keyboardMarkup, Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) { }
        }

        private void SendTwoMessages(long id, String s1, String s2, Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardMarkup)
        {
            try
            {
                bot.SendTextMessageAsync(id, s1, true, true, 0, keyboardMarkup,  Telegram.Bot.Types.Enums.ParseMode.Html);
                bot.SendTextMessageAsync(id, s2, true, true, 0, keyboardMarkup, Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) { }
        }

        private void SendOneInlineMessage(long id, String s, Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup keyboardMarkup)
        {
            try
            {
                bot.SendTextMessageAsync(id, s, true, true, 0, keyboardMarkup, Telegram.Bot.Types.Enums.ParseMode.Default);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) { }
        }

        private void SendMessagAll(String s)
        {
            List<Person> people = new DaoPerson().GetObjects();
            foreach (Person person in people)
                SendOneMessage(person.id, s, view.keyboardHome);
        }
    }
}
