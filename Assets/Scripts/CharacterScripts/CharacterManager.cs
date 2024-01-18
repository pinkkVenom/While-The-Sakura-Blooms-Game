using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

//central hub for creating, retrieving, and managing characters in the scene
namespace CHARACTERS
{

    public class CharacterManager : MonoBehaviour
    {
        //should only be one in the scene
        //can persist between scenes
        public static CharacterManager instance { get; private set; }
        //create dictionary for characters
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        //pointer to config data
        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigAsset;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        //get configuration data for character
        public CharacterConfigData GetCharacterConfig(string characterName)
        {
            return config.GetConfig(characterName);
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                return characters[characterName.ToLower()];            
            }
            else if (createIfDoesNotExist)
            {
                return CreateCharacter(characterName);
            }
            return null;
        }

        //handles character creation
        public Character CreateCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create this character.");
                return null;
            }
            //at this point we don't have the character created so we must do so
            CHARACTER_INFO info = GetCharacterInfo(characterName);
            Character character = CreateCharacterFromInfo(info);

            //log the character so we dont create them twice
            characters.Add(character.name.ToLower(), character);

            return character;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();
            result.name = characterName;
            result.config = config.GetConfig(characterName);

            return result;
        }

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            switch (info.config.characterType)
            {
                case Character.CharacterType.Text:
                    return new Character_Text(info.name, info.config);
                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name, info.config);
                case Character.CharacterType.Live2D:
                    return new Character_Live2D(info.name, info.config);
                default:
                    return null;
            }
        }

        private class CHARACTER_INFO
        {
            public string name = "";
            public CharacterConfigData config = null;
        }


    }
}