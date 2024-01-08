using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//storage for dialogue information that has been parsed and taken from a strings

namespace DIALOGUE
{
    public class DIALOGUE_LINE
    {
        public string speaker;
        public string dialogue;
        public string commands;

        //checks right away if speaker is empty
        public bool hasSpeaker => speaker != string.Empty;
        //checks right away if dialogue is empty
        public bool hasDialogue => dialogue != string.Empty;
        //checks right away if commands is empty
        public bool hasCommands => commands != string.Empty;

        public DIALOGUE_LINE(string speaker, string dialogue, string commands)
        {
            this.speaker = speaker;
            this.dialogue = dialogue;
            this.commands = commands;
        }
    }
}
