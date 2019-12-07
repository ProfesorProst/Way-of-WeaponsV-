using System.Collections.Generic;
using System.Linq;

namespace revcom_bot
{
    class ViewTown
    {
        public string goToTown;
        public string goToShop;
        public string goToTavern;
        public string goToTavernDrinkOk;
        public string goToTavernDrinkOkAndFight;
        public string goToTavernDrinkFail;
        public string goToTavernPlay;
        public string goToTavernPlayWin;
        public string goToTavernPlayLose;
        public string equipped;

        public string sellOk;
        public string sellBad;
        public string byOk;
        public string byBad;

        public string shop;
        public string weaponSwords;
        public string weaponBones;
        public string weaponMace;
        public string weaponAxe;
        public string armorHead;
        public string armorBody;
        public string armorHands; 
        public string armorLegs;

        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardTowne { get; }
        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardTavern { get; }
        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardShop { get; }
        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardShopArmor { get; }
        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardShopWeapon { get; }
        public Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup keyboardGuild { get; }

        public ViewTown()
        {
            keyboardTowne = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[]
                               {
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton("\U0001F3DB Tavern T"),
                                        new Telegram.Bot.Types.KeyboardButton("\U0001F4E6 Inventory"),
                                        new Telegram.Bot.Types.KeyboardButton(" Guild G")
                                    },
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton(" Shop S "),
                                        new Telegram.Bot.Types.KeyboardButton(" Back ")
                                    },
                                },
                ResizeKeyboard = true
            };

            keyboardTavern = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[]
                   {
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton("Drink(-50 \U0001F4B0) T"),
                                        new Telegram.Bot.Types.KeyboardButton("Play a coin(+- 100 \U0001F4B0) T")
                                    },
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton("\U0001F3F0 Town")
                                    },
                                },
                ResizeKeyboard = true
            };

            //Shop keyboard
            {
                keyboardShop = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                {
                    Keyboard = new[]
                                    {
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton(" Armor S"),
                                        new Telegram.Bot.Types.KeyboardButton(" Weapon S")
                                    },
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton(" Back ")
                                    },
                                },
                    ResizeKeyboard = true
                };

                keyboardShopArmor = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                {
                    Keyboard = new[]
                        {
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton(" Head S"),
                                        new Telegram.Bot.Types.KeyboardButton(" Body S"),
                                        new Telegram.Bot.Types.KeyboardButton(" Hands S")
                                    },
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton(" Legs S"),
                                        new Telegram.Bot.Types.KeyboardButton(" Back ")
                                    },
                                },
                    ResizeKeyboard = true
                };

                keyboardShopWeapon = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                {
                    Keyboard = new[]
                        {
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton(" Swords S"),
                                        new Telegram.Bot.Types.KeyboardButton(" Bones S"),
                                        new Telegram.Bot.Types.KeyboardButton(" Axes S")
                                    },
                                    new[]
                                    {
                                        new Telegram.Bot.Types.KeyboardButton(" Maces S"),
                                        new Telegram.Bot.Types.KeyboardButton(" Back ")
                                    },
                                },
                    ResizeKeyboard = true
                };
            }

            goToTown = "It seems that the city is prosperous. \n ";
            goToShop = "You see two benches";
            goToTavern = "This is Tavern...";
            goToTavernDrinkOk = "Wow this honey fry is good!";
            goToTavernDrinkOkAndFight = "Oh my God! You made a fight! But even your penis appeared stronger and took away this loser wallet(+200 \U0001F4B0)";
            goToTavernDrinkFail = "You don't have enouf money. We dont want to see person like you!";
            goToTavernPlay = "Play a coin with the owner of the tavern";
            goToTavernPlayLose = "You lose";
            goToTavernPlayWin = "You win";

            equipped = "The thing is made like for you.";

            //shop string
            {
                shop = " - We are glad to see you at our store. \n" +
                    "What do you whant to by?";
                byOk = "Thank you! We will be glad to see you again.";
                byBad = "Hmmm.. It seems you do not have enough money. ";
                sellOk = "Ok. if you don't need this anymore I will take it";
                sellBad = "You want to cheat!";
            }


            // weapon
            {
                weaponSwords = "We have some swords:\n" +
                    "Sword(\U0001F6E1 1 | \U00002694 2) - 30 \n" +
                    "/Sby_1 \n" +
                    "Gladius(\U0001F6E1 1 | \U00002694 4) - 100 \n" +
                    "/Sby_2 \n" +
                    "Bell sword(\U0001F6E1 2 | \U00002694 8) - 350 \n" +
                    "/Sby_3 \n" +
                    "Two-handed sword(\U0001F6E1 5 | \U00002694 12) - 800 \n" +
                    "/Sby_4";
                weaponBones = "We have some bones:\n" +
                    "Recruit bow(\U0001F6E1 0 | \U00002694 2) - 20 \n" +
                    "/Sby_5 \n" +
                    "Reflex bow(\U0001F6E1 1 | \U00002694 4) - 100 \n" +
                    "/Sby_6 \n" +
                    "Longbow(\U0001F6E1 1 | \U00002694 9) - 350 \n" +
                    "/Sby_7 \n" +
                    "Composite bow(\U0001F6E1 2 | \U00002694 14) - 780 \n" +
                    "/Sby_8";
                weaponMace = "We have some maces:\n" +
                    "Mace(\U0001F6E1 1 | \U00002694 2) - 30 \n" +
                    "/Sby_9 \n" +
                    "Multicolored mace(\U0001F6E1 1 | \U00002694 4) - 100 \n" +
                    "/Sby_10 \n" +
                    "Divan(\U0001F6E1 2 | \U00002694 8) - 350 \n" +
                    "/Sby_11 \n" +
                    "Morgenstern(\U0001F6E1 5 | \U00002694 12) - 800 \n" +
                    "/Sby_12";
                weaponAxe = "We have some axes:\n" +
                    "Simple axe(\U0001F6E1 0 | \U00002694 3) - 30 \n" +
                    "/Sby_13 \n" +
                    "Battle axe(\U0001F6E1 0 | \U00002694 5) - 100 \n" +
                    "/Sby_14 \n" +
                    "Tomahawk(\U0001F6E1 1 | \U00002694 10) - 350 \n" +
                    "/Sby_15 \n" +
                    "War axe(\U0001F6E1 2 | \U00002694 15) - 800 \n" +
                    "/Sby_16";
            }

            //armor
            {
                armorHead = "We have some armor on head:\n" +
                    "Leater helmet(\U0001F6E1 2) - 30 \n" +
                    "/Sby_21 \n" +
                    "Steel helmet(\U0001F6E1 5) - 100 \n" +
                    "/Sby_22 \n" +
                    "Bundhugels(\U0001F6E1 8) - 350 \n" +
                    "/Sby_23 \n" +
                    "Plate Helmets(\U0001F6E1 12) - 800 \n" +
                    "/Sby_24";
                armorBody = "We have some armor on body:\n" +
                    "Mail(\U0001F6E1 3) - 40 \n" +
                    "/Sby_25 \n" +
                    "Iron breastpiece(\U0001F6E1 6) - 120 \n" +
                    "/Sby_26 \n" +
                    "Lamellar Armor(\U0001F6E1 10) - 380 \n" +
                    "/Sby_27 \n" +
                    "Metal plates(\U0001F6E1 15) - 900 \n" +
                    "/Sby_28";
                armorHands = "We have some hands:\n" +
                    "Leater Bracers(\U0001F6E1 1) - 30 \n" +
                    "/Sby_29 \n" +
                    "Steel Bracers(\U0001F6E1 3) - 100 \n" +
                    "/Sby_30 \n" +
                    "Lamellar Bracers(\U0001F6E1 7) - 350 \n" +
                    "/Sby_31 \n" +
                    "Plate bracers(\U0001F6E1 10) - 700 \n" +
                    "/Sby_32";
                armorLegs = "We have some legs:\n" +
                    "Leater leggings(\U0001F6E1 1) - 30 \n" +
                    "/Sby_33 \n" +
                    "Steel leggings(\U0001F6E1 3) - 100 \n" +
                    "/Sby_34 \n" +
                    "Lamellar leggings(\U0001F6E1 7) - 350 \n" +
                    "/Sby_35 \n" +
                    "Plate leggings(\U0001F6E1 10) - 700 \n" +
                    "/Sby_36";
            }
        }

        public string GetInventory(List<Item> items)
        {
            if (items.Count() == 0) return "Inventory is empty...";
            string inventory = "You have:\n";
            foreach(Item item in items)
            {
                inventory += item.name + " (\U0001F6E1 " + item.def + " | \U00002694 " + item.attack + ") - " + item.count;
                if (item.eqiup) inventory += " Eqiuped";
                inventory += "\n/Ssell_" + item.id + " | /Sequip_" + item.id + "\n";
            }
            inventory += "(if you want to sell it will cost twice as cheaply)";
            return inventory;
        }
    }
}
