using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//handles saving, loading, and encryption of files

public class FileManager
{
    //reading from text file found under gameData/
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        //check if we're providing a relative or absolute path
        //forward slash is an absolute path
        if(!filePath.StartsWith('/'))
        {
            //forcing the file path to be where local files are
            filePath = FilePaths.root + filePath;
        }
        //read all the lines in the file
        List<string> lines = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                //while we still have lines to read
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    //check if we're including blank lines
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
        }
        catch(FileNotFoundException ex)
        {
            Debug.LogError($"File not found: '{ex.FileName}'");
        }

        return lines;
    }

    //load text file from resources/
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        //if we don't find the file
        if(asset == null)
        {
            Debug.LogError($"Asset not found: '{filePath}'");
            return null;
        }
        return ReadTextAsset(asset, includeBlankLines);
    }

    //additional variant of method that takes the actual text asset instead of filepath
    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        //asset file is one massive string, so we use string reader instead
        using (StringReader sr = new StringReader(asset.text))
        {
            //check if we are at the end of the line
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                //check if we're including blank lines
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line);
                }
            }
        }
        return lines;
    }
}
