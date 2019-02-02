using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGenerator : MonoBehaviour {

    [SerializeField]
    public GameObject houseRenderer;
    [SerializeField]
    public SpriteRenderer houseTile;

    public int maxSize;
    public int minSize;

    [SerializeField]
    Sprite[] roofSprites;

    [SerializeField]
    Sprite[] baseSprites;

    [SerializeField]
    Sprite[] textureSprites;

    [SerializeField]
    Sprite[] doorSprites;

    [SerializeField]
    ColorTheme[] themes;

	public GameObject BuildHouse()
    {
        int size = UnityEngine.Random.Range(minSize, maxSize);
        for (int i = 0; i < size; i++)
        {
            Instantiate(houseTile);
        }

        return houseRenderer;
    }
}

[System.Serializable]
public struct ColorTheme
{
    public Color primary;
    public Color secondary;
    public Color tertiary;
}
