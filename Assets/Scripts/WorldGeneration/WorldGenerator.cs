using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap groundTilemap;

    [Header("Original 9 Terrain Tiles")]
    [SerializeField] private TileBase topLeftTile;
    [SerializeField] private TileBase topTile;
    [SerializeField] private TileBase topRightTile;

    [SerializeField] private TileBase leftTile;
    [SerializeField] private TileBase centerTile;
    [SerializeField] private TileBase rightTile;

    [SerializeField] private TileBase bottomLeftTile;
    [SerializeField] private TileBase bottomTile;
    [SerializeField] private TileBase bottomRightTile;

    [Header("Inner Corner Tiles")]
    [SerializeField] private TileBase innerTopLeftTile;
    [SerializeField] private TileBase innerTopRightTile;
    [SerializeField] private TileBase innerBottomLeftTile;
    [SerializeField] private TileBase innerBottomRightTile;

    [Header("World Size")]
    [SerializeField] private int worldWidth = 256;
    [SerializeField] private int groundDepth = 12;

    [Header("Terrain Height")]
    [SerializeField] private int startingGroundHeight = 8;
    [SerializeField] private int minimumGroundHeight = -12;
    [SerializeField] private int maximumGroundHeight = 14;

    [Header("Terrain Feature Size")]
    [SerializeField] private int minFeatureWidth = 20;
    [SerializeField] private int maxFeatureWidth = 45;

    [Header("Height Changes")]
    [SerializeField] private int maxFeatureHeightChange = 2;

    [Range(0f, 1f)]
    [SerializeField] private float downwardChance = 0.60f;

    [Header("Flat Combat Areas")]
    [Range(0f, 1f)]
    [SerializeField] private float combatAreaChance = 0.25f;

    [SerializeField] private int minCombatAreaWidth = 20;
    [SerializeField] private int maxCombatAreaWidth = 35;

    [Header("Testing")]
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private int seed = 12345;

    private bool[,] solidTerrain;

    private int terrainBottom;
    private int terrainTop;
    private int terrainHeight;

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        if (groundTilemap == null)
        {
            Debug.LogError(
                "WorldGenerator: Ground Tilemap is not assigned!"
            );
            return;
        }

        if (centerTile == null || topTile == null)
        {
            Debug.LogError(
                "WorldGenerator: Terrain tiles are not assigned!"
            );
            return;
        }

        SetupSeed();

        groundTilemap.ClearAllTiles();

        terrainBottom =
            minimumGroundHeight - groundDepth - 2;

        terrainTop =
            maximumGroundHeight + 2;

        terrainHeight =
            terrainTop - terrainBottom + 1;

        solidTerrain =
            new bool[worldWidth, terrainHeight];

        GenerateTerrainShape();
        PaintTerrain();

        groundTilemap.CompressBounds();

        Debug.Log(
            "World generated with seed: " + seed
        );
    }

    private void SetupSeed()
    {
        if (useRandomSeed)
        {
            seed = Random.Range(
                int.MinValue,
                int.MaxValue
            );
        }

        Random.InitState(seed);
    }

    private void GenerateTerrainShape()
    {
        int currentX = 0;
        int currentHeight = startingGroundHeight;

        while (currentX < worldWidth)
        {
            bool makeCombatArea =
                Random.value < combatAreaChance;

            if (makeCombatArea)
            {
                GenerateFlatArea(
                    ref currentX,
                    currentHeight
                );
            }
            else
            {
                GenerateTerrainFeature(
                    ref currentX,
                    ref currentHeight
                );
            }
        }
    }

    private void GenerateFlatArea(
        ref int currentX,
        int currentHeight
    )
    {
        int width = Random.Range(
            minCombatAreaWidth,
            maxCombatAreaWidth + 1
        );

        for (
            int i = 0;
            i < width && currentX < worldWidth;
            i++
        )
        {
            CreateSolidColumn(
                currentX,
                currentHeight
            );

            currentX++;
        }
    }

    private void GenerateTerrainFeature(
        ref int currentX,
        ref int currentHeight
    )
    {
        int featureWidth = Random.Range(
            minFeatureWidth,
            maxFeatureWidth + 1
        );

        int targetHeight =
            ChooseTargetHeight(currentHeight);

        int startingHeight = currentHeight;

        for (
            int i = 0;
            i < featureWidth && currentX < worldWidth;
            i++
        )
        {
            float progress =
                featureWidth <= 1
                    ? 1f
                    : (float)i / (featureWidth - 1);

            float smoothedProgress =
                Mathf.SmoothStep(
                    0f,
                    1f,
                    progress
                );

            float interpolatedHeight =
                Mathf.Lerp(
                    startingHeight,
                    targetHeight,
                    smoothedProgress
                );

            int surfaceHeight =
                Mathf.RoundToInt(
                    interpolatedHeight
                );

            CreateSolidColumn(
                currentX,
                surfaceHeight
            );

            currentX++;
        }

        currentHeight = targetHeight;
    }

    private int ChooseTargetHeight(
        int currentHeight
    )
    {
        int direction =
            Random.value < downwardChance
                ? -1
                : 1;

        int amount = Random.Range(
            1,
            maxFeatureHeightChange + 1
        );

        return Mathf.Clamp(
            currentHeight + direction * amount,
            minimumGroundHeight,
            maximumGroundHeight
        );
    }

    private void CreateSolidColumn(
        int x,
        int surfaceHeight
    )
    {
        int bottom =
            surfaceHeight - groundDepth;

        for (
            int y = bottom;
            y <= surfaceHeight;
            y++
        )
        {
            SetSolid(x, y);
        }
    }

    private void SetSolid(int x, int y)
    {
        if (x < 0 || x >= worldWidth)
            return;

        int arrayY = y - terrainBottom;

        if (
            arrayY < 0 ||
            arrayY >= terrainHeight
        )
        {
            return;
        }

        solidTerrain[x, arrayY] = true;
    }

    private bool IsSolid(int x, int y)
    {
        if (
            x < 0 ||
            x >= worldWidth
        )
        {
            return false;
        }

        int arrayY = y - terrainBottom;

        if (
            arrayY < 0 ||
            arrayY >= terrainHeight
        )
        {
            return false;
        }

        return solidTerrain[x, arrayY];
    }

    private void PaintTerrain()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (
                int y = terrainBottom;
                y <= terrainTop;
                y++
            )
            {
                if (!IsSolid(x, y))
                    continue;

                TileBase tile =
                    ChooseTile(x, y);

                groundTilemap.SetTile(
                    new Vector3Int(x, y, 0),
                    tile
                );
            }
        }
    }

    private TileBase ChooseTile(int x, int y)
    {
        bool up = IsSolid(x, y + 1);
        bool down = IsSolid(x, y - 1);
        bool left = IsSolid(x - 1, y);
        bool right = IsSolid(x + 1, y);

        bool upLeft =
            IsSolid(x - 1, y + 1);

        bool upRight =
            IsSolid(x + 1, y + 1);

        bool downLeft =
            IsSolid(x - 1, y - 1);

        bool downRight =
            IsSolid(x + 1, y - 1);

        // ----- OUTER CORNERS -----

        if (!up && !left)
            return topLeftTile != null
                ? topLeftTile
                : topTile;

        if (!up && !right)
            return topRightTile != null
                ? topRightTile
                : topTile;

        if (!down && !left)
            return bottomLeftTile != null
                ? bottomLeftTile
                : centerTile;

        if (!down && !right)
            return bottomRightTile != null
                ? bottomRightTile
                : centerTile;

        // ----- EDGES -----

        if (!up)
            return topTile;

        if (!down)
            return bottomTile != null
                ? bottomTile
                : centerTile;

        if (!left)
            return leftTile != null
                ? leftTile
                : centerTile;

        if (!right)
            return rightTile != null
                ? rightTile
                : centerTile;

        // ----- INNER CORNERS -----
        //
        // All four direct neighbors are solid,
        // but a diagonal neighbor is empty.

        if (!upLeft)
            return innerTopLeftTile != null
                ? innerTopLeftTile
                : centerTile;

        if (!upRight)
            return innerTopRightTile != null
                ? innerTopRightTile
                : centerTile;

        if (!downLeft)
            return innerBottomLeftTile != null
                ? innerBottomLeftTile
                : centerTile;

        if (!downRight)
            return innerBottomRightTile != null
                ? innerBottomRightTile
                : centerTile;

        // Completely surrounded terrain.
        return centerTile;
    }
}