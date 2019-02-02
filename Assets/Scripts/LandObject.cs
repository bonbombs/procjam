using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Land", menuName = "LandData", order = 1)]
public class LandObject : ScriptableObject
{

    public Sprite[] innerTiles;
    public Sprite[] outerTiles;
    public Sprite[] edgeTiles;

    public GameObject[] decorators;
}