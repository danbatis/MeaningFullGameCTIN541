#define PostUnity51

using UnityEditor;
using UnityEngine;

public class DirectorSettingsWindow : EditorWindow
{
    private const string TITLE = "Settings";
    private bool enableBetaFeatures;

    /// <summary>
    /// Sets the window title and minimum pane size
    /// </summary>
    public void Awake()
    {
#if PostUnity51
        base.titleContent = new GUIContent(TITLE);
#else
        base.title = TITLE;
#endif
        this.minSize = new Vector2(250f, 150f);

        if (EditorPrefs.HasKey("DirectorControl.EnableBetaFeatures"))
        {
            enableBetaFeatures = EditorPrefs.GetBool("DirectorControl.EnableBetaFeatures");
        }
    }

    /// <summary>
    /// Draws the Settings GUI
    /// </summary>
    protected void OnGUI()
    {
        bool temp = EditorGUILayout.Toggle(new GUIContent("Enable Beta Features"), enableBetaFeatures);
        if(temp!=enableBetaFeatures)
        {
            enableBetaFeatures = temp;
            EditorPrefs.SetBool("DirectorControl.EnableBetaFeatures", enableBetaFeatures);
        }

        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Apply"))
        {
            EditorWindow.GetWindow<DirectorWindow>().LoadSettings();
        }
    }
}