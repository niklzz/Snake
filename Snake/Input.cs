﻿using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    class Input
    {
        private static Hashtable keyTable = new Hashtable(); //получаем лист доступных клавиш клавы

        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {

                return false;
            }
            return (bool) keyTable[key];
        }

        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;

        }

    }
}
