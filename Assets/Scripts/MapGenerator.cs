using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [SerializeField]
    GameObject tile;

    [SerializeField]
    LandObject lowerLandData;
    [SerializeField]
    LandObject midLandData;
    [SerializeField]
    SeaObject seaData;

    public Coordinate mapSize;
    public int maxIslandSize;
    public int maxInnerSize;
    public int islandCount;
    public float decoratorProbability;
    private TileData[,] map;
    private List<int> islandTileCount;
    private int islandIndex;
    public Vector3[] IslandStart { get; private set; }
    private Coroutine currentGeneration;
    private Bounds mMapBounds;
    public Bounds mapBounds {
        get {
            mMapBounds.center = transform.position;
            return mMapBounds;
        }
        private set { mMapBounds = value; }
    }
    public bool isDone
    {
        get;
        private set;
    }
    private int iterCount;
    public int skipEvery;
	// Use this for initialization
	void Awake () {
        GenerateMap();
	}

    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = 4;
        lr.SetPositions(new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(mapSize.x, 0, 0),
            new Vector3(mapSize.x, 0, mapSize.y),
            new Vector3(0, 0, mapSize.y)
        });
    }

    public void GenerateMap()
    {
        mMapBounds = new Bounds(transform.position, Vector3.zero);
        map = new TileData[mapSize.x, mapSize.y];
        IslandStart = new Vector3[islandCount];
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            DecoratorData[] decorators = FindObjectsOfType<DecoratorData>();
            foreach (DecoratorData d in decorators)
            {
                Destroy(d.gameObject);
            }
        }
        currentGeneration = StartCoroutine(GenerateMapHelper(0));
        MakeWorldEdges();
    }

    public void ResetGeneration()
    {
        iterCount = 0;
        StopAllCoroutines();
        isDone = false;
        GenerateMap();
    }

    private IEnumerator GenerateMapHelper(float waitTime = 0.0f)
    {
        islandTileCount = new List<int>();
        islandIndex = 0;
        int tileCount = 0;
        int islands = 0;
        while (islands < islandCount && tileCount < mapSize.x * mapSize.y)
        {
            // Pick random coordinate
            int randX = UnityEngine.Random.Range(1, mapSize.x - 1);
            int randY = UnityEngine.Random.Range(1, mapSize.y - 1);
            if (map[randX, randY].tileObj == null)
            {
                GameObject startTile = MakeTile(randX, randY, TileType.LAND);
                IslandStart[islands] = startTile.transform.position;
                islands++;
                islandTileCount.Add(1);
                yield return Grow(randX, randY, waitTime);
                islandIndex = islandTileCount.Count;
            }
        }

        for (int row = 0; row < mapSize.x; row++)
        {
            for (int col = 0; col < mapSize.y; col++)
            {
                if (map[row, col].tileObj != null)
                {
                    SpriteRenderer spriteRender = map[row, col].tileObj.GetComponentInChildren<SpriteRenderer>();
                    Vector3 rotation;
                    SpriteTileEdgeType spriteType = DetermineEdge(row, col, out rotation);
                    if (spriteType != SpriteTileEdgeType.LAND && spriteType != SpriteTileEdgeType.NUB)
                    {
                        map[row, col].isEdge = true;
                        spriteRender.sprite = lowerLandData.outerTiles[(int)spriteType];
                    }
                    else if (spriteType == SpriteTileEdgeType.NUB)
                    {
                        map[row, col].isEdge = true;
                        spriteRender.sprite = lowerLandData.innerTiles[1];
                        map[row, col].isBoard = true;
                        map[row, col].tileObj.gameObject.layer = LayerMask.NameToLayer("Boarding");
                        map[row, col].tileObj.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    }
                    else
                    {
                        spriteRender.sprite = lowerLandData.innerTiles[0];
                    }
                    
                    if (rotation != Vector3.zero || spriteType == SpriteTileEdgeType.NUB)
                    {
                        rotation = new Vector3(-90f, rotation.y, 0);
                        spriteRender.transform.rotation = Quaternion.Euler(rotation);
                    }
                    if (waitTime > 0 || iterCount++ % skipEvery == 0)
                        yield return new WaitForSeconds(waitTime / 2);
                }
                else
                {
                    MakeTile(row, col, TileType.SEA);
                }
            }

        }

        for (int row = 0; row < mapSize.x; row++)
        {
            for (int col = 0; col < mapSize.y; col++)
            {
                TileData tile = map[row, col];
                if (isLandTile(tile) && !tile.isEdge)
                {
                    SpriteRenderer spriteRender = tile.tileObj.GetComponentInChildren<SpriteRenderer>();
                    SpriteTileInnerType spriteType = DetermineSpriteInnerType(row, col);
                    if (spriteType != SpriteTileInnerType.LAND)
                    {
                        spriteRender.sprite = lowerLandData.edgeTiles[(int)spriteType];
                        if (waitTime > 0 || iterCount++ % skipEvery == 0)
                            yield return new WaitForSeconds(waitTime / 2);
                    }
                }

                if (tile.isDecorated && !tile.isEdge)
                {
                    if (tile.type == TileType.LAND)
                    {
                        int idx = UnityEngine.Random.Range(0, lowerLandData.decorators.Length);
                        DecoratorData d = lowerLandData.decorators[idx].GetComponent<DecoratorData>();
                        if (UnityEngine.Random.value <= d.probability)
                        {
                            Instantiate(lowerLandData.decorators[idx], tile.tileObj.transform.position, lowerLandData.decorators[idx].transform.rotation);
                        }
                    }
                    else if (tile.type == TileType.SEA)
                    {
                        int idx = UnityEngine.Random.Range(0, seaData.decorators.Length);
                        DecoratorData d = seaData.decorators[idx].GetComponent<DecoratorData>();
                        if (UnityEngine.Random.value <= d.probability)
                        {
                            Instantiate(seaData.decorators[idx], tile.tileObj.transform.position, seaData.decorators[idx].transform.rotation);
                        }
                    }
                }
            }
        }

        isDone = true;
    }

    public Vector3 GetRandomLandTile()
    {
        TileData tile = new TileData();
        tile.type = TileType.NONE;
        while(tile.type != TileType.LAND)
        {
            int x = UnityEngine.Random.Range(0, mapSize.x);
            int y = UnityEngine.Random.Range(0, mapSize.y);
            tile = map[x, y];
        }
        return tile.tileObj.transform.position;
    }

    public void GenerateHouse()
    {

    }

    private IEnumerator Grow(int x, int y, float waitTime = 0)
    {
        if (islandTileCount[islandIndex] >= maxIslandSize) yield return null;

        int left = x - 1;
        int right = x + 1;
        int up = y - 1;
        int down = y + 1;
        if (left < 0) left = mapSize.x - 1;
        if (left > 0 && map[left, y].tileObj == null) {
            MakeTile(left, y, TileType.LAND);
            islandTileCount[islandIndex]++;
            if (waitTime > 0 || iterCount++ % skipEvery == 0)
                yield return new WaitForSeconds(waitTime);
        }
        if (right >= mapSize.x) right = 0;
        if (right < mapSize.x && map[right, y].tileObj == null) {
            MakeTile(right, y, TileType.LAND); islandTileCount[islandIndex]++;
            if (waitTime > 0 || iterCount++ % skipEvery == 0)
                yield return new WaitForSeconds(waitTime);
        }
        if (up < 0) up = mapSize.y - 1;
        if (up > 0 && map[x, up].tileObj == null) {
            MakeTile(x, up, TileType.LAND); islandTileCount[islandIndex]++;
            if (waitTime > 0 || iterCount++ % skipEvery == 0)
                yield return new WaitForSeconds(waitTime);
        }
        if (down < 0) down = 0;
        if (down < mapSize.y && map[x, down].tileObj == null) {
            MakeTile(x, down, TileType.LAND); islandTileCount[islandIndex]++;
            if (waitTime > 0 || iterCount++ % skipEvery == 0)
                yield return new WaitForSeconds(waitTime);
        }

        // Pick a direction to grow in
        int growthDirection = UnityEngine.Random.Range(0, 4);
        if (left > 0 && growthDirection == 0) yield return Grow(left, y, waitTime);
        if (right < mapSize.x && growthDirection == 1) yield return Grow(right, y, waitTime);
        if (up > 0 && growthDirection == 2) yield return Grow(x, up, waitTime);
        if (down < mapSize.y && growthDirection == 3) yield return Grow(x, down, waitTime);

        yield return null;
    }

    private GameObject MakeTile(int x, int y, TileType type)
    {
        Vector2 tileSize = tile.GetComponentInChildren<SpriteRenderer>().size;
        GameObject newTile = Instantiate(tile, new Vector3(x: x * tile.transform.localScale.x - 0.05f, y: 0, z: y * tile.transform.localScale.y - 0.05f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
        mMapBounds.Encapsulate(newTile.GetComponentInChildren<SpriteRenderer>().bounds);
        newTile.transform.SetParent(transform);
        newTile.name = String.Format("X: {0}, Y: {1}", x, y);
        //TODO: fix this
        if (type == TileType.LAND)
        {
            newTile.GetComponentInChildren<SpriteRenderer>().sprite = lowerLandData.innerTiles[0];
            newTile.tag = "Land";
            newTile.layer = LayerMask.NameToLayer("Land");
        }
        else if (type == TileType.SEA)
        {
            newTile.GetComponentInChildren<SpriteRenderer>().sprite = seaData.innerTiles[0];
            newTile.tag = "Sea";
            newTile.layer = LayerMask.NameToLayer("Water");
        }
        map[x, y].tileObj = newTile;
        map[x, y].position = new Coordinate(x, y);
        map[x, y].type = type;
        map[x, y].isEdge = false;
        map[x, y].isDecorated = UnityEngine.Random.value <= decoratorProbability;
        return newTile;
    }

    private void MakeWorldEdges()
    {
        GameObject[] edges = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            edges[i] = new GameObject();
            BoxCollider b = edges[i].AddComponent<BoxCollider>();
            switch (i)
            {
                case 0:
                    b.size = new Vector3(1.2f, 10f, mapSize.x * 1.2f);
                    b.transform.position = new Vector3(-0.6f, 0, (mapSize.x / 2));
                    break;
                case 1:
                    b.size = new Vector3(1.2f, 10f, mapSize.x * 1.2f);
                    b.transform.position = new Vector3(mapSize.x, 0, (mapSize.x / 2));
                    break;
                case 2:
                    b.size = new Vector3(mapSize.y * 1.2f, 10f, 1.2f);
                    b.transform.position = new Vector3((mapSize.y / 2), 0, mapSize.y);
                    break;
                case 3:
                    b.size = new Vector3(mapSize.y * 1.2f, 10f, 1.2f);
                    b.transform.position = new Vector3((mapSize.y / 2), 0, -0.6f);
                    break;
            }
            b.transform.SetParent(transform, true);
        }
    }

    private SpriteTileEdgeType DetermineEdge(int x, int y, out Vector3 rotate)
    {
        int left = x - 1;
        int right = x + 1;
        int up = y - 1;
        int down = y + 1;
        bool hasUp = true, 
            hasBottom = true, 
            hasLeft = true, 
            hasRight = true;
        if (left >= 0 && !isLandTile(map[left, y])) { hasLeft = false; }
        if (right < mapSize.x && !isLandTile(map[right, y])) { hasRight = false; }
        if (up >= 0 && !isLandTile(map[x, up])) { hasUp = false; }
        if (down < mapSize.y && !isLandTile(map[x, down])) { hasBottom = false; }

        rotate = Vector3.zero;

        if (hasUp && hasBottom)
        {
            if (hasRight && hasLeft) return SpriteTileEdgeType.LAND;
            else if (hasRight) return SpriteTileEdgeType.EDGE_CENTERLEFT;
            else if (hasLeft) return SpriteTileEdgeType.EDGE_CENTERRIGHT;
        }
        else if (hasUp)
        {
            if (hasRight && hasLeft) return SpriteTileEdgeType.EDGE_BOTTOMCENTER;
            else if (hasRight) return SpriteTileEdgeType.CORNER_BOTTOMLEFT;
            else if (hasLeft) return SpriteTileEdgeType.CORNER_BOTTOMRIGHT;
            else rotate = new Vector3(0, 180, 0);
        }
        else if (hasBottom)
        {
            if (hasRight && hasLeft) return SpriteTileEdgeType.EDGE_UPCENTER;
            else if (hasRight) return SpriteTileEdgeType.CORNER_UPLEFT;
            else if (hasLeft) return SpriteTileEdgeType.CORNER_UPRIGHT;
            
        }
        else
        {
            if (hasRight) rotate = new Vector3(0, 90, 0);
            else if (hasLeft) rotate = new Vector3(0, -90, 0);
        }

        // Nub tiles
        return SpriteTileEdgeType.NUB;
    }

    private SpriteTileInnerType DetermineSpriteInnerType(int x, int y)
    {
        int left = x - 1;
        int right = x + 1;
        int up = y - 1;
        int down = y + 1;
        bool hasUp = false,
            hasBottom = false,
            hasLeft = false,
            hasRight = false,
            hasUpLeft = false,
            hasUpRight = false,
            hasBottomLeft = false,
            hasBottomRight = false;
        if (left > 0 && isEdgeTile(map[left, y])) { hasLeft = true; }
        if (right < mapSize.x && isEdgeTile(map[right, y])) { hasRight = true; }
        if (up > 0 && isEdgeTile(map[x, up])) { hasUp = true; }
        if (down < mapSize.y && isEdgeTile(map[x, down])) { hasBottom = true; }
        if (left > 0 && up > 0 && isEdgeTile(map[left, up])) hasUpLeft = true;
        if (left > 0 && down < mapSize.y && isEdgeTile(map[left, down])) hasBottomLeft = true;
        if (right < mapSize.x && up > 0 && isEdgeTile(map[right, up])) hasUpRight = true;
        if (right < mapSize.x && down < mapSize.y && isEdgeTile(map[right, down])) hasBottomRight = true;


        if (hasUp && hasRight && hasLeft && hasBottom)
        {
            if (hasUpLeft && hasUpRight && hasBottomLeft && hasBottomRight)
                return SpriteTileInnerType.CROSS;
            if (hasUpLeft && hasBottomRight)
                return SpriteTileInnerType.DIAG_LEFT;
            if (hasUpRight && hasBottomLeft)
                return SpriteTileInnerType.DIAG_RIGHT;
            return SpriteTileInnerType.LAND;
        }
        if (hasUp && hasRight && !hasUpRight) return SpriteTileInnerType.INNER_UPRIGHT;
        else if (hasUp && hasLeft && !hasUpLeft) return SpriteTileInnerType.INNER_UPLEFT;
        else if (hasBottom && hasRight && !hasBottomRight) return SpriteTileInnerType.INNER_BOTTOMRIGHT;
        else if (hasBottom && hasLeft && !hasBottomLeft) return SpriteTileInnerType.INNER_BOTTOMLEFT;

        return SpriteTileInnerType.LAND;
    }

    private bool isLandTile(TileData tile)
    {
        return tile.type == TileType.LAND && tile.tileObj != null;
    }
    
    private bool isEdgeTile(TileData tile)
    {
        return tile.isEdge || tile.isBoard;
    }
}

public enum SpriteTileEdgeType
{
    CORNER_UPLEFT,
    EDGE_UPCENTER,
    CORNER_UPRIGHT,
    EDGE_CENTERLEFT,
    EDGE_CENTERRIGHT,
    CORNER_BOTTOMLEFT,
    EDGE_BOTTOMCENTER,
    CORNER_BOTTOMRIGHT,
    NUB,
    LAND
}

public enum SpriteTileInnerType
{
    INNER_BOTTOMRIGHT,
    INNER_BOTTOMLEFT,
    INNER_UPRIGHT,
    INNER_UPLEFT,
    DIAG_RIGHT,
    DIAG_LEFT,
    CROSS,
    LAND
}

public enum TileType
{
    NONE,
    LAND,
    SEA,
    BOARD
}

public enum TileLevel
{
    SEA,
    LOWER,
    MID,
    UPPER
}

[Serializable]
public struct Coordinate
{
    public int x, y;
    public Coordinate (int mX, int mY)
    {
        x = mX;
        y = mY;
    }
}

public struct TileData
{
    public Coordinate position;
    public TileType type;
    public TileLevel level;
    public GameObject tileObj { get; set; }
    public bool isEdge;
    public bool isBoard;
    public bool isDecorated;
}
