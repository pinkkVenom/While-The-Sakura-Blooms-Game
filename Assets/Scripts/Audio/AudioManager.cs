using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//handles sound effects, voices, ambience, and music

public class AudioManager : MonoBehaviour
{
    private const string SFX_PARENT_NAME = "SFX";
    private const string SFX_NAME_FORMAT = "SFX - [{0}]";
    //default speed which tracks transition
    public const float TRACK_TRANSITION_SPEED = 1f;
    public static AudioManager instance { get; private set; }

    //create dictionary to keep track of all audio channels
    //1: audio channel #, 2: audio channel
    public Dictionary<int, AudioChannel> channels = new Dictionary<int, AudioChannel>();

    //audio mixer groups
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup voicesMixer;

    private Transform sfxRoot;

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

        sfxRoot = new GameObject(SFX_PARENT_NAME).transform;
        sfxRoot.SetParent(transform);
    }

    public AudioSource PlaySoundEffect(string filePath, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        if(clip == null)
        {
            Debug.LogError($"Could not load audio file '{filePath}'");
            return null;
        }
        return PlaySoundEffect(clip, mixer, volume, pitch, loop);
    }

    public AudioSource PlaySoundEffect(AudioClip clip, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false)
    {
        AudioSource effectSource = new GameObject(string.Format(SFX_NAME_FORMAT, clip.name)).AddComponent<AudioSource>();
        effectSource.transform.SetParent(sfxRoot);
        effectSource.transform.position = sfxRoot.position;

        effectSource.clip = clip;

        if(mixer == null)
        {
            mixer = sfxMixer;
        }

        effectSource.outputAudioMixerGroup = mixer;
        effectSource.volume = volume;
        effectSource.spatialBlend = 0;
        effectSource.pitch = pitch;
        effectSource.loop = loop;

        effectSource.Play();

        if (!loop)
        {
            //will still destroy properly even if we adjust the pitch
            //the adjusted pitch will alter how quickly the clip ends and then destroys
            Destroy(effectSource.gameObject, (clip.length/pitch) + 1);
        }

        return effectSource;
    }

    public AudioSource PlayVoice(string filePath, float volume = 1, float pitch = 1, bool loop = false)
    {
        return PlaySoundEffect(filePath, voicesMixer, volume, pitch, loop);
    }

    public AudioSource PlayVoice(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false)
    {
       return PlaySoundEffect(clip, voicesMixer, volume, pitch, loop);
    }

    public void StopSoundEffect(AudioClip clip) => StopSoundEffect(clip.name);

    public void StopSoundEffect(string soundName)
    {
        soundName = soundName.ToLower();
        AudioSource[] sources = sfxRoot.GetComponentsInChildren<AudioSource>();
        foreach(var source in sources)
        {
            if(source.clip.name.ToLower() == soundName)
            {
                Destroy(source.gameObject);
                return;
            }
        }
    }

    public AudioTrack PlayTrack(string filePath, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap =1f)
    {
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        if (clip == null)
        {
            Debug.LogError($"Could not load audio file '{filePath}'");
            return null;
        }
        return PlayTrack(clip, filePath, channel, loop, startingVolume, volumeCap);
    }
    
    public AudioTrack PlayTrack(AudioClip clip, string filePath, int channel = 0, bool loop = true, float startingVolume = 0f, float volumeCap =1f)
    {
        AudioChannel audioChannel = TryGetChannel(channel, createIfDoesNotExist: true);
        AudioTrack track = audioChannel.PlayTrack(clip, loop, startingVolume, volumeCap, filePath);
        return track;
    }

    public AudioChannel TryGetChannel(int channelNumber, bool createIfDoesNotExist = false)
    {
        AudioChannel channel = null;
        //search dictionary for the channel
        if(channels.TryGetValue(channelNumber, out channel))
        {
            return channel;
        }
        else if (createIfDoesNotExist)
        {
            return new AudioChannel(channelNumber); ;
        }

        return null;
    }
}
