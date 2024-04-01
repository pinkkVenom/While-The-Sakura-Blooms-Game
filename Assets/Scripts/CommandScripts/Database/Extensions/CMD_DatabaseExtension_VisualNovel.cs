using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMAND {
    public class CMD_DatabaseExtension_VisualNovel : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("setplayername", new Action<string>(SetPlayerNameVariable));
            //database.AddCommand("setplayermoney", new Action<string>(SetPlayerMoneyVariable));
        }

        private static void SetPlayerNameVariable(string data)
        {
            VISUALNOVEL.VNGameSave.activeFile.playerName = data;
        }

        //private static void SetPlayerMoneyVariable(string data)
        //{
        //    VISUALNOVEL.VNGameSave.activeFile.playerMoney = data;
        //}
    }
}
