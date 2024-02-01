using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


//character that uses sprites or sprite sheets to render its display
namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        //the name under anim in unity scene
        public const string SPRITE_RENDERER_PARENT_NAME = "Renderers";
        public const string SPRITESHEET_DEFAULT_SHEETNAME = "Default";
        private const char SPRITESHEET_TEX_SPRITE_DELIMITER = '-';
        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();
        //list of all layers
        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();
        private string artAssetsDirectory = "";
        public override bool isVisible
        {
            get { return isRevealing || rootCG.alpha == 1; }
            set { rootCG.alpha = value ? 1 : 0; }
        }
        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCG.alpha = ENABLE_ON_START ? 1 : 0;
            artAssetsDirectory = rootAssetsFolder + "/Images";
            GetLayers();
            Debug.Log($"Created Sprite Character: '{name}'");
        }

        private void GetLayers()
        {
            //find the child under the renderer name
            Transform rendererRoot = animator.transform.Find(SPRITE_RENDERER_PARENT_NAME);
            if(rendererRoot == null)
            {
                return;
            }
            //loop through children and find the renderers attached to them
            for(int i =0; i < rendererRoot.transform.childCount; i++)
            {
                Transform child = rendererRoot.transform.GetChild(i);
                //check if children have images
                Image rendererImage = child.GetComponentInChildren<Image>();
                if(rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer: {i}";
                }
            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if(config.characterType == CharacterType.SpriteSheet)
            {
                string[] data = spriteName.Split(SPRITESHEET_TEX_SPRITE_DELIMITER);
                Sprite[] spriteArray = new Sprite[0];

                //we have texture and sprite name
                if(data.Length == 2)
                {
                    string texturename = data[0];
                    spriteName = data[1];
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{texturename}");
                }
                //otherwise we have just the sprite name
                else
                {
                    //this gets us every single sprite in the texture
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{SPRITESHEET_DEFAULT_SHEETNAME}");
                }
                if (spriteArray.Length == 0)
                {
                    Debug.Log($"Character '{name}' does not have a default art asset called '{SPRITESHEET_DEFAULT_SHEETNAME}'");
                }

                //if we have texture and sprite, get it like this
                return Array.Find(spriteArray, sprite => sprite.name == spriteName);
            }
            else
            {
                return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName}");
            }
        }

        //gradually change image on a layer
        public Coroutine TransitionSprite(Sprite sprite, int layer =0, float speed = 1)
        {
            CharacterSpriteLayer spriteLayer = layers[layer];
            return spriteLayer.TransitionSprite(sprite, speed);
        }

        public override IEnumerator ShowingOrHiding(bool show)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while(self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f*Time.deltaTime);
                yield return null;
            }
            co_revealing = null;
            co_hiding = null;
        }

        public override void SetColor(Color color)
        {
            base.SetColor(color);

            foreach(CharacterSpriteLayer layer in layers)
            {
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            //access the layer and change the color on it
            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(color, speed);
            }
            yield return null;

            //check if coroutines are still running
            //if any layer is still changing color then wait for it to finish
            while(layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }
            co_changingColor = null;
        }

    }
}