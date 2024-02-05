using UnityEngine;

[CreateAssetMenu(fileName = "Level Properties")] 
public class LevelProperties : ScriptableObject
{
    public int currentLevelIndex;
    public int nextLevelIndex;
}
