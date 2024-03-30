using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HISTORY
{
    [System.Serializable]
    public class SFXData
    {
        public string filePath;
        public string fileName;
        public float volume;
        public float pitch;

        public static List<SFXData> Capture()
        {
            List<SFXData> audioList = new List<SFXData>();
            AudioSource[] sfx = AudioManager.instance.allSFX;
            
            foreach(var sound in sfx)
            {
                if (!sound.loop)
                {
                    continue;
                }

                SFXData data = new SFXData();
                data.volume = sound.volume;
                data.pitch = sound.pitch;
                data.fileName = sound.clip.name;

                string resourcesPath = sound.gameObject.name.Split(AudioManager.SFX_NAME_FORMAT_CONTAINERS)[1];

                data.filePath = resourcesPath;

                audioList.Add(data);
            }
            return audioList;
        }

        public static void Apply(List<SFXData> sfx)
        {
            List<string> cache = new List<string>();

            foreach(var sound in sfx)
            {
                if(!AudioManager.instance.isPlayingSFX(sound.fileName))
                    AudioManager.instance.PlaySoundEffect(sound.filePath, volume: sound.volume, pitch: sound.pitch, loop:true);

                cache.Add(sound.fileName);
            }

            foreach(var source in AudioManager.instance.allSFX)
            {
                if (!cache.Contains(source.clip.name))
                {
                    AudioManager.instance.StopSoundEffect(source.clip);
                }
            }
        }
    }
}