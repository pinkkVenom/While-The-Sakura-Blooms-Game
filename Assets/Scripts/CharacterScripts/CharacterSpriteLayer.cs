using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//contains all data and functions available to a layer composing a sprite character
namespace CHARACTERS
{
    public class CharacterSpriteLayer
    {
        //reference to character manager
        private CharacterManager characterManager => CharacterManager.instance;

        private const float DEFAULT_TRANSITION_SPEED = 3f;
        private float transitionSpeedMultiplier = 1f;
        //our current layer
        public int layer { get; private set; } = 0;
        //the renderer image
        public Image renderer { get; private set; } = null;
        //point to active canvas group
        public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>();

        //COROUTINES/////////////////
        //check if coroutine is running
        private Coroutine co_transitioningLayer = null;
        private Coroutine co_levelingAlpha = null;
        private Coroutine co_changingColor = null;
        private Coroutine co_flipping = null;
        //any newly created layer will have default orientation applied
        private bool isFacingLeft = Character.DEFAULT_ORIENTATION_ISFACING_LEFT;
        public bool isTransitioningLayer => co_transitioningLayer != null;
        public bool isLevelingAlpha => co_levelingAlpha != null;
        public bool isChangingColor => co_changingColor != null;
        public bool isFlipping => co_flipping != null;

        //the inactive renderers
        private List<CanvasGroup> oldRenderers = new List<CanvasGroup>();

        public CharacterSpriteLayer(Image defaultRenderer, int layer = 0)
        {
            renderer = defaultRenderer;
            this.layer = layer;
        }

        public void SetSprite(Sprite sprite)
        {
            renderer.sprite = sprite;
        }

        public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
        {
            //if were already using that sprite, dont do anything
            if(sprite == renderer.sprite)
            {
                return null;
            }
            if (isTransitioningLayer)
            {
                characterManager.StopCoroutine(co_transitioningLayer);
            }
            co_transitioningLayer = characterManager.StartCoroutine(TransitioningSprite(sprite, speed));
            return co_transitioningLayer;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier)
        {
            transitionSpeedMultiplier = speedMultiplier;

            Image newRenderer = CreateRenderer(renderer.transform.parent);
            newRenderer.sprite = sprite;

            yield return TryStartLevelingAlphas();
            co_transitioningLayer = null;
        }

        private Image CreateRenderer(Transform parent)
        {
            Image newRenderer = Object.Instantiate(renderer, parent);
            //add current renderer to the list of old ones
            oldRenderers.Add(rendererCG);

            newRenderer.name = renderer.name;
            renderer = newRenderer;
            renderer.gameObject.SetActive(true);
            rendererCG.alpha = 0;

            return newRenderer;
        }

        //to fade in new images and fade out old ones
        private Coroutine TryStartLevelingAlphas()
        {
            if (isLevelingAlpha)
            {
                characterManager.StopCoroutine(co_levelingAlpha);
            }
            co_levelingAlpha = characterManager.StartCoroutine(RunAlphaLeveling());
            return co_levelingAlpha;
        }


        private IEnumerator RunAlphaLeveling()
        {
            while(rendererCG.alpha < 1 || oldRenderers.Any(oldCG => oldCG.alpha > 0))
            {
                //fade in new renderer
                float speed = DEFAULT_TRANSITION_SPEED * transitionSpeedMultiplier * Time.deltaTime;
                rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);

                //fade out old renderers
                for(int i = oldRenderers.Count - 1; i >= 0; i--)
                {
                    CanvasGroup oldCG = oldRenderers[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);

                    if(oldCG.alpha <= 0)
                    {
                        oldRenderers.RemoveAt(i);
                        Object.Destroy(oldCG.gameObject);
                    }
                }
                yield return null;
            }
            co_levelingAlpha = null;
        }

        //set color for specific layer
        public void SetColor(Color color)
        {
            renderer.color = color;

            foreach(CanvasGroup oldCG in oldRenderers)
            {
                oldCG.GetComponent<Image>().color = color;
            }
        }

        //start transitioning the color in the layer
        public Coroutine TransitionColor(Color color, float speed)
        {
            if (isChangingColor)
            {
                characterManager.StopCoroutine(co_changingColor);
            }
            co_changingColor = characterManager.StartCoroutine(ChangingColor(color, speed));
            return co_changingColor;
        }

        public void StopChangingColor()
        {
            if (!isChangingColor)
            {
                return;
            }
            characterManager.StopCoroutine(co_changingColor);
            co_changingColor = null;
        }

        private IEnumerator ChangingColor(Color color, float speedMultiplier)
        {
            Color oldColor = renderer.color;
            List<Image> oldImages = new List<Image>();

            //cache all old images and set the color afterwards
            foreach(var oldCG in oldRenderers)
            {
                oldImages.Add(oldCG.GetComponent<Image>());
            }

            //lerp the color so it fades in
            float colorPercent = 0;
            while(colorPercent < 1)
            {
                colorPercent += DEFAULT_TRANSITION_SPEED * speedMultiplier * Time.deltaTime;
                renderer.color = Color.Lerp(oldColor, color, colorPercent);

                //set new color for old images
                for(int i = oldImages.Count - 1; i>=0; i--)
                {
                    Image image = oldImages[i];
                    if(image != null)
                    {
                        image.color = renderer.color;
                    }
                    else
                    {
                        oldImages.RemoveAt(i);
                    }
                }
                yield return null;
            }
            co_changingColor = null;
        }

        //flip sprite regardless of the direction they're facing
        //this is for layer specific flipping
        public Coroutine Flip(float speed = 1f, bool immediate = false)
        {
            if (isFacingLeft)
            {
                return FaceLeft(speed, immediate);
            }
            else
            {
                return FaceRight(speed, immediate);
            }
        }
        public Coroutine FaceLeft(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
            {
                characterManager.StopCoroutine(co_flipping);
            }
            isFacingLeft = true;
            co_flipping = characterManager.StartCoroutine(FaceDirection(isFacingLeft, speed, immediate));
            return co_flipping;
        }
        public Coroutine FaceRight(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
            {
                characterManager.StopCoroutine(co_flipping);
            }
            isFacingLeft = false;
            co_flipping = characterManager.StartCoroutine(FaceDirection(isFacingLeft, speed, immediate));
            return co_flipping;
        }
        private IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            //flip x if were facing right
            float xScale = faceLeft ? 1 : -1;
            Vector3 newScale = new Vector3(xScale, 1, 1);

            if (!immediate)
            {
                Image newRenderer = CreateRenderer(renderer.transform.parent);
                newRenderer.transform.localScale = newScale;

                //level all alphas to change active renderer
                transitionSpeedMultiplier = speedMultiplier;
                TryStartLevelingAlphas();

                while (isLevelingAlpha)
                {
                    yield return null;
                }
            }
            else
            {
                renderer.transform.localScale = newScale;
            }
            co_flipping = null;
        }

    }
}
