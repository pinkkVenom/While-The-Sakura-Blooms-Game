using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace COMMAND
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
        }

        //takes in a single value for time
        //time is in seconds
        private static IEnumerator Wait(string data)
        {
            //try parsing to get a type
            if(float.TryParse(data, out float time))
            {
                yield return new WaitForSeconds(time);
            }
        }
    }
}
