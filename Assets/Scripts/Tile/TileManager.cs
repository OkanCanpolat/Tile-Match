using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    [SerializeField] private Transform tileStandartAdditionParent;
    [SerializeField] private float tileWidth;
    [SerializeField] private float spacing;
    [SerializeField] private float padding;
    [SerializeField] private float movementSpeed;
    [SerializeField] private BonusFeatureManager bonusFeatureManager;
    [SerializeField] private List<Tile> currentTiles;
    private List<Tile> activeTiles;
    private int currentAddedTileCount = 0;
    private const int amountToMatch = 3;
    private const int maxTileCount = 8;
    public List<Tile> ActiveTiles { get => activeTiles; }

    private void Start()
    {
        activeTiles = FindObjectsOfType<Tile>().ToList();
    }
    public void AddTile(Tile tile)
    {
        currentTiles[currentAddedTileCount] = tile;
        currentAddedTileCount++;
    }
    public void RemoveTile(Tile tile)
    {
        currentTiles[currentTiles.IndexOf(tile)] = null;
        currentAddedTileCount--;
    }
    public void ChangeTilePosition(Tile tile, Vector2 destination)
    {
        StartCoroutine(MoveTile(tile, destination));
    }
    public void ControlAdditionResult(Tile tile)
    {
        StartCoroutine(ControlAddResultCo(tile));
    }
    public void RemoveActiveTile(Tile tile)
    {
        activeTiles.Remove(tile);
    }
    public void AddActiveTile(Tile tile)
    {
        activeTiles.Add(tile);
    }
    public void ApplyTrinityBonus(int trinityBonusCount, Transform parent)
    {
        for (int i = 0; i < trinityBonusCount; i++)
        {
            if (currentTiles[i] != null)
            {
                Tile tile = currentTiles[i];
                Vector3 destination = GetPosition(i, parent);
                RemoveTile(tile);
                ChangeTilePosition(tile, destination);
                tile.CanMove = true;
                tile.IsBonusTile = true;
                TileCommandInvoker.Instance.RemoveCommand(tile);
            }
        }

        RepositionTiles();
    }
    private bool IsThereMatch(TileType type)
    {
        int counter = 0;

        foreach (Tile tile in currentTiles)
        {
            if (tile != null && type == tile.Type)
            {
                counter++;
            }
        }

        if (counter >= amountToMatch) return true;
        return false;
    }
    private IEnumerator MoveTile(Tile tile, Vector2 destination)
    {
        float elapsedTime = 0;
        Vector2 currentPosition = tile.transform.position;
        GameManager.Instance.GameState = GameState.Stop;

        while (elapsedTime < 1)
        {
            tile.transform.position = Vector2.Lerp(currentPosition, destination, elapsedTime);
            elapsedTime += Time.deltaTime * movementSpeed;
            yield return null;
        }

        GameManager.Instance.GameState = GameState.Continue;
        tile.transform.position = destination;
    }
    private Vector2 GetPosition(int referenceIndex, Transform referenceTransform)
    {
        float x = referenceTransform.position.x + referenceIndex * (tileWidth + spacing) + tileWidth / 2 + padding;
        Vector2 destination = new Vector2(x, referenceTransform.position.y);
        return destination;
    }
    private IEnumerator ControlAddResultCo(Tile tile)
    {
        Vector2 destination = GetPosition(currentAddedTileCount - 1, tileStandartAdditionParent);

        if (currentAddedTileCount >= maxTileCount && !IsThereMatch(tile.Type))
        {
            GameManager.Instance.LoseGame();
            StartCoroutine(MoveTile(tile, destination));
        }
        else if (IsThereMatch(tile.Type))
        {
            List<Tile> matchedTiles = HandleMatches(tile);
            yield return StartCoroutine(MoveTile(tile, destination));
            DestroyMatchedTiles(matchedTiles);
            RepositionTiles();
        }
        else
        {
            StartCoroutine(MoveTile(tile, destination));
        }
    }
    private List<Tile> HandleMatches(Tile tile)
    {
        List<Tile> matchedTiles = new List<Tile>();
        TileType type = tile.Type;

        for (int i = 0; i < currentTiles.Count; i++)
        {
            if (currentTiles[i] != null && currentTiles[i].Type == type)
            {
                TileCommandInvoker.Instance.RemoveCommand(currentTiles[i]);
                matchedTiles.Add(currentTiles[i]);
                RemoveTile(currentTiles[i]);
            }
        }

        return matchedTiles;
    }
    private void DestroyMatchedTiles(List<Tile> matchedTiles)
    {
        for (int i = matchedTiles.Count - 1; i >= 0; i--)
        {
            Destroy(matchedTiles[i].gameObject);
            matchedTiles.RemoveAt(i);
        }
    }
    private void RepositionTiles()
    {
        for (int i = 0; i < currentTiles.Count - 1; i++)
        {
            if (currentTiles[i] == null)
            {
                for (int j = i + 1; j < currentTiles.Count; j++)
                {
                    if (currentTiles[j] != null)
                    {
                        Vector2 newPosition = GetPosition(i, tileStandartAdditionParent);
                        StartCoroutine(MoveTile(currentTiles[j], newPosition));
                        currentTiles[i] = currentTiles[j];
                        currentTiles[j] = null;
                        break;
                    }
                }
            }
        }
    }
}
