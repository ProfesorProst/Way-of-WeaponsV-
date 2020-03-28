using System;
using wayofweapon.Entities;

namespace wayofweapon.View
{
    class AView
    {
        public String createNewUser { get; }// text for new user
        public String createNewUserEmptyUsername { get; }
        public String chooseStates { get; } // for make choise beetwen atack / defence
        public String upStatesOK { get; }     // when +1 attack/def and all is ok
        public String upStatesFalse { get; }     // when +1 attack/def and some thing wrong(dont have available point)
        public String setFractionSuccses { get; }
        public String setFractionFail { get; }
        public String chooseFraction;
        public String errorMessage { get; }

        public Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup keyboardStart { get; }
        public Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup keyboardLvlUp { get; }
        public Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup keyboardChouseFraction { get; }
        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardHome { get; }


        public AView()
        {
            keyboardHome = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[]
                                {
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton("\U0001F482 Hero"),
                                        new Telegram.Bot.Types.KeyboardButton("\U0001F528 Work \U0001F33E")
                                    },
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton("\U0001F3F0 Town"),
                                        new Telegram.Bot.Types.KeyboardButton(" Back ")
                                    },
                                },
                ResizeKeyboard = true
            };

            keyboardStart = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new Telegram.Bot.Types.InlineKeyboardButton[][]
            {
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton("a special Person","Person"),
                },
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton("a rich Gnome","Gnome"),
                },
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton("a terrible Orc","Orc"),
                },
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton("a strong Elf","Elf"),
                },
            });

            keyboardLvlUp = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new Telegram.Bot.Types.InlineKeyboardButton[][]
            {
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton(" Attack ","attack"),
                    new Telegram.Bot.Types.InlineKeyboardButton(" Defence ","def"),
                },
            });

            keyboardChouseFraction = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new Telegram.Bot.Types.InlineKeyboardButton[][]
{
                new []
                {
                    new Telegram.Bot.Types.InlineKeyboardButton(" Alliance ","Alliance"),
                    new Telegram.Bot.Types.InlineKeyboardButton(" Republic ","Republic"),
                },
});

            createNewUser = "Hello new Hero! We was waiting for you! \n" +
                "Please visit our group to have more info. https://t.me/WWGlobalChat \n" +
                " And chouse who you are ...";
            createNewUserEmptyUsername = "Please set your Username in the settings of Telegram!";
            chooseStates = "\U00002B50 Congratulation! You have become more powerful! And you stand stronger in ";
            upStatesOK = " You feel the power!";
            upStatesFalse = " You need to grow more...";
            chooseFraction = "Please, chose you fraction! Brave Republic or eternal Alliance?";
            setFractionSuccses = "We knew that you would choose the best!";
            setFractionFail = "Huston we have problems";
            errorMessage = "Gods dont understand what do you whant to do";
        }

        // All states of person
        public string States(Person person, int atackAdditional, int defAdditional)
        {
            DateTime date = DateTime.Now;
            TimeSpan timeSpan = person.energytime - date;

            string s = "<pre>\U0001F464 " + person.race + ":  " + person.personNick + "\n";
            if (person.fraction != null)
                s += (person.fraction == 1) ? "Citizen of the \U000026CE Republic" : "Citizen of the \U00003299 Alliance" + "\n";
            s += "\U0001F49A Health:       " + person.hp + "\n"
               + "\U000026A1 Energy:       " + person.energy + "/" + person.maxenergy + "\n";
            if (person.energy != person.maxenergy)
                s += "\U000023F3 to +1\U000026A1:     " + timeSpan.Minutes + " min\n";
            s += "\n" + "\U0001F4EF Lvl:          " + person.lvl + "\n"
                      + "\U0001F4A1 Experience:   " + person.exp + "/" + person.GetExpToNextLVL() + "\n"
                      + "\U0001F4B0 Gold:         " + person.gold + "\n" + "\n"
                      + "\U0001F6E1 Defence:      " + person.def + " + ( " + defAdditional + " )\n"
                      + "\U00002694 Attack:       " + person.atack + " + ( " + atackAdditional + " )</pre>";
            return s;
        }

        // When person go to "Work"
        public String[] Work(Person person, int oldGold, int oldExp, bool lvlUp)
        {
            String[] s = new String[3];
            if (person == null) { s[0] = "You lose this mision!"; return s; }
            string[] variants = { "You went to the mine and got some stones", "You went to the field and caught a big mouse",
                "You did not catch any fish, but on the way back found a wallet" };
            s[0] = variants[new Random().Next(0, variants.Length)] + "\n\n";
            s[0] += "\U000026A1 Energy:           " + person.energy + "/" + person.maxenergy + "\n"
                        + "Well done! Your work give you some gold and exp." + "\n"
                        + "\U0001F4B0 Gold earned: " + (person.gold - oldGold) + "\n"
                        + "\U0001F4A1 Experience earned: " + (person.exp - oldExp);
            if ((person.gold - oldGold) == 0 || (person.exp - oldExp) == 0)
                s[0] = "You dont have enough energy!";
            else if (lvlUp)
                s[1] = "\U00002B50 Congratulation! You have lvl up!";
            if (person.lvl == 5 && person.fraction != null)
                s[2] = "Please, chose you fraction!";
            return s;
        }
    }
}
