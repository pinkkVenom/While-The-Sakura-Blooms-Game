using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DIALOGUE
{

    //data container that holds all segments and information about a single dialogue line
    public class DL_DIALOGUE_DATA
    {
        public List<DIALOGUE_SEGMENT> segments;
        //the regex pattern for locating identifiers in the dialogue lines
        private const string segmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";

        //constructor that will extract elements of the dialogue line
        //into separate data pieces
        public DL_DIALOGUE_DATA(string rawDialogue)
        {
            segments = RipSegments(rawDialogue);
        }

        //this method rips the segments from the dialogue lines
        //looks for identifiers
        public List<DIALOGUE_SEGMENT> RipSegments(string rawDialogue)
        {
            List<DIALOGUE_SEGMENT> segments = new List<DIALOGUE_SEGMENT>();
            MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPattern);

            int lastIndex = 0;
            //find the first or only segment in the file
            DIALOGUE_SEGMENT segment = new DIALOGUE_SEGMENT();
            segment.dialogue = matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index);
            segment.startSignal = DIALOGUE_SEGMENT.StartSignal.NONE;
            segment.signalDelay = 0;
            segments.Add(segment);

            //dont continue if there are no matches
            if (matches.Count == 0)
            {
                return segments;
            }
            else
            {
                lastIndex = matches[0].Index;
            }
            //if we have more segments, this loop goes through all of them
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                segment = new DIALOGUE_SEGMENT();

                //getting the start signal for the segment
                string signalMatch = match.Value; //this will grab the container {} as well
                signalMatch = signalMatch.Substring(1, match.Length - 2); //this grabs only the content of the container
                string[] signalSplit = signalMatch.Split(' '); //splits the signals by spaces

                segment.startSignal = (DIALOGUE_SEGMENT.StartSignal)Enum.Parse(typeof(DIALOGUE_SEGMENT.StartSignal), signalSplit[0].ToUpper());

                //get signal delay
                //we know we have a delay if our signal split has more than 1 value
                if (signalSplit.Length > 1)
                {
                    float.TryParse(signalSplit[1], out segment.signalDelay);
                }

                //get the dialogue for the segment
                //and to find the end of the line we check if theres another segment after this one
                int nextIndex = i + 1 < matches.Count ? matches[i + 1].Index : rawDialogue.Length;
                //we make sure we exclude that first { that is included in the match
                //add the match length to skip over the {
                segment.dialogue = rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length));
                lastIndex = nextIndex;

                segments.Add(segment);

            }
            return segments;

        }

        //stores several variables relating to the dialogue segment pieces
        public struct DIALOGUE_SEGMENT
        {
            public string dialogue;
            public StartSignal startSignal;
            //tracks the wait time for signals
            public float signalDelay;

            //tracks the signals in the dialogue
            //c is clear when clicked
            //a is append when clicked
            //wc is wait and clear
            //wa is wait and append
            public enum StartSignal { NONE, C, A, WC, WA }

            public bool appendText => startSignal == StartSignal.A || startSignal == StartSignal.WA;
        }
    }
}