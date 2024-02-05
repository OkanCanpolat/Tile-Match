
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Stop, Continue
}
public class GameManager : Singleton<GameManager>
{
    public GameState GameState;
    public event Action OnWin;
    public event Action OnLose;
    [HideInInspector] public int levelTileCount;
    [SerializeField] private LevelProperties levelProperty;
    private void Start()
    {
        GameState = GameState.Continue;
    }
    public void OnTileDestroyed()
    {
        levelTileCount--;
        ControlLevelState();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(levelProperty.currentLevelIndex);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(levelProperty.nextLevelIndex);
    }
    public void LoseGame()
    {
        GameState = GameState.Stop;
        OnLose?.Invoke();
    }
    private void ControlLevelState()
    {
        if(levelTileCount <= 0)
        {
            OnWin?.Invoke();
            GameState = GameState.Stop;
        }
    }
}
