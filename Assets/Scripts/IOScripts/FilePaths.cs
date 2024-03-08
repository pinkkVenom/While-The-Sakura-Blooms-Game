using UnityEngine;
//access for files and directory locations
//helps eliminate the duplication of hard coded strings

public class FilePaths
{
    private const string HOME_DIRECTORY_SYMBOL = "~/";
    //define root path for project (even once we have executable file)
    public static readonly string root = $"{Application.dataPath}/gameData/";

    //public static readonly string gameSaves = $"{runtimePath}Save Files/";

    //Resources Paths
    public static readonly string resources_graphics = "Graphics/";
    public static readonly string resources_fonts = "Fonts/";
    public static readonly string resources_backgroundImages = $"{resources_graphics}BG Images/";
    public static readonly string resources_backgroundVideos = $"{resources_graphics}BG Videos/";
    public static readonly string resources_blendTextures = $"{resources_graphics}Transition Effects/";

    public static readonly string resources_audio = "Audio/";
    public static readonly string resources_sfx = $"{resources_audio}SFX/";
    public static readonly string resources_voices = $"{resources_audio}Voices/";
    public static readonly string resources_music = $"{resources_audio}Music/";

    public static readonly string resources_dialogueFiles = $"Dialogue Files/";


    //returns path to resources using default path or the root of resources folder
    public static string GetPathToResources(string defaultPath, string resourceName)
    {
        if (resourceName.StartsWith(HOME_DIRECTORY_SYMBOL))
        {
            return resourceName.Substring(HOME_DIRECTORY_SYMBOL.Length);
        }
        return defaultPath + resourceName;
    }
}
