using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusFeatureManager : MonoBehaviour
{
    [Header("Shuffle Bonus Variables")]
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private float minRotationSpeed;
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationTime;
    private List<Tile> tileBox = new List<Tile>();
    private int shuffleBonusUsageCount = 1;
    public event Action<int> OnShuffleCountChanged;

    [Header("Trinity Bonus Variables")]
    [SerializeField] private Transform tileBonusAdditionParent;
    private const int trinityBonusCount = 3;
    private int trinityBonusUsageCount = 1;
    public event Action<int> OnTrinityCountChanged;

    [Header("Undo Bonus Variables")]
    private int undoBonusUsageCount = 3;
    public event Action<int> OnUndoCountChanged;

    private void Start()
    {
        OnShuffleCountChanged?.Invoke(shuffleBonusUsageCount);
        OnTrinityCountChanged?.Invoke(trinityBonusUsageCount);
        OnUndoCountChanged?.Invoke(undoBonusUsageCount);
    }

    public void UseShuffleBonus()
    {
        if (GameManager.Instance.GameState == GameState.Stop) return;

        List<Tile> tiles = TileManager.Instance.ActiveTiles;

        if (shuffleBonusUsageCount > 0)
        {
            shuffleBonusUsageCount--;
            OnShuffleCountChanged?.Invoke(shuffleBonusUsageCount);
            SetupTileBox(tiles);

            foreach (Tile tile in tiles)
            {
                StartCoroutine(StartShuffle(tile));
            }
        }

    }
    public void UseTrinity()
    {
        if (GameManager.Instance.GameState == GameState.Stop) return;

        if (trinityBonusUsageCount > 0)
        {
            TileManager.Instance.ApplyTrinityBonus(trinityBonusCount, tileBonusAdditionParent);
            trinityBonusUsageCount--;
            OnTrinityCountChanged?.Invoke(trinityBonusUsageCount);
        }
    }
    public void UseUndo()
    {
        if (GameManager.Instance.GameState == GameState.Stop) return;

        if (undoBonusUsageCount > 0)
        {
            TileCommandInvoker.Instance.UndoCommand();
            undoBonusUsageCount--;
            OnUndoCountChanged?.Invoke(undoBonusUsageCount);
        }
    }
    private IEnumerator StartShuffle(Tile tile)
    {
        Vector2 startingPosition = tile.transform.position;

        GameManager.Instance.GameState = GameState.Stop;
        yield return StartCoroutine(MoveTileMiddle(tile));
        yield return StartCoroutine(MoveRandomCircular(tile));
        ShuffleTileTypes(tile);
        yield return StartCoroutine(RepositionAfterShuffle(tile, startingPosition));
        GameManager.Instance.GameState = GameState.Continue;
    }
    private IEnumerator MoveTileMiddle(Tile tile)
    {
        float elapsedTime = 0;
        Transform tileTransform = tile.transform;
        Vector2 firstPosition = tileTransform.position;
        Vector2 destination = Vector2.zero;

        while (elapsedTime < 1)
        {
            tileTransform.position = Vector2.Lerp(firstPosition, destination, elapsedTime);
            elapsedTime += Time.deltaTime * movementSpeed;
            yield return null;
        }

        tileTransform.position = destination;
    }
    private IEnumerator MoveRandomCircular(Tile tile)
    {
        float angle = 0;
        float progress = 0;
        Vector2 startPos = tile.transform.position;
        Transform tileTransform = tile.transform;
        float rotationSpeed = UnityEngine.Random.Range(minRotationSpeed, maxRotationSpeed); ;
        float rotationRadius = UnityEngine.Random.Range(minRadius, maxRadius); ;

        tile.ChangeTypeRendererState();

        while (progress < rotationTime)
        {
            progress += Time.deltaTime;
            angle += rotationSpeed * Time.deltaTime;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * rotationRadius;
            tileTransform.position = startPos + offset;
            yield return null;
        }

        tile.ChangeTypeRendererState();
    }
    private IEnumerator RepositionAfterShuffle(Tile tile, Vector2 destination)
    {
        float elapsedTime = 0;
        Transform tileTransform = tile.transform;
        Vector2 firstPosition = tileTransform.position;

        while (elapsedTime < 1)
        {
            tileTransform.position = Vector2.Lerp(firstPosition, destination, elapsedTime);
            elapsedTime += Time.deltaTime * movementSpeed;
            yield return null;
        }

        tileTransform.position = destination;
    }
    private void ShuffleTileTypes(Tile sourceTile)
    {
        Tile targetTile = tileBox[UnityEngine.Random.Range(0, tileBox.Count)];

        Sprite tempSprite = sourceTile.GetTypeSprite();
        sourceTile.ChangeSprite(targetTile.GetTypeSprite());
        targetTile.ChangeSprite(tempSprite);

        TileType tempType = sourceTile.Type;
        sourceTile.Type = targetTile.Type;
        targetTile.Type = tempType;

        tileBox.Remove(targetTile);
    }
    private void SetupTileBox(List<Tile> tiles)
    {
        tileBox.Clear();

        foreach (Tile tile in tiles)
        {
            tileBox.Add(tile);
        }
    }
}
