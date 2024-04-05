using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace COMMAND {
    public class CMD_DatabaseExtension_VisualNovel : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("setplayername", new Action<string>(SetPlayerNameVariable));
            database.AddCommand("setplayermoney", new Action<int>(SetPlayerMoneyVariable));
            database.AddCommand("sethanakopoints", new Action<float>(SetHanakoPoints));
            database.AddCommand("setUIactive", new Action<bool>(SetUIActive));
        }

        private static void SetPlayerNameVariable(string data)
        {
            VISUALNOVEL.VNGameSave.activeFile.playerName = data;
        }

        private static void SetPlayerMoneyVariable(int data)
        {
            VISUALNOVEL.VNGameSave.activeFile.playerMoney = data;
        }

        private static void SetHanakoPoints(float data)
        {
            JournalPage.pointsHanako = data;
        }

        private static void SetUIActive(bool data)
        {
            if (data == true)
            {
                DialogueSystem.cgController.Show();
            }
            if(data == false)
            {
                DialogueSystem.cgController.Hide();
            }
        }
    }
}
