using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;



public sealed class InterfaceBar : MonoBehaviour
{
  [SerializeField]
  private bool guiShow = true;


  [SerializeField]
  private Text text = null;

  [SerializeField]
  private string credits = "";

  private enum Presets
  {
    One,
    Two,
    Three
  }
  

  private bool menuOpen = false;

  private const float guiMargen = 10.0f;
  private const float guiWidth = 425.0f;

  private float updateInterval = 0.5f;
  private float accum = 0.0f;
  private int frames = 0;
  private float timeleft;
  private float fps = 0.0f;

  private GUIStyle effectNameStyle;
  private GUIStyle menuStyle;
  private GUIStyle boxStyle;
  private GUIStyle creditsStyle;

  private int toolbarMultipass = 0;
  private int toolbarRenderSize = 0;
  private int toolbarColorControl = 1;
  private int toolbarDistortion = 2;

  private Vector2 scrollPosition = Vector2.zero;

  private AudioSource audioSourceFX; 
  private AudioSource audioSourceMusic; 
  private AudioSource audioSourceBreath;

  private Presets preset = Presets.Three;
  private IEnumerator presetCoroutine;  
  
  private IEnumerator FadeImage(bool fadeIn)
  {
    float time = 1.0f;
    while (time > 0.0f)
    {
      text.color = new Color(1.0f, 1.0f, 1.0f, fadeIn == true ? 1.0f - time : time);

      time -= Time.deltaTime;
          
      yield return null;
    }

    text.color = new Color(1.0f, 1.0f, 1.0f, fadeIn == true ? 1.0f : 0.0f);
  }

  
  
  private void Update()
  {
    timeleft -= Time.deltaTime;
    accum += Time.timeScale / Time.deltaTime;
    frames++;

    if (timeleft <= 0.0f)
    {
      fps = accum / frames;
      timeleft = updateInterval;
      accum = 0.0f;
      frames = 0;
    }

    if (Input.GetKeyUp(KeyCode.Tab) == true)
      guiShow = !guiShow;

  }

  private void OnGUI()
  {

    if (effectNameStyle == null)
    {
      effectNameStyle = new GUIStyle(GUI.skin.textArea)
      {
        alignment = TextAnchor.MiddleCenter,
        fontSize = 22
      };
    }

    if (menuStyle == null)
      menuStyle = new GUIStyle(GUI.skin.textArea)
      {
        normal = { background = MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 0.5f)) },
        alignment = TextAnchor.MiddleCenter,
	      fontSize = 14
      };

    if (boxStyle == null)
    {
      boxStyle = new GUIStyle(GUI.skin.box);
      boxStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 0.5f));
      boxStyle.focused.textColor = Color.red;
    }

    if (creditsStyle == null)
    {
      creditsStyle = new GUIStyle(GUI.skin.label);
      creditsStyle.wordWrap = true;
    }

    if (guiShow == false)
      return;

    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(Screen.width));
    {
      GUILayout.Space(guiMargen);

      //if (GUILayout.Button("OPTIONS", menuStyle, GUILayout.Width(80.0f)) == true)
      //  menuOpen = !menuOpen;

      GUILayout.FlexibleSpace();

	    GUILayout.Label(" HEADLINE TITLE", menuStyle, GUILayout.Width(200.0f));

      GUILayout.FlexibleSpace();

      //if (GUILayout.Button("MUTE", menuStyle, GUILayout.Width(50.0f)) == true)
      //  AudioListener.volume = 1.0f - AudioListener.volume;

      //GUILayout.Space(guiMargen);
      
      if (fps < 30.0f)
        GUI.contentColor = Color.yellow;
      else if (fps < 15.0f)
        GUI.contentColor = Color.red;
      else
        GUI.contentColor = Color.green;

      GUILayout.Label(fps.ToString("000"), menuStyle, GUILayout.Width(40.0f));

      GUI.contentColor = Color.white;

      GUILayout.Space(guiMargen);
    }
    GUILayout.EndHorizontal();

  //  if (menuOpen == true)
  //  {
  //    GUILayout.BeginVertical(boxStyle, GUILayout.Width(guiWidth));
  //    {
  //      scrollPosition = GUILayout.BeginScrollView(scrollPosition);

  //      GUILayout.Space(guiMargen);

  //      // ON / OFF
  //      GUILayout.BeginVertical(boxStyle);
  //      {
  //        GUILayout.BeginHorizontal();
  //        {
  //        }
  //        GUILayout.EndHorizontal();
  //      }
  //      GUILayout.EndHorizontal();

  //      GUILayout.Space(guiMargen);

  //      // Presets
  //    GUILayout.EndVertical();
  //  }
  //}
  }

  
  private Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];
    for (int i = 0; i < pix.Length; ++i)
      pix[i] = col;

    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();

    return result;
  }
}

