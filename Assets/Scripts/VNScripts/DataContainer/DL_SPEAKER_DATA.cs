using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

namespace DIALOGUE
{
    public class DL_SPEAKER_DATA
    {
        public string rawData { get; private set; } = string.Empty;
        //name = true name of the character
        //casterName = temporary name (ex. Strange Woman, Stranger, Angry Man)
        public string name, castName;
        //if our cast name isnt empty then we use castname data, otherwise use character name
        public string displayName => isCastingName ? castName : name;
        //where the character will appear on screen when theyre displayed (left or right side)
        public Vector2 castPosition;
        //stores the expression layers
        //each character has several expression types that need to be stored
        public List<(int layer, string expression)> CastExpressions { get; set; }

        public bool isCastingName => castName != string.Empty;
        public bool isCastingPosition = false;
        public bool isCastingExpressions => CastExpressions.Count > 0;

        public bool makeCharacterEnter = false;

        //CONSTS FOR SPEAKER DATA
        private const string NAMECAST_ID = " as ";
        private const string POSITIONCAST_ID = " at ";
        private const string ENTER_KEYWORD = "enter ";
        private const string EXPRESSIONCAST_ID = " [";
        private const char AXIS_DELIMITER = ':';
        private const char EXPRESSIONLAYER_JOINER = ',';
        //private const char EXPRESSIONLAYER_DELIMITER = ':'

        private string ProcessKeywords(string rawSpeaker)
        {
            if (rawSpeaker.StartsWith(ENTER_KEYWORD))
            {
                //remove keyword from dialogue string
                rawSpeaker = rawSpeaker.Substring(ENTER_KEYWORD.Length);
                makeCharacterEnter = true;
            }
            return rawSpeaker;
        }

        public DL_SPEAKER_DATA(string rawSpeaker)
        {
            rawData = rawSpeaker;
            rawSpeaker = ProcessKeywords(rawSpeaker);

            string pattern = @$"{NAMECAST_ID}|{POSITIONCAST_ID}|{EXPRESSIONCAST_ID.Insert(EXPRESSIONCAST_ID.Length - 1, @"\")}";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            //populate the data so there are no null references to values
            castName = "";
            castPosition = Vector2.zero;
            CastExpressions = new List<(int layer, string expression)>();

            //if we have no casting data then speaker name is the only data
            if (matches.Count == 0)
            {
                name = rawSpeaker;
                return;
            }

            //isolate the speaker name from the casting data
            int index = matches[0].Index;
            name = rawSpeaker.Substring(0, index);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                int startIndex = 0, endIndex = 0;

                if (match.Value == NAMECAST_ID)
                {
                    startIndex = match.Index + NAMECAST_ID.Length;
                    //if i is less than the matches when we have a match, then the index will go up to the next match and thats all our speaker data
                    //otherwise we want the end of the speaker data is the end of the index
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    //now we grab the name we pulled from the string
                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                else if (match.Value == POSITIONCAST_ID)
                {
                    isCastingPosition = true;

                    startIndex = match.Index + POSITIONCAST_ID.Length;
                    //if i is less than the matches when we have a match, then the index will go up to the next match and thats all our speaker data
                    //otherwise we want the end of the speaker data is the end of the index
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    //now we grab the position we pulled from the string
                    string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                    //we need to split the casPos string because it can have an x and y value
                    string[] axis = castPos.Split(AXIS_DELIMITER, System.StringSplitOptions.RemoveEmptyEntries);
                    //now we try to parse the contents of the line
                    float.TryParse(axis[0], out castPosition.x);

                    //if we have more data then we set the y value
                    if (axis.Length > 1)
                    {
                        float.TryParse(axis[1], out castPosition.y);
                    }
                }
                else if (match.Value == EXPRESSIONCAST_ID)
                {
                    startIndex = match.Index + EXPRESSIONCAST_ID.Length;
                    //if i is less than the matches when we have a match, then the index will go up to the next match and thats all our speaker data
                    //otherwise we want the end of the speaker data is the end of the index
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    //now we grab the position we pulled from the string
                    string castExp = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));

                    CastExpressions = castExp.Split(EXPRESSIONLAYER_JOINER)
                        .Select(x =>
                        {
                            var parts = x.Trim().Split(AXIS_DELIMITER);

                            if (parts.Length == 2)
                            {
                                return (int.Parse(parts[0]), parts[1]);
                            }
                            else
                            {
                                return (0, parts[0]);
                            }
                        }).ToList();
                }
            }
        }
    }
}