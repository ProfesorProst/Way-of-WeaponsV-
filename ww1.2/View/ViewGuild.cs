using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using wayofweapon.Entities;

namespace wayofweapon.View
{
    class ViewGuild
    {
        public string guildHello;
        public string createGuildOk;
        public string createGuildFalseName;
        public string createGuildFalseGold;
        public string createGuildFalseLvl;
        public string createGuild;
        public string workGuild;
        public string settingGuildMaster;
        public string changeHire;

        public string chatUrl;
        public string chatUrlSucces;
        public string chatUrlFail;

        public string invite;
        public string exclude;
        public string excludeSucces;
        public string excludeSuccesMessageToPlayer;
        public string inviteFaileByName;
        public string kikFailIMaster;
        public string inviteFaileByGuild;
        public string inviteSucces;
        public string inviteAccepted;
        public string leaveGuildSucces;

        public string doSomeThingWrong;

        public InlineKeyboardMarkup keyboardInvite { get; set; }
        public ReplyKeyboardMarkup keyboardCreateChoose { get; }
        public ReplyKeyboardMarkup keyboardGuild { get; set; }
        public ReplyKeyboardMarkup keyboardWorkGuild { get; }
        public ReplyKeyboardMarkup keyboardGuildMaster { get; }
        public ReplyKeyboardMarkup keyboardGuildMasterSetting { get; }

