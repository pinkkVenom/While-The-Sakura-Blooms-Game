using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using COMMAND;
using DIALOGUE;

public class TESTING_AUDIO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
    }

    Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

    IEnumerator Running()
    {
        Character_Sprite Hanako = CreateCharacter("Hanako") as Character_Sprite;
        Hanako.Show();

        yield return new WaitForSeconds(1);

        AudioManager.instance.PlaySoundEffect("Audio/SFX/footsteps", loop: true);

        yield return new WaitForSeconds(1);

        Hanako.Animate("Hop");
        Hanako.Say("Im walking");

        yield return new WaitForSeconds(2);

        AudioManager.instance.StopSoundEffect("footsteps");

        AudioManager.instance.PlayTrack("Audio/Music/reflectedLight", startingVolume: 0.7f);
    }
}
