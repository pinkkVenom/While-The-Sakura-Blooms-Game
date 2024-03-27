using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

namespace COMMAND
{
    public class CMD_DatabaseExtension_GraphicPanels: CMD_DatabaseExtension
    {
        private static string[] PARAM_PANEL = new string[] { "-p", "-panel"};
        private static string[] PARAM_LAYER = new string[] { "-l", "-layer"};
        private static string[] PARAM_MEDIA = new string[] { "-m", "-media"};
        private static string[] PARAM_SPEED = new string[] { "-spd", "-speed"};
        private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate"};
        private static string[] PARAM_BLENDTEX = new string[] { "-b", "-blend"};
        private static string[] PARAM_USEVIDEOAUDIO = new string[] { "-aud", "-audio"};

        private const string HOMEDIRECTORY_SYMBOL = "~/";
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("setlayermedia", new Func<string[], IEnumerator>(SetLayerMedia));
            database.AddCommand("clearlayermedia", new Func<string[], IEnumerator>(ClearLayerMedia));
        }

        private static IEnumerator SetLayerMedia(string[] data)
        {
            string panelName = "";
            int layer = 0;
            string mediaName = "";
            float transitionSpeed = 0;
            bool immediate = false;
            string blendTextName = "";
            bool useAudio = true;

            string pathToGraphic = "";
            UnityEngine.Object graphic = null;
            Texture blendTex = null;

            //get parameters
            var parameters = ConvertDataToParameters(data);

            //get the panel that this media is applied to
            parameters.TryGetValue(PARAM_PANEL, out panelName);
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
            if(panel == null)
            {
                Debug.LogError($"Unable to grab panel '{panelName}' because it is not valid.");
                yield break;
            }

            //try to get layer to apply graphic to
            parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: 0);

            //try to get the graphic/media
            parameters.TryGetValue(PARAM_MEDIA, out mediaName);

            //try to see if this is an immediate effect
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            //if were not using immediate, try to find the speed of the transition
            if (!immediate)
            {
                parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1f);
            }

            //find blend texture if we're using one
            parameters.TryGetValue(PARAM_BLENDTEX, out blendTextName);

            //if we are loading video, check to see if we are using sound
            parameters.TryGetValue(PARAM_USEVIDEOAUDIO, out useAudio, defaultValue: false);

            pathToGraphic = FilePaths.GetPathToResources(FilePaths.resources_backgroundImages, mediaName);
            //try to find it as an image first, then a video
            graphic = Resources.Load<Texture>(pathToGraphic);
            if(graphic == null)
            {
                pathToGraphic = FilePaths.GetPathToResources(FilePaths.resources_backgroundVideos, mediaName);
                graphic = Resources.Load<VideoClip>(pathToGraphic);
            }
            //if we are still null then media doesnt exist
            if(graphic == null)
            {
                Debug.LogError($"Could not find media file called '{mediaName}'");
                yield break;
            }

            //add the blend texture if we arent immediately transitioning and we have a texture
            if(!immediate && blendTextName != string.Empty)
            {
                blendTex = Resources.Load<Texture>(FilePaths.resources_blendTextures + blendTextName);
            }

            //get the layer to apply media, and create one if we dont have one
            GraphicLayer graphicLayer = panel.GetLayer(layer, createIfDoesNotExist: true);
            if(graphic is Texture)
            {
                if (!immediate)
                {
                    CommandManager.instance.AddTerminationActionToCurrentProcess(() => { graphicLayer?.SetTexture(graphic as Texture, filePath: pathToGraphic, immediate: true); });
                }
                yield return graphicLayer.SetTexture(graphic as Texture, transitionSpeed, blendTex, pathToGraphic, immediate);
            }
            else
            {
                if (!immediate)
                {
                    CommandManager.instance.AddTerminationActionToCurrentProcess(() => { graphicLayer?.SetVideo(graphic as VideoClip, filePath: pathToGraphic, immediate: true); });
                }
                yield return graphicLayer.SetVideo(graphic as VideoClip, transitionSpeed, useAudio, blendTex, pathToGraphic, immediate);
            }
        }

        private static IEnumerator ClearLayerMedia(string[] data)
        {
            string panelName = "";
            int layer = 0;
            float transitionSpeed = 0;
            bool immediate = false;
            string blendTextName = "";

            Texture blendTex = null;

            //get parameters
            var parameters = ConvertDataToParameters(data);

            //get the panel that this media is applied to
            parameters.TryGetValue(PARAM_PANEL, out panelName);
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);
            if (panel == null)
            {
                Debug.LogError($"Unable to grab panel '{panelName}' because it is not valid.");
                yield break;
            }

            //try to get layer 
            parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: -1);

            //try to see if this is an immediate effect
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            //if were not using immediate, try to find the speed of the transition
            if (!immediate)
            {
                parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1);
            }

            //find blend texture if we're using one
            parameters.TryGetValue(PARAM_BLENDTEX, out blendTextName);
            //add the blend texture if we arent immediately transitioning and we have a texture
            if (!immediate && blendTextName != string.Empty)
            {
                blendTex = Resources.Load<Texture>(FilePaths.resources_blendTextures + blendTextName);
            }

            //clearing the layer
            if(layer == -1)
            {
                panel.Clear(transitionSpeed, blendTex, immediate);
            }
            else
            {
                GraphicLayer graphicsLayer = panel.GetLayer(layer);
                if(graphicsLayer == null)
                {
                    Debug.LogError($"Could not clear layer [{layer}] on panel '{panel.panelName}'");
                    yield break;
                }
                graphicsLayer.Clear(transitionSpeed, blendTex, immediate);
            }
        }

        private static string GetPathToGraphic(string defaultPath, string graphicName)
        {
            if (graphicName.StartsWith(HOMEDIRECTORY_SYMBOL))
            {
                return graphicName.Substring(HOMEDIRECTORY_SYMBOL.Length);
            }
            return defaultPath + graphicName;
        }
    }
}
