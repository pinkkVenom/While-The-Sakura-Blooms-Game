using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//character that uses sprites or sprite sheets to render its display
namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        public Character_Sprite(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Created Sprite Character: '{name}'");
        }

    }
}