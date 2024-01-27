using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//character with no graphical art
//text operations only
namespace CHARACTERS
{

    public class Character_Text : Character
    {
       public Character_Text(string name, CharacterConfigData config) : base(name, config, prefab: null)
        {
            Debug.Log($"Created Text Character: '{name}'");
        }
    }
}