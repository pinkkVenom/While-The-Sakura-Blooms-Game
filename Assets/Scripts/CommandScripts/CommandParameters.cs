using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//helps the database extensions to allow easy location of parameters within the data passed to commands
namespace COMMAND 
{
    public class CommandParameters
    {
        private const char PARAMETER_IDENTIFIER = '-';

        private Dictionary<string, string> parameters = new Dictionary<string, string>();
        private List<string> unlabeledParameters = new List<string>();

        public CommandParameters(string[] paramaterArray, int startingIndex = 0)
        {
            for(int i = startingIndex; i < paramaterArray.Length; i++)
            {
                if (paramaterArray[i].StartsWith(PARAMETER_IDENTIFIER) && !float.TryParse(paramaterArray[i], out _))
                {
                    string pName = paramaterArray[i];
                    string pValue = "";

                    //check if the value after the - symbol is a valid symbol
                    if (i + 1 < paramaterArray.Length && !paramaterArray[i + 1].StartsWith(PARAMETER_IDENTIFIER))
                    {
                        pValue = paramaterArray[i + 1];
                        //skips by 1 more
                        i++;
                    }
                    parameters.Add(pName, pValue);
                }
                else
                {
                    unlabeledParameters.Add(paramaterArray[i]);
                }
            }
        }

        //try to get a value from the dictionary
        //T represents any value
        public bool TryGetValue<T>(string parameterName, out T value, T defaultValue = default(T)) => TryGetValue(new string[] { parameterName }, out value, defaultValue);
        public bool TryGetValue<T>(string[] parameterNames, out T value, T defaultValue = default(T))
        {
            //go through all specified parameter values
            foreach(string parameterName in parameterNames)
            {
                //if we find it, it has to be cast to 1 of 4 data types
                if(parameters.TryGetValue(parameterName, out string parameterValue))
                {
                    if(TryCastParameter(parameterValue, out value))
                    {
                        //we found what we were looking for
                        return true;
                    }
                }

            }
            //if we get here, no match was found in the existing parameters
            //search unlabelled parameters if present
            foreach (string parameterName in unlabeledParameters)
            {
                if (TryCastParameter(parameterName, out value))
                {
                    unlabeledParameters.Remove(parameterName);
                    return true;
                }

            }

            value = defaultValue;
            return false;
        }

        //the function that casts data type
        private bool TryCastParameter<T>(string parameterValue, out T value)
        {
            if(typeof(T) == typeof(bool))
            {
                if (bool.TryParse(parameterValue, out bool boolValue))
                {
                    //must be cast as object
                    //cant convert T to bool overwise
                    value = (T)(object)boolValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(parameterValue, out int intValue))
                {
                    value = (T)(object)intValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(parameterValue, out float floatValue))
                {
                    value = (T)(object)floatValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                    value = (T)(object)parameterValue;
                    return true;
            }
            value = default(T);
            return false;
        }
    }
}
