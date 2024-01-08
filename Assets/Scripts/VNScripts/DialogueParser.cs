using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//system that handles parsing functions to convert strings into DIALOGUE_LINES

namespace DIALOGUE
{ 
    public class DialogueParser
    {
        //w identifies word character of any length (*)
        //the [^] will exclude the proceeding of s which is white space
        //the ( is looking for parenthesis
        private const string commandRegexPattern = "\\w*[^\\s]\\(";

        //to parse a string straight from the dialogue file
        //sends string to ripcontent method to be separated into 3 parts
        public static DIALOGUE_LINE Parse(string rawLine)
        {
            Debug.Log($"Parsing line - '{rawLine}'");
            (string speaker, string dialogue, string commands) = RipContent(rawLine);
            return new DIALOGUE_LINE(speaker, dialogue, commands);
        }

        //this method will separate 3 fields from a line 
        //the method searches for quotation mark symbols in the string
        //it will take the first one and the last one as the dialogue
        //the remainder will be the speaker information, and commands
        private static (string, string, string) RipContent(string rawLine)
        {
            string speaker = "", dialogue = "", commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            //searches for quotations inside dialogue that have backslash in front of them
            bool isEscaped = false;

            //search through string to find the quotation marks
            for (int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                //if we find a \ symbol, then change is escaped state to opposite
                if (current == '\\')
                {
                    isEscaped = !isEscaped;
                }
                //if we find a " and we're not escaped, then we're either at the start or end of the dialogue
                else if(current == '"' && !isEscaped)
                {
                    if (dialogueStart == -1)
                    {
                        dialogueStart = i;
                    }
                    else if (dialogueEnd == -1)
                    {
                        dialogueEnd = i;
                        break;
                    }
                    
                }
                else
                {
                    isEscaped = false;
                }
            }

            //Indentifying Command Patterns in Strings
            Regex commandRegex = new Regex(commandRegexPattern);
            Match match = commandRegex.Match(rawLine);
            int commandStart = -1;
            //if the line matches the regex pattern
            if (match.Success)
            {
                commandStart = match.Index;
                if(dialogueStart == -1 && dialogueEnd == -1)
                {
                    return ("", "", rawLine.Trim());
                }
            }

            //this figures out whether we have dialogue or if the quotations are in a command
            //if we have dialogue AND the command hasnt been found or the command index is greater than dialogue end index, then we found the dialogue
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                //there is valid dialogue in the string
                //speaker data is from beginning to dialogue
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                //dialogue is between the start point (-1 to remove quotation) and dialogue end minus the start and quotation
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart -1).Replace("\\\"", "\"");
                if(commandStart != -1)
                {
                    //we have commands present in the string
                    commands = rawLine.Substring(commandStart).Trim();
                }
            }
            //if commandStart is found, and dialogue is greater than the commandStart, then we found a command argument in quotations
            else if (commandStart != -1 && dialogueStart > commandStart)
            {
                commands = rawLine;
            }
            else
            {
                speaker = rawLine;
            }

            Debug.Log(rawLine.Substring(dialogueStart + 1, (dialogueEnd - dialogueStart)-1));

            return (speaker, dialogue, commands);
        }
    }
}
