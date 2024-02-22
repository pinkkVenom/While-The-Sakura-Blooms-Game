using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores all commands that the commandManager will use (a dictionary)
public class CommandDatabase
{
    //dynamic list of commands found in text files (not pre-populated)
    private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();
    //check if a command exists based on the searched name within the dictionary
    public bool HasCommand(string commandName) => database.ContainsKey(commandName.ToLower());

    public void AddCommand(string commandName, Delegate command)
    {
        commandName = commandName.ToLower();
        //if the command hasnt been added to the database
        if (!database.ContainsKey(commandName))
        {
            database.Add(commandName, command);
        }
        else
        {
            Debug.LogError($"Command already exists in the database '{commandName}'");
        }
    }

    public Delegate GetCommand(string commandName)
    {
        commandName = commandName.ToLower();
        if (!database.ContainsKey(commandName))
        {
            Debug.LogError($"Command '{commandName}' does not exist in the database");
            return null;
        }
        return database[commandName];
    }
}
