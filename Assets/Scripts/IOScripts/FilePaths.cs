using UnityEngine;
//access for files and directory locations
//helps eliminate the duplication of hard coded strings

public class FilePaths
{
    //define root path for project (even once we have executable file)
    public static readonly string root = $"{Application.dataPath}/gameData/";

    //Resources Paths
    public static readonly string resources_graphics = "Graphics/";
    public static readonly string resources_backgroundImages = $"{resources_graphics}BG Images/";
    public static readonly string resources_backgroundVideos = $"{resources_graphics}BG Videos/";
    public static readonly string resources_blendTextures = $"{resources_graphics}Transition Effects/";
}
