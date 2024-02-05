using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private List<TileTypeSpriteDTO> tileTypeChoices;
    [SerializeField] private int[] choiceDistributions;
    private List<Tile> levelTiles;
    public int LevelTileCount { get => levelTiles.Count; }
    private void Start()
    {
        levelTiles = FindObjectsOfType<Tile>().ToList();
        GameManager.Instance.levelTileCount = levelTiles.Count;
        SetupLevelRandomly();
    }
    private void SetupLevelRandomly()
    {
        List<TileTypeSpriteDTO> choices = GetTileChoicesCopy();
        List<Tile> tiles = GetLevelTilesCopy();

        for (int i = 0; i < choiceDistributions.Length; i++)
        {
            int currentTileCount = choiceDistributions[i];
            TileTypeSpriteDTO currentChoice = choices[Random.Range(0, choices.Count)];
            choices.Remove(currentChoice);

            for(int j = 0; j < currentTileCount; j++)
            {
                Tile currentTile = tiles[Random.Range(0, tiles.Count)];
                tiles.Remove(currentTile);
                currentTile.Type = currentChoice.Type;
                currentTile.ChangeSprite(currentChoice.Sprite);
            }
        }
    }
    private List<TileTypeSpriteDTO> GetTileChoicesCopy()
    {
        List<TileTypeSpriteDTO> choices = new List<TileTypeSpriteDTO>();
        foreach(TileTypeSpriteDTO choice in tileTypeChoices)
        {
            choices.Add(choice);
        }
        return choices;
    }
    private List<Tile> GetLevelTilesCopy()
    {
        List<Tile> tiles = new List<Tile>();
        foreach (Tile tile in levelTiles)
        {
            tiles.Add(tile);
        }
        return tiles;
    }
}
