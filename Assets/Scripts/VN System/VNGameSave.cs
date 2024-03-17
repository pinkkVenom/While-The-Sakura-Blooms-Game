using HISTORY;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;
//file that is written to disk to save a players session progress

namespace VISUALNOVEL
{
    [System.Serializable]
    public class VNGameSave
    {
        //variables to write to file

        //there can only be one instance loaded at a time, either a new game or loaded game
        public static VNGameSave activeFile = null;
        //custom extension for save files
        public const string FILE_TYPE = ".vns";
        //preview screenshot file extension
        public const string SCREENSHOT_FILE_TYPE = ".jpg";
        //encryption is enabled
        public const bool ENCRYPT_FILES = false;

        public string filePath => $"{FilePaths.gameSaves}{slotNumber}{FILE_TYPE}";
        public string screenshotPath => $"{FilePaths.gameSaves}{slotNumber}{SCREENSHOT_FILE_TYPE}";

        public string playeName;
        public int slotNumber = 1;

        public string[] activeConversations;
        public HistoryState activeState;
        public HistoryState[] historyLogs;

        public void Save()
        {
            activeState = HistoryState.Capture();
            historyLogs = HistoryManager.instance.history.ToArray();
            activeConversations = GetConversationData();

            string saveJSON = JsonUtility.ToJson(this);
            FileManager.Save(filePath, saveJSON);
        }

        public void Load()
        {
            if(activeState != null)
            {
                activeState.Load();
            }

            HistoryManager.instance.history = historyLogs.ToList();
            HistoryManager.instance.logManager.Clear();
            HistoryManager.instance.logManager.Rebuild();
            SetConversationData();

            DialogueSystem.instance.prompt.Hide();
        }

        private string[] GetConversationData()
        {
            List<string> retData = new List<string>();
            var conversations = DialogueSystem.instance.conversationManager.GetConversationQueue();

            for (int i = 0; i < conversations.Length; i++)
            {
                var conversation = conversations[i];
                string data = "";

                if (conversation.file != string.Empty)
                {
                    var compressedData = new VN_ConversationDataCompressed();
                    compressedData.fileName = conversation.file;
                    compressedData.progress = conversation.GetProgress();
                    compressedData.startIndex = conversation.fileStartIndex;
                    compressedData.endIndex = conversation.fileEndIndex;
                    data = JsonUtility.ToJson(compressedData);
                }
                else
                {
                    var fullData = new VN_ConversationData();
                    fullData.conversation = conversation.GetLines();
                    fullData.progress = conversation.GetProgress();
                    data = JsonUtility.ToJson(fullData);
                }

                retData.Add(data);
            }

            return retData.ToArray();
        }

        private void SetConversationData()
        {
            for (int i = 0; i < activeConversations.Length; i++)
            {
                try
                {
                    string data = activeConversations[i];
                    Conversation conversation = null;

                    var fullData = JsonUtility.FromJson<VN_ConversationData>(data);
                    if (fullData != null && fullData.conversation != null && fullData.conversation.Count > 0)
                    {
                        conversation = new Conversation(fullData.conversation, fullData.progress);
                    }
                    else
                    {
                        var compressedData = JsonUtility.FromJson<VN_ConversationDataCompressed>(data);
                        if (compressedData != null && compressedData.fileName != string.Empty)
                        {
                            TextAsset file = Resources.Load<TextAsset>(compressedData.fileName);

                            int count = compressedData.endIndex - compressedData.startIndex;

                            List<string> lines = FileManager.ReadTextAsset(file).Skip(compressedData.startIndex).Take(count + 1).ToList();

                            conversation = new Conversation(lines, compressedData.progress, compressedData.fileName, compressedData.startIndex, compressedData.endIndex);
                        }
                        else
                        {
                            Debug.LogError($"Unknown conversation format! Unable to reload conversation from VNGameSave using data '{data}'");
                        }
                    }

                    if (conversation != null && conversation.GetLines().Count > 0)
                    {
                        if (i == 0)
                            DialogueSystem.instance.conversationManager.StartConversation(conversation);
                        else
                            DialogueSystem.instance.conversationManager.Enqueue(conversation);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Encountered error while extracting saved conversation data! {e}");
                    continue;
                }
            }
        }
    }
}
