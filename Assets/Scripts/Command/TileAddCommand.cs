using UnityEngine;

public class TileAddCommand : ITileCommand
{
    private Tile tile;
    private Vector2 startingPosition;
    public TileAddCommand(Tile tile)
    {
        this.tile = tile;
        startingPosition = tile.transform.position;
    }
    public Tile Tile { get => tile; set => tile = value; }
    public void Execute()
    {
        TileManager.Instance.AddTile(tile);
        TileManager.Instance.RemoveActiveTile(tile);
        TileManager.Instance.ControlAdditionResult(tile);
        tile.DecreaseBlocks();
        tile.CanMove = false;
    }
    public void Undo()
    {
        if (tile != null)
        {
            TileManager.Instance.RemoveTile(tile);
            TileManager.Instance.AddActiveTile(tile);
            TileManager.Instance.ChangeTilePosition(tile, startingPosition);
            tile.IncreaseBlocks();
            tile.CanMove = true;
        }
    }
}
