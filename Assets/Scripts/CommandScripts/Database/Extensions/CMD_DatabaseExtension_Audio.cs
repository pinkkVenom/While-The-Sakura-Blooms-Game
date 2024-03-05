using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace COMMAND
{
    public class CMD_DatabaseExtension_Audio : CMD_DatabaseExtension
    {
        private static string[] PARAM_SFX = new string[] { "-s", "-sfx" };
        private static string[] PARAM_VOLUME = new string[] { "-v", "-vol", "-volume" };
        private static string[] PARAM_PITCH = new string[] { "-p", "-pitch" };
        private static string[] PARAM_LOOP = new string[] { "-l", "-loop" };

        private static string[] PARAM_CHANNEL = new string[] { "-c", "-channel" };
        private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
        private static string[] PARAM_START_VOLUME = new string[] { "-sv", "-startvolume" };
        private static string[] PARAM_SONG = new string[] { "-so", "-song" };

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("playsfx", new Action<string[]>(PlaySFX));
            database.AddCommand("stopsfx", new Action<string>(StopSFX));

            database.AddCommand("playvoice", new Action<string[]>(PlayVoice));
            database.AddCommand("stopvoice", new Action<string>(StopSFX));

            database.AddCommand("playsong", new Action<string[]>(PlaySong));
            database.AddCommand("stopsong", new Action<string>(StopSong));
        }

        private static void PlaySFX(string[] data)
        {
            string filePath;
            float volume, pitch;
            bool loop;

            var parameters = ConvertDataToParameters(data);

            //get the name or path to sound effect
            parameters.TryGetValue(PARAM_SFX, out filePath);
            //try to get volume of sound
            parameters.TryGetValue(PARAM_VOLUME, out volume);
            //try to get pitch of the sound
            parameters.TryGetValue(PARAM_PITCH, out pitch);
            //try to see if sound loops
            parameters.TryGetValue(PARAM_LOOP, out loop);

            AudioClip sound = Resources.Load<AudioClip>(FilePaths.GetPathToResources(FilePaths.resources_sfx, filePath));

            if(sound == null)
            {
                return;
            }

            AudioManager.instance.PlaySoundEffect(sound, volume: volume, pitch: pitch, loop: loop);
        }

        private static void StopSFX(string data)
        {
            AudioManager.instance.StopSoundEffect(data);
        }

        private static void PlayVoice(string[] data)
        {
            string filePath;
            float volume, pitch;
            bool loop;

            var parameters = ConvertDataToParameters(data);

            //get the name or path to sound effect
            parameters.TryGetValue(PARAM_SFX, out filePath);
            //try to get volume of sound
            parameters.TryGetValue(PARAM_VOLUME, out volume);
            //try to get pitch of the sound
            parameters.TryGetValue(PARAM_PITCH, out pitch);
            //try to see if sound loops
            parameters.TryGetValue(PARAM_LOOP, out loop);

            AudioClip sound = Resources.Load<AudioClip>(FilePaths.GetPathToResources(FilePaths.resources_sfx, filePath));

            if (sound == null)
            {
                return;
            }

            AudioManager.instance.PlayVoice(sound, volume: volume, pitch: pitch, loop: loop);
        }

        private static void PlaySong(string[] data)
        {
            string filePath;
            int channel;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SONG, out filePath);
            filePath = FilePaths.GetPathToResources(FilePaths.resources_music, filePath);

            parameters.TryGetValue(PARAM_CHANNEL, out channel, defaultValue: 1);

            PlayTrack(filePath, channel, parameters);
        }

        private static void PlayTrack(string filePath, int channel, CommandParameters parameters)
        {
            bool loop;
            float volumeCap;
            float startVolume;
            float pitch;


            parameters.TryGetValue(PARAM_VOLUME, out volumeCap, defaultValue: 1f);

            parameters.TryGetValue(PARAM_START_VOLUME, out startVolume, defaultValue: 0f);

            parameters.TryGetValue(PARAM_PITCH, out pitch, defaultValue: 1f);

            parameters.TryGetValue(PARAM_LOOP, out loop, defaultValue: true);


            AudioClip sound = Resources.Load<AudioClip>(filePath);

            if(sound == null)
            {
                Debug.LogError($"was not able to load voice '{filePath}'");
                return;
            }

            AudioManager.instance.PlayTrack(sound, filePath, channel, loop, startVolume, volumeCap, pitch);
        }

        private static void StopSong(string data)
        {
            if(data == string.Empty)
            {
                StopTrack("1");
            }
            else
            {
                StopTrack(data);
            }
        }

        private static void StopTrack(string data)
        {
            if(int.TryParse(data, out int channel))
            {
                AudioManager.instance.StopTrack(channel);
            }
            else
            {
                AudioManager.instance.StopTrack(data);
            }
        }
    }
}
