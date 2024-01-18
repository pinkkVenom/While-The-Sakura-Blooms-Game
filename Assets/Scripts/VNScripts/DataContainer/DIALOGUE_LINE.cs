using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//storage for dialogue information that has been parsed and taken from a strings

namespace DIALOGUE
{
    public class DIALOGUE_LINE
    {
        public DL_SPEAKER_DATA speakerData;
        public DL_DIALOGUE_DATA dialogueData;
        public DL_COMMAND_DATA commandData;

        //checks right away if speaker is null
        public bool hasSpeaker => speakerData != null;
        //checks right away if dialogue is null, points to DLDIALOGUEDATA variable
        public bool hasDialogue => dialogueData != null;
        //checks right away if commands is null
        public bool hasCommands => commandData != null;

        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speakerData = (string.IsNullOrWhiteSpace(speaker) ? null : new DL_SPEAKER_DATA(speaker));
            this.dialogueData = (string.IsNullOrWhiteSpace(dialogue) ? null : new DL_DIALOGUE_DATA(dialogue));
            this.commandData = (string.IsNullOrWhiteSpace(commands) ? null : new DL_COMMAND_DATA(commands));
        }
    }
}
