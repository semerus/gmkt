using GiraffeStar;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConfigAsset))]
public class ConfigAssetInspector : Editor
{

    SerializedProperty listProp;

    void OnEnable()
    {
        listProp = serializedObject.FindProperty("Configurations");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.OnInspectorGUI();

        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Add Item"))
        {
            listProp.InsertArrayElementAtIndex(listProp.arraySize);
            var addedItem = listProp.GetArrayElementAtIndex(listProp.arraySize - 1);
            addedItem.FindPropertyRelative("IsOn").boolValue = true;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("On", GUILayout.Width(30f));
        EditorGUILayout.LabelField("Key", GUILayout.Width(150f));
        EditorGUILayout.LabelField("Value", GUILayout.Width(300f));
        EditorGUILayout.LabelField("Bool", GUILayout.Width(30f));
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < listProp.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            var item = listProp.GetArrayElementAtIndex(i);
            var isOnItem = item.FindPropertyRelative("IsOn");
            var keyItem = item.FindPropertyRelative("Key");
            var valueItem = item.FindPropertyRelative("Value");
            isOnItem.boolValue = EditorGUILayout.Toggle(isOnItem.boolValue, GUILayout.Width(30f));
            keyItem.stringValue = EditorGUILayout.TextField(keyItem.stringValue, GUILayout.Width(150f));
            valueItem.stringValue = EditorGUILayout.TextField(valueItem.stringValue, GUILayout.Width(300f));

            var boolValue = false;
            if (valueItem.stringValue.Equals("True"))
            {
                boolValue = true;
            }
            var cached = EditorGUILayout.Toggle(boolValue, GUILayout.Width(30f));
            if (boolValue != cached)
            {
                // on value change
                valueItem.stringValue = cached ? "True" : "False";
            }

            if (GUILayout.Button("Delete"))
            {
                listProp.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    // ToolBar
    //=======================================================================================================
    [MenuItem("GiraffeStar/Config/New Config Asset")]
    public static ConfigAsset CreateConfigAsset()
    {
        var asset = ScriptableObject.CreateInstance<ConfigAsset>();

        AssetDatabase.CreateAsset(asset, "Assets/Config/Config.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}