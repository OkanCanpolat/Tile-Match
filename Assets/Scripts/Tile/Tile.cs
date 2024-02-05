using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /*[HideInInspector] */public TileType Type;
    [HideInInspector] public bool CanMove;
    [HideInInspector] public int BlockCount;
    [SerializeField] private List<Tile> blockedTiles;
    private SpriteRenderer typeSpriteRenderer;
    private SpriteRenderer cardSpriteRenderer;
    private Collider2D tileCollider;
    private bool isBonusTile;
    public bool IsBonusTile { get => isBonusTile; set => isBonusTile = value; }

    private void Awake()
    {
        CanMove = true;
        tileCollider = GetComponent<Collider2D>();
        cardSpriteRenderer = GetComponent<SpriteRenderer>();
        typeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
  
    private void Start()
    {
        IncreaseBlocks();
    }
    private void OnMouseDown()
    {
        if (!CanMove || GameManager.Instance.GameState == GameState.Stop) return;
        TileAddCommand command = new TileAddCommand(this);
        TileCommandInvoker.Instance.AddCommand(command);
    }
    public void ChangeColliderState()
    {
        tileCollider.enabled = !tileCollider.enabled;
    }
    public void IncreaseBlockCount() 
    {
        BlockCount++;
    }
    public void DecreseBlockCount()
    {
        BlockCount--;
    }
    public void ControlBlockState()
    {
        if (BlockCount <= 0)
        {
            tileCollider.enabled = true;
            cardSpriteRenderer.color = Color.white;
            typeSpriteRenderer.color = Color.white;
        }
        else
        {
            tileCollider.enabled = false;
            cardSpriteRenderer.color = Color.grey;
            typeSpriteRenderer.color = Color.grey;
        }
    }
    public void IncreaseBlocks()
    {
        if (IsBonusTile) return;

        foreach(Tile tile in blockedTiles)
        {
            tile.IncreaseBlockCount();
            tile.ControlBlockState();
        }
    }
    public void DecreaseBlocks()
    {
        if (IsBonusTile) return;

        foreach (Tile tile in blockedTiles)
        {
            tile.DecreseBlockCount();
            tile.ControlBlockState();
        }
    }
    public void ChangeSprite(Sprite sprite)
    {
        typeSpriteRenderer.sprite = sprite;
    }
    public void ChangeTypeRendererState()
    {
        typeSpriteRenderer.enabled = !typeSpriteRenderer.enabled;
    }
    public Sprite GetTypeSprite()
    {
        return typeSpriteRenderer.sprite;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnTileDestroyed();
    }
}
