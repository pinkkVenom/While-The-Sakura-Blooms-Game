using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
//single layer on a graphic panel that can be assigned an image or video and stacked on other layers

public class GraphicLayer
{
    public const string LAYER_OBJECT_NAME_FORMAT = "Layer: {0}";
    public int layerDepth = 0;
    public Transform panel;

    public GraphicObject currentGraphic = null;
    private List<GraphicObject> oldGraphics = new List<GraphicObject>();
   
    //create texture through filepath
    public void SetTexture(string filePath, float transitionSpeed = 1f, Texture blendingTexture = null)
    {
        Texture tex = Resources.Load<Texture>(filePath);

        if (tex == null)
        {
            Debug.LogError($"Could not load graphic texture from path '{filePath}'");
            return;
        }
        SetTexture(tex, transitionSpeed, blendingTexture, filePath);
    }

    //create texture 
    public void SetTexture(Texture tex, float transitionSpeed = 1f, Texture blendingTexture = null, string filePath = "")
    {
        CreateGraphic(tex, transitionSpeed, filePath, blendingTexture: blendingTexture);
    }

    //to set videos
    public void SetVideo(string filePath, float transitionSpeed = 1f, bool useAudio= true,  Texture blendingTexture = null)
    {
        VideoClip clip = Resources.Load<VideoClip>(filePath);

        if (clip == null)
        {
            Debug.LogError($"Could not load graphic video from path '{filePath}'");
            return;
        }
        SetVideo(clip, transitionSpeed, useAudio, blendingTexture, filePath);
    }
    //set video texture 
    public void SetVideo(VideoClip clip, float transitionSpeed = 1f, bool useAudio = true, Texture blendingTexture = null, string filePath = "")
    {
        CreateGraphic(clip, transitionSpeed, filePath, useAudio, blendingTexture);
    }

    public void CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true, Texture blendingTexture = null)
    {
        GraphicObject newGraphic = null;

        if (graphicData is Texture)
        {
            newGraphic = new GraphicObject(this, filePath, graphicData as Texture);
        }
        else if(graphicData is VideoClip)
        {
            newGraphic = new GraphicObject(this, filePath, graphicData as VideoClip, useAudioForVideo);
        }

        if(currentGraphic != null && !oldGraphics.Contains(currentGraphic))
        {
            oldGraphics.Add(currentGraphic);
        }

        currentGraphic = newGraphic;

        currentGraphic.FadeIn(transitionSpeed, blendingTexture);
    }

    //destroy all old contents inside layers
    public void DestroyOldGraphics()
    {
        foreach(var g in oldGraphics)
        {
            Object.Destroy(g.renderer.gameObject);
        }

        oldGraphics.Clear();
    }

    //clear the layer
    public void Clear()
    {
        if (currentGraphic != null)
        {
            currentGraphic.FadeOut();
        }
        foreach(var g in oldGraphics)
        {
            g.FadeOut();
        }
    }
}
