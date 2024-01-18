using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    [CreateAssetMenu(fileName = "Character Configuration Asset", menuName = "Dialogue System/Character Configuration Asset")]
    public class CharacterConfigSO : ScriptableObject
    {
        public CharacterConfigData[] characters;

        public CharacterConfigData GetConfig(string characterName)
        {
            //make sure the character name isnt case sensitive
            characterName = characterName.ToLower();

            //loop through all existing characters to find a match
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterConfigData data = characters[i];
                //use string equals to find match
                //if we have a match
                if(string.Equals(characterName, data.name.ToLower()) || string.Equals(characterName, data.alias.ToLower()))
                {
                    return data.Copy();
                }
            }
            //if we dont have a match, then make a default character
            return CharacterConfigData.Default;
        }
    }
}