using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMAND
{

    //base class that extends the available commands in the commandDatabase
    //abstract class cannot be used or instanced, something must inherit from it
    public abstract class CMD_DatabaseExtension
    {
        public static void Extend(CommandDatabase database)
        {
            //nothing here 
            //all logic for extending will be in the subclasses
        }

        public static CommandParameters ConvertDataToParameters(string[] data, int startingIndex = 0) => new CommandParameters(data, startingIndex);
    }
}