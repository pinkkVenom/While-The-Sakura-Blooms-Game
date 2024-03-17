using System.Collections;
using UnityEngine;
using TMPro;

//this code is responsible for building and showing text in a dynamic way (typewriter style)

public class TextManager
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;
    //get either UI or world text, depending on which one is assigned
    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;

    //what is on textmeshpro right now
    public string currentText => tmpro.text;
    //publicly retrievable, privately assignable
    public string targetText { get; private set; } = "";
    //stored before new text is displayed
    public string preText { get; private set; } = "";
    private int preTextLength = 0;

    //combination of stored text and the target text
    public string fullTargetText => preText + targetText;

    //default is typewriter
    public enum BuildMethod { instant, typewriter, fade}
    public BuildMethod buildMethod = BuildMethod.typewriter;

    //shortcut for changing text color
    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    //obtaining/setting speed of text generation
    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }
    //speed of text generation
    private const float baseSpeed = 1;
    private float speedMultiplier = 1;

    //allow more charactes to appear per frame
    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5 ? characterMultiplier * 2 : characterMultiplier * 3; } }
    private int characterMultiplier = 1;

    //quickly clicking through text
    public bool hurryUpText = false;


    //constructors /////////////////////////////////////////////////////////
    public TextManager(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui;
    }
    public TextManager(TextMeshPro tmpro_world)
    {
        this.tmpro_world = tmpro_world;
    }

    //Coroutines////////////////////////////////////////////////////////////////////////////
    //delay actions until coroutines for textmanager are complete
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        //make sure there is no active build happening
        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    //appends text to what is already stored in text manager
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        //make sure there is no active build happening
        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }
    //check if there is already a build happening
    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;
    public void Stop()
    {
        if(!isBuilding)
        {
            return;
        }
        //stops the coroutine and sets buildprocess back to null
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    //the build process
    IEnumerator Building()
    {
        Prepare();
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                //wait for method to finish
                yield return Build_Typewriter();
                break;
            case BuildMethod.fade:
                //wait for method to finish
                yield return Build_Fade();
                break;

        }
        OnComplete();
    }

    
    //runs when build process is done
    private void OnComplete()
    {
        buildProcess = null;
        hurryUpText = false;
    }

    //allows the hurryUpText to alter the speed of characters appearing on screen
    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                tmpro.ForceMeshUpdate();
                break;
        }
        Stop();
        OnComplete();
    }

    //prepares which type of text builder is used to prevent any bugs in text appearing on screen
    private void Prepare()
    {
        switch(buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_TypeWriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }
    private void Prepare_Instant()
    {
        //reset color first since we're working with text alpha
        tmpro.color = tmpro.color;
        //get all the text we are trying to set
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        //make sure every character is visible on screen
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }
    private void Prepare_TypeWriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        //making sure there is pretext
        if(preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }
    private void Prepare_Fade()
    {
        tmpro.text = preText;
        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            preTextLength = tmpro.textInfo.characterCount;
        }
        else
        {
            //if there is no pretext
            preTextLength = 0;
        }

        tmpro.text = targetText;
        //fade wont use max visible characters, so we set it without limit
        tmpro.maxVisibleCharacters = int.MaxValue;
        tmpro.ForceMeshUpdate();

        //we need 2 colors to change the text from hidden to visible
        TMP_TextInfo textInfo = tmpro.textInfo;

        Color colorVisible = new Color(textColor.r, textColor.g, textColor.b, 1);
        Color colorHidden = new Color(textColor.r, textColor.g, textColor.b, 0);

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;

        for(int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            //checks to make sure first character in text will also have the fade effect applied
            if(!charInfo.isVisible)
            {
                continue;
            }

            if(i < preTextLength)
            {
                //for each vertex in pretext, set color as visible
                for(int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v] = colorVisible;
                }
            }
            else
            {
                //for each vertex not in pretext, set color as hidden
                for (int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v] = colorHidden;
                }
            }
        }
        tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    //loops through every character and makes it appear on screen
    private IEnumerator Build_Typewriter()
    {
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUpText ? charactersPerCycle * 5 : charactersPerCycle;
            yield return new WaitForSeconds(0.015f / speed);
        }
    }
    //loops through every character and fades it in
    private IEnumerator Build_Fade()
    {
        int minRange = preTextLength;
        int maxRange = minRange + 1;

        byte alphaThreshold = 15;

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;
        //will allow color to fade differently since color32 cant be converted to float
        float[] alphas = new float[textInfo.characterCount];

        while (true)
        {
            float fadeSpeed = ((hurryUpText ? charactersPerCycle * 5 : charactersPerCycle) * speed) *4f;

            for(int i = minRange; i < maxRange; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                //checks to make sure first character in text will also have the fade effect applied
                if (!charInfo.isVisible)
                {
                    continue;
                }
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                alphas[i] = Mathf.MoveTowards(alphas[i], 255, fadeSpeed);
                for (int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v].a = (byte)alphas[i];
                }
                if(alphas[i] >= 255)
                {
                    minRange++;
                }
            }
            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            //check to make sure last character is visible, if its invisible then we just skip it
            bool lastCharacterIsInvisible = !textInfo.characterInfo[maxRange - 1].isVisible;
            if(alphas[maxRange-1] > alphaThreshold || lastCharacterIsInvisible)
            {
                if(maxRange < textInfo.characterCount)
                {
                    maxRange++;
                }
                //if the last character has reached max alpha value or is invisible, then exit building coroutine
                else if (alphas[maxRange - 1] >= 255 || lastCharacterIsInvisible)
                {
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
