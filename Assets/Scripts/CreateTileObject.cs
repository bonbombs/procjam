using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class MakeLandTileObject
{
    [MenuItem("Assets/Create/TileData/Land")]
    public static void CreateMyAsset()
    {
        LandObject asset = ScriptableObject.CreateInstance<LandObject>();

        AssetDatabase.CreateAsset(asset, "Assets/LandData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}

public class MakeSeaTileObject
{
    [MenuItem("Assets/Create/TileData/Sea")]
    public static void CreateMyAsset()
    {
        SeaObject asset = ScriptableObject.CreateInstance<SeaObject>();

        AssetDatabase.CreateAsset(asset, "Assets/SeaData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
#endif