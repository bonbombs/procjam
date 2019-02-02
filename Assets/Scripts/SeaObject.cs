using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sea", menuName = "SeaData", order = 2)]
public class SeaObject : ScriptableObject
{

    public Sprite[] innerTiles;

    public GameObject[] decorators;
}
