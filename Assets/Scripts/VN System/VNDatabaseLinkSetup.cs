using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace VISUALNOVEL
{
    public class VNDatabaseLinkSetup : MonoBehaviour
    {
        public void SetupExternalLinks()
        {
            VariableStore.CreateVariable("VN.mainCharName", "", () => VNGameSave.activeFile.playerName, value => VNGameSave.activeFile.playerName = value);
            VariableStore.CreateVariable("VN.money", 0, () => VNGameSave.activeFile.playerMoney, value => VNGameSave.activeFile.playerMoney = value);
            VariableStore.CreateVariable("VN.gameSaveIndex", 0, () => StoryManager.storyIndex, value => StoryManager.storyIndex = value);

            //Character Points
            VariableStore.CreateVariable("VN.kenjiPoints", 0.0f, () => JournalPage.pointsKenji, value => JournalPage.pointsKenji = value);
            VariableStore.CreateVariable("VN.hanakoPoints", 0.0f, () => JournalPage.pointsHanako, value => JournalPage.pointsHanako = value);
            VariableStore.CreateVariable("VN.emiPoints", 0.0f, () => JournalPage.pointsEmi, value => JournalPage.pointsEmi = value);
            VariableStore.CreateVariable("VN.yukiPoints", 0.0f, () => JournalPage.pointsYuki, value => JournalPage.pointsYuki = value);
        }
    }
}
