using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using System.Linq;

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

        private const string CHARACTER_CASTING_ID = " as ";
        //find character prefab
        private const string CHARACTER_NAME_ID = "<charname>";
        public string characterRootPathFormat => $"Characters/{CHARACTER_NAME_ID}";
        public string characterPrefabNameFormat => $"Character - [{CHARACTER_NAME_ID}]";
        public string characterPrefabPathFormat => $"{characterRootPathFormat}/{characterPrefabNameFormat}";


        [SerializeField] private RectTransform _characterpanel = null;
        public RectTransform characterPanel => _characterpanel;
        
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
        public Character CreateCharacter(string characterName, bool revealAfterCreation = false)
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
            characters.Add(info.name.ToLower(), character);

            //check if we want to reveal character
            if (revealAfterCreation)
            {
                character.Show();
            }

            return character;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();
            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);
            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;

            result.config = config.GetConfig(result.castingName);
            result.prefab = GetPrefabForCharacter(result.castingName);

            result.rootCharacterFolder = FormatCharacterPath(characterRootPathFormat, result.castingName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPathFormat, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        public string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            switch (info.config.characterType)
            {
                case Character.CharacterType.Text:
                    return new Character_Text(info.name, info.config);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name, info.config, info.prefab, info.rootCharacterFolder);

                case Character.CharacterType.Live2D:
                    return new Character_Live2D(info.name, info.config, info.prefab, info.rootCharacterFolder);

                default:
                    return null;
            }
        }

        //manager sorts the priority of characters to be shown on UI
        //takes into account active and inactive characters in the scene
        public void SortCharacters()
        {
            //grab all active characters in scene and put into list
            //also make sure character is visible
            List<Character> activeCharacters = characters.Values.Where(c => c.root.gameObject.activeInHierarchy && c.isVisible).ToList();
            //then grab everything that isnt active
            List<Character> inactiveCharacters = characters.Values.Except(activeCharacters).ToList();

            //compare a and b priority and then sort
            activeCharacters.Sort((a, b) => a.priority.CompareTo(b.priority));
            activeCharacters.Concat(inactiveCharacters);

            SortCharacters(activeCharacters);
        }

        //set multiple characters priorities at once
        //any character in this array should prio above all other characters on screen
        public void SortCharacters(string[] characterNames)
        {
            List<Character> sortedCharacters = new List<Character>();

            //gets all characters as long as theyre valid in our dictionary
            sortedCharacters = characterNames
                .Select(name => GetCharacter(name))
                .Where(character => character != null)
                .ToList();

            //get remaining (inactive) characters
            //sort the remaining characters by their priority
            List<Character> remainingCharacters = characters.Values
                .Except(sortedCharacters)
                .OrderBy(character => character.priority)
                .ToList();

            sortedCharacters.Reverse();
            //manually update the actual chracter priority
            int startingPriority = remainingCharacters.Count > 0 ? remainingCharacters.Max(c => c.priority) : 0;
            for(int i = 0; i < sortedCharacters.Count; i++)
            {
                Character character = sortedCharacters[i];
                character.SetPriority(startingPriority + i + 1, autoSortCharactersOnUI: false);
            }

            List<Character> allCharacters = remainingCharacters.Concat(sortedCharacters).ToList();
            SortCharacters(allCharacters);
        }

        //set the sorting priority on screen UI
        private void SortCharacters(List<Character> charactersSortingOrder)
        {
            //first child
            int i = 0;
            foreach(Character character in charactersSortingOrder)
            {
                Debug.Log($"{character.name} priority is {character.priority}");
                character.root.SetSiblingIndex(i++);
            }
        }

        private class CHARACTER_INFO
        {
            public string name = "";
            public string castingName = "";
            public string rootCharacterFolder = "";
            public CharacterConfigData config = null;
            public GameObject prefab = null;
        }


    }
}