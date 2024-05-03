using Assets.__Game.Scripts.Managers;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
  public class SettingsEditorWindow : EditorWindow
  {
    [MenuItem("Tools/Reset Game Settings")]
    public static void OpenWindow()
    {
      SettingsEditorWindow window = (SettingsEditorWindow)GetWindow(typeof(SettingsEditorWindow));

      window.titleContent = new GUIContent("Reset Game Settings");
      window.Show();
    }

    private void OnGUI()
    {
      GUILayout.Label("Are you sure you want to reset settings?");

      if (GUILayout.Button("Reset"))
      {
        SettingsManager.ResetSettings();
        Debug.Log("Settings are reset");  
      }
    }
  }
}