        public ViewGuild()
        {
            keyboardCreateChoose = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton("Create G"),
                        new Telegram.Bot.Types.KeyboardButton("Change G"),
                    },
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton(" Back "),
                    }
                },
                ResizeKeyboard = true
            };

            keyboardGuild = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton(" Chat G"),
                        new Telegram.Bot.Types.KeyboardButton(" Work G")
                    },
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton(" Guild G"),
                        new Telegram.Bot.Types.KeyboardButton(" Back ")
                    },
                },
                ResizeKeyboard = true
            };

            keyboardGuildMasterSetting = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton(" Invite G"),
                        new Telegram.Bot.Types.KeyboardButton(" Exclude G"),
                        new Telegram.Bot.Types.KeyboardButton(" Change chat G")
                    },
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton(" Change Hire status G"),
                        new Telegram.Bot.Types.KeyboardButton(" All members G"),
                        new Telegram.Bot.Types.KeyboardButton(" Guild G ")
                    },
                },
                ResizeKeyboard = true
            };


            keyboardWorkGuild = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton("1. Easy job( 2 \U000026A1) G"),
                        new Telegram.Bot.Types.KeyboardButton("2. Normal job( 3 \U000026A1) G")

                    },
                    new[]
                    {
                        new Telegram.Bot.Types.KeyboardButton("3. Hard job( 4 \U000026A1) G"),
                        new Telegram.Bot.Types.KeyboardButton (" Guild G ")
                    },
                },
                ResizeKeyboard = true
            };

            keyboardInvite = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new Telegram.Bot.Types.InlineKeyboardButton[][]
            {
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton(" Accept ","Accept G"),
                    new Telegram.Bot.Types.InlineKeyboardButton(" No ","No G")
                }
            });

            guildHello = "You are enter a guild house";
            workGuild = "You see on the notice board some jobs: ";
            createGuild = "Write a name of the guild after command /GCreate_. For example: /GCreate_GuildName. You need 1000 gold and 15 lvl";
            createGuildOk = "You just create a new guild. Guildmaster!";
            createGuildFalseName = " Some things go wrong. Maybe this name is used";
            createGuildFalseGold = " Some things go wrong. You are too poor to be a guildmaster";
            createGuildFalseLvl = " Some things go wrong. You are too young to be a guildmaster";

            settingGuildMaster = "What do you want to change guild master?";
            changeHire = "You change hire statys";

            chatUrl = "Plese write comand /GSetChatUrl_ and  your invite link (without https://t.me/)";
            chatUrlSucces = "All right. You changed succesfuly";
            chatUrlFail = "Some thing gone wrong";

            invite = "Write a name of the person after command /GInvite_. For example: /GInvite_PersonTest";
            exclude = "Write a name of the person after command /GExlude_. For example: /GExlude_PersonTest";
            excludeSucces = "Exclude succes";
            excludeSuccesMessageToPlayer = "You was exclude from guild!";
            kikFailIMaster = "You cant kik your self!";
            inviteFaileByName = "We dont have such player!";
            inviteFaileByGuild = "This player is already a member of the guild!";
            inviteSucces = "We send invite message";
            inviteAccepted = "Player accept you invite!";

            leaveGuildSucces = "You leave this guild. So now try to find better";

            doSomeThingWrong = "I think you do some thing wrong!";
        }

        // true - master; false - player
        public ReplyKeyboardMarkup GetGuldReplyKeyboard(bool masterOrPlayer)
        {
            DateTime localDate = DateTime.Now;
            DateTime startReadyWar = new DateTime(2018, 9, 9, 18, 50, 0);
            DateTime endReadyWar = new DateTime(2018, 9, 9, 19, 0, 0);
            if (!(localDate.Hour >= startReadyWar.Hour && localDate.Hour < endReadyWar.Hour && localDate.Minute >= startReadyWar.Minute))
                switch (masterOrPlayer)
                {
                    case false:
                        {
                            keyboardGuild = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                            {
                                Keyboard = new[]
                                {
                            new[]
                            {
                                new Telegram.Bot.Types.KeyboardButton(" Chat G"),
                                new Telegram.Bot.Types.KeyboardButton(" Work G")
                            },
                            new[]
                            {
                                new Telegram.Bot.Types.KeyboardButton(" Guild G "),
                                new Telegram.Bot.Types.KeyboardButton(" Back ")
                            },
                        },
                                ResizeKeyboard = true
                            };
                            return keyboardGuild;
                        }
                    case true:
                        {
                            return new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                            {
                                Keyboard = new[]
                                {
                            new[]
                            {
                                new Telegram.Bot.Types.KeyboardButton(" Setting G"),
                                new Telegram.Bot.Types.KeyboardButton(" Chat G"),
                                new Telegram.Bot.Types.KeyboardButton(" Work G")
                            },
                            new[]
                            {
                                new Telegram.Bot.Types.KeyboardButton(" Guild G "),
                                new Telegram.Bot.Types.KeyboardButton(" Back ")
                            },
                        },
                                ResizeKeyboard = true
                            };
                        }
                }
            return keyboardGuild;
        }

        public string GetChatUrl(string chatUrl)
        {
            if (chatUrl == "") return "Your guild master havant set your chat yet";
            else
                return "Good luck";
        }

        public string GetMembersOfGuild(List<Person> people)
        {
            string allMembers = "";
            foreach (Person person in people)
                allMembers += "Name:" + person.personNick + "\n";
            if (allMembers == "") allMembers = "hmmm say it to the God";
            return allMembers;
        }

        public InlineKeyboardMarkup GetChatUrlKeyboard(string chatUrl)
        {
            if (chatUrl == "") return null;
            else
                return new InlineKeyboardMarkup
                {
                    InlineKeyboard = new[]
                    {
                        new[] {new InlineKeyboardButton {Text = "Chat", Url = "https://t.me/" + chatUrl } }
                    }
                };
        }

        public string GetGuild(Guild guild)
        {
            string s = "closed";
            if (guild.hire) s = "open";
            return " You are enter a guild house. \n" +
                "Name: " + guild.name + "\n" +
                "Master: @" + guild.master + "\n" +
                "Count of members: " + guild.count + "/" + guild.maxplayers + "\n" +
                "Gold in the treasury: " + guild.gold + "\n" +
                "Hiring people: " + s;
        }

        public String[] Work(Person person, int oldGold, int oldExp, bool lvlUp)
        {
            return new AView().Work(person, oldGold, oldExp, lvlUp);
        }

        public string GetMessageForInvite(Guild guild)
        {
            return "@" + guild.master + " want to hire you in guild " + guild.name + ". Please make you chouse)";
        }

        public InlineKeyboardMarkup InviteInlineKeyboard(Guild guild)
        {
            keyboardInvite = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new Telegram.Bot.Types.InlineKeyboardButton[][]
            {
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton(" Accept ","Accept " + guild.id),
                    new Telegram.Bot.Types.InlineKeyboardButton(" No ","No G")
                }
            });
            return keyboardInvite;
        }

        public string GetOpenGuilds(List<Guild> guilds)
        {
            string guldslist = "";
            Random rand = new Random((DateTime.Now.Millisecond + DateTime.Now.Minute));
            if (guilds == null) return "We dont have guilds. this is bad(";
            int? z = guilds.Count();
            if (z >= 5)
                for (int i = 0; i < 5; i++)
                {
                    Guild guild = guilds[rand.Next(guilds.Count)];
                    guldslist += "Name: " + guild.name + ". " + guild.count + "/" + guild.maxplayers + "\n" +
                        "/Gjoin_" + guild.id;
                }
            else if (z < 5)
                for (int i = 0; i < z; i++)
                {
                    Guild guild = guilds[i];
                    guldslist += "Name: " + guild.name + ". " + guild.count + "/" + guild.maxplayers + "\n" +
                        "/Gjoin_" + guild.id;
                }
            if (z == 0) guldslist = "We dont have guilds. this is bad(";
            return guldslist;
        }

    }
}
