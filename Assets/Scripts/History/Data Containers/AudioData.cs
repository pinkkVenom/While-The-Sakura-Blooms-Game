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

        public static void Apply(List<AudioData> data)
        {
            List<int> cache = new List<int>();

            foreach(var channelData in data)
            {
                AudioChannel channel = AudioManager.instance.TryGetChannel(channelData.channel, createIfDoesNotExist: true);
                if(channel.activeTrack == null || channel.activeTrack.name != channelData.trackName)
                {
                    AudioClip clip = HistoryCache.LoadAudio(channelData.trackPath);
                    if(clip != null)
                    {
                        channel.StopTrack(immediate: true);
                        channel.PlayTrack(clip, channelData.loop, channelData.trackVolume, channelData.trackVolume, channelData.trackPitch, channelData.trackPath);
                    }
                    else
                    {
                        Debug.LogWarning($"History State: Could not load audio track '{channelData.trackPath}'");
                    }
                }
                cache.Add(channelData.channel);
            }
            foreach (var channel in AudioManager.instance.channels)
            {
                if (!cache.Contains(channel.Value.channelIndex))
                {
                    channel.Value.StopTrack(immediate: true);
                }
            }
        }

    }
}