using TMPro;
using UnityEngine;

public class BonusFeatereUIManager : MonoBehaviour
{
    [SerializeField] private BonusFeatureManager bonusFeatureManager;
    [SerializeField] private TMP_Text undoCountText;
    [SerializeField] private TMP_Text shuffleCountText;
    [SerializeField] private TMP_Text trinityCountText;

    private void Awake()
    {
        bonusFeatureManager.OnShuffleCountChanged += OnShuffleCountChanged;
        bonusFeatureManager.OnTrinityCountChanged += OnTrinityCountChanged;
        bonusFeatureManager.OnUndoCountChanged += OnUndoCountChanged;
    }

    private void OnUndoCountChanged(int value)
    {
        undoCountText.text = value.ToString();
    }
    private void OnShuffleCountChanged(int value)
    {
        shuffleCountText.text = value.ToString();
    }
    private void OnTrinityCountChanged(int value)
    {
        trinityCountText.text = value.ToString();
    }
}
