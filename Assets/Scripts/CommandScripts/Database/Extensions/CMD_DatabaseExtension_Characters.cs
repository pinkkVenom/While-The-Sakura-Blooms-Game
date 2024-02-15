using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CHARACTERS;
using System.Linq;

namespace COMMAND
{
    public class CMD_DatabaseExtension_Characters : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));
            database.AddCommand("show", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("hide", new Func<string[], IEnumerator>(ShowAll));
        }

        public static void CreateCharacter(string[] data)
        {
            string characterName = data[0];
            //to enable character on start
            bool enable = false;

            var parameters = ConvertDataToParameters(data);
            parameters.TryGetValue(new string[] { "-e", "-enabled" }, out enable, defaultValue: false);
            CharacterManager.instance.CreateCharacter(characterName, revealAfterCreation: enable);
        }

        //global functions for showing/hiding MULTIPLE characters
        public static IEnumerator ShowAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            //if we want to immediately show character
            bool immediate = false;

            //find out all the characters we have in the list
            foreach(string s in data)
            {
                //only retrieve character if they exist in the scene
                Character character = CharacterManager.instance.GetCharacter(s, createIfDoesNotExist: false);
                if(character != null)
                {
                    characters.Add(character);
                }
            }
            if(characters.Count == 0)
            {
                yield break;
            }

            //convert data array to parameter container
            var parameters = ConvertDataToParameters(data);
            //we can have multiple ways of writing immediate
            parameters.TryGetValue(new string[] { "-i", "-immediate"}, out immediate, defaultValue: false);

            //call logic on all the characters
            foreach(Character character in characters)
            {
                if (immediate)
                {
                    character.isVisible = true;
                }
                else
                {
                    character.Show();
                }
            }

            if (!immediate)
            {
                //if any characters are revealing, we wait for them to finish fading on screen
                while (characters.Any(c => c.isRevealing))
                {
                    yield return null;
                }
            }
        }
        public static IEnumerator HideAll(string[] data)
        {
            List<Character> characters = new List<Character>();
            //if we want to immediately show character
            bool immediate = false;

            //find out all the characters we have in the list
            foreach (string s in data)
            {
                //only retrieve character if they exist in the scene
                Character character = CharacterManager.instance.GetCharacter(s, createIfDoesNotExist: false);
                if (character != null)
                {
                    characters.Add(character);
                }
            }
            if (characters.Count == 0)
            {
                yield break;
            }

            //convert data array to parameter container
            var parameters = ConvertDataToParameters(data);
            //we can have multiple ways of writing immediate
            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out immediate, defaultValue: false);

            //call logic on all the characters
            foreach (Character character in characters)
            {
                if (immediate)
                {
                    character.isVisible = false;
                }
                else
                {
                    character.Hide();
                }
            }

            if (!immediate)
            {
                //if any characters are revealing, we wait for them to finish fading on screen
                while (characters.Any(c => c.isHiding))
                {
                    yield return null;
                }
            }
        }
    }
}