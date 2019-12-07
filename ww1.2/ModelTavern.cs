﻿using System;

namespace revcom_bot
{
    class ModelTavern
    {
        Person person;
        ModelPerson _modelPerson;

        const int flipCoinCost = 50;
        const int chanseToWinFightWhenDrinkPersents = 15;
        const int costOfDrink = 50;
        const int goldFromWinningFightWhenDrink = 200;

        public ModelTavern()
        {
            _modelPerson = new ModelPerson();
        }

        public bool FlipCoin(long userId, out bool ifHaveEnoufMoney)
        {
            Random random = new Random();
            person = _modelPerson.GetMe(userId);
            ifHaveEnoufMoney = true;
            if (person.gold < flipCoinCost)
            {
                ifHaveEnoufMoney = false;
                return false;
            }

            if (new Random().Next(0, flipCoinCost) > (flipCoinCost / 2))
            {
                person.gold -= flipCoinCost;
                _modelPerson.Update(person);
                return false;
            }
            else
            {
                person.gold += flipCoinCost;
                _modelPerson.Update(person);
                return true;
            }
        }

        public bool Drink(long userId, out bool haveWinInFight)
        {
            haveWinInFight = (new Random().Next(0, 100) < chanseToWinFightWhenDrinkPersents) ? true : false;
            Person person = _modelPerson.GetMe(userId);
            if (person.gold >= costOfDrink)
            {
                person.gold -= costOfDrink;
                _modelPerson.Update(person);
            }
            else
                return false;
            if (haveWinInFight)
            {
                person.gold += goldFromWinningFightWhenDrink;
                _modelPerson.Update(person);
            }
            return true;
        }
    }
}
