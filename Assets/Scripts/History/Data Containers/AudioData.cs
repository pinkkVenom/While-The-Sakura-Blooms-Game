using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HISTORY
{
    [System.Serializable]
    public class AudioData
    {
        public int channel = 0;
        public string trackName;
        public string trackPath;
        public float trackVolume;
        public bool loop;
        public float trackPitch;

        public AudioData(AudioChannel channel)
        {
            this.channel = channel.channelIndex;

            if(channel.activeTrack == null)
            {
                return;
            }

            var track = channel.activeTrack;
            trackName = track.name;
            trackPath = track.filePath;
            trackVolume = track.volumeCap;
            loop = track.loop;
            trackPitch = track.pitch;
        }

        public static List<AudioData> Capture()
        {
            List<AudioData> audioChannels = new List<AudioData>();
            foreach(var channel in AudioManager.instance.channels)
            {

                if (channel.Value.activeTrack == null)
                {
                    continue;
                }
                AudioData data = new AudioData(channel.Value);
                audioChannels.Add(data);
            }
            return audioChannels;
        }

    }
}