using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//contains layers that can be assigned images and videos on the UI

[System.Serializable]
public class GraphicPanel
{
    public string panelName;
    public GameObject rootPanel;
    public List<GraphicLayer> layers { get; private set; } = new List<GraphicLayer>();

    public bool isClear => layers == null || layers.Count == 0 || layers.All(layer => layer.currentGraphic == null);

    //see if we have a layer and return if it exists
    public GraphicLayer GetLayer(int layerDepth, bool createIfDoesNotExist = false)
    {
        for(int i = 0; i < layers.Count; i++)
        {
            if(layers[i].layerDepth == layerDepth)
            {
                return layers[i];
            }
        }

        if (createIfDoesNotExist)
        {
            return CreateLayer(layerDepth);
        }
        return null;
    }

    //create layer
    private GraphicLayer CreateLayer(int layerDepth)
    {
        GraphicLayer layer = new GraphicLayer();
        GameObject panel = new GameObject(string.Format(GraphicLayer.LAYER_OBJECT_NAME_FORMAT, layerDepth));
        RectTransform rect = panel.AddComponent<RectTransform>();
        panel.AddComponent<CanvasGroup>();
        panel.transform.SetParent(rootPanel.transform, false);

        //manually set transform values upon creation
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.one;

        layer.panel = panel.transform;
        layer.layerDepth = layerDepth;

        //make sure layers are created in the right order
        int index = layers.FindIndex(l => l.layerDepth > layerDepth);
        if(index == -1)
        {
            layers.Add(layer);
        }
        else
        {
            layers.Insert(index, layer);
        }

        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].panel.SetSiblingIndex(layers[i].layerDepth);
        }

        return layer;
    }

    //clears out multiple layers
    public void Clear(float transitionSpeed = 1, Texture blendTexture = null, bool immediate = false)
    {
        foreach (var layer in layers)
        {
            layer.Clear(transitionSpeed, blendTexture, immediate);
        }
    }
}
