using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("setlayermedia", new Func<string[], IEnumerator>(SetLayerMedia));
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

            parameters.TryGetValue(PARAM_MEDIA, out mediaName);
        }
    }
}
