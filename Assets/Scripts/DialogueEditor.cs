using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class MakeDialogueDataObject
{
    [MenuItem("Assets/Create/DialogueData")]
    public static void CreateMyAsset()
    {
        DialogueData asset = ScriptableObject.CreateInstance<DialogueData>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Dialogue.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}

public class MakeQuestObject
{
    [MenuItem("Assets/Create/Quest")]
    public static void CreateMyAsset()
    {
        Quest asset = ScriptableObject.CreateInstance<Quest>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/Quest.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}

public class MakeItemObject
{
    [MenuItem("Assets/Create/Item")]
    public static void CreateMyAsset()
    {
        ItemData asset = ScriptableObject.CreateInstance<ItemData>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/ItemData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}

public class MakeFriendDataObject
{
    [MenuItem("Assets/Create/FriendData")]
    public static void CreateMyAsset()
    {
        FriendData asset = ScriptableObject.CreateInstance<FriendData>();

        AssetDatabase.CreateAsset(asset, "Assets/Data/FriendData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
#endif
