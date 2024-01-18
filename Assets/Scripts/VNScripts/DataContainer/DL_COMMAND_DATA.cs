using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//data container for commands and arguments that will be executed in a dialogue line

public class DL_COMMAND_DATA
{
    public List<Command> commands;

    //consts///
    private const char COMMANDSPLITTER_ID = ',';
    private const char ARGUMENTCONTAINER_ID = '(';
    private const string WAITCOMMAND_ID = "[wait]";
    ///////////////////////////
    

    public struct Command
    {
        public string name;
        public string[] arguments;
        public bool waitForCompletion;
    }

    public DL_COMMAND_DATA(string rawCommands)
    {
        commands = RipCommands(rawCommands);
    }

    //rips a list of command lines from the raw data
    private List<Command> RipCommands(string rawCommands)
    {
        //split the line by commas, then remove any emtpy sections
        string[] data = rawCommands.Split(COMMANDSPLITTER_ID, System.StringSplitOptions.RemoveEmptyEntries);
        //store the extracted commands in a new list
        List<Command> result = new List<Command>();

        //create a struct for each command and its arguments
        foreach(string cmd in data)
        {
            Command command = new Command();
            int index = cmd.IndexOf(ARGUMENTCONTAINER_ID);
            command.name = cmd.Substring(0, index).Trim();

            //check if we have to wait for a command
            if (command.name.ToLower().StartsWith(WAITCOMMAND_ID))
            {
                command.name = command.name.Substring(WAITCOMMAND_ID.Length);
                command.waitForCompletion = true;
            }
            else
            {
                command.waitForCompletion = false;
            }
            command.arguments = getArgs(cmd.Substring(index + 1, cmd.Length - index - 2));
            result.Add(command);
        }

        return result;
    }

    private string[] getArgs(string args)
    {
        List<string> argList = new List<string>();
        //faster than concatenation because new content is added to an existing string rather than creating a new one
        StringBuilder currentArg = new StringBuilder();
        //to ignore spaces in quotations
        bool inQuotes = false;

        for(int i = 0; i < args.Length; i++)
        {
            //if we locate a quote
            if (args[i] == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }
            //if we're not in quotes and we've hit a space
            if(!inQuotes && args[i] == ' ')
            {
                //add the argument to the argument list, then clear the current argument to move on
                argList.Add(currentArg.ToString());
                currentArg.Clear();
                continue;
            }

            //if we havent hit a quotation or space then we just append whatever character we have to build argument character by character
            currentArg.Append(args[i]);
        }
        //make sure to get the last argument saved (otherwise it wouldnt be included since it isnt followed by a space or "
        if (currentArg.Length > 0)
        {
            argList.Add(currentArg.ToString());
        }
        return argList.ToArray();
    }
}
