using TMPro;
using UnityEngine;

public class EndGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePopup;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text endGameDescriptionText;
    [SerializeField] private LevelProperties levelProperties;
    private const string loseDescription = "FAIL!";
    private const string winDescription = "COMPLETE!";

    private void Start()
    {
        GameManager.Instance.OnWin += OnWin;
        GameManager.Instance.OnLose += OnLose;
        levelText.text = (levelProperties.currentLevelIndex + 1).ToString();
    }
    private void OnWin()
    {
        endGamePopup.SetActive(true);
        nextLevelButton.SetActive(true);
        endGameDescriptionText.text = winDescription;
    }
    private void OnLose()
    {
        endGamePopup.SetActive(true);
        endGameDescriptionText.text = loseDescription;
    }
}
