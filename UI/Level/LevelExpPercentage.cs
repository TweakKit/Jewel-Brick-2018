using UnityEngine;

using TMPro;

public class LevelExpPercentage : MonoBehaviour
{
    private TextMeshProUGUI _progressPercentText;

    private void Awake()
    {
        _progressPercentText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _progressPercentText.text = "0%";
    }

    public void OnNormalUpdate(int currentLevelExp, int addedExp, int currentNeededLevelExp)
    {
        StartCoroutine(Lerp.TransformText
        (
            _progressPercentText,
            Mathf.Clamp((int)(((float)(currentLevelExp - addedExp) / currentNeededLevelExp) * 100), 0, 100),
            Mathf.Clamp((int)(((float)currentLevelExp / currentNeededLevelExp) * 100), 0, 100),
            "%",
            UIDefinition.TextTransformingSpeed)
        );
    }

    public void OnLevelUpUpdate(int currentLevelExp, int currentNeededLevelExp)
    {
        StartCoroutine(Lerp.TransformText
        (
            _progressPercentText,
            0,
            (int)(((float)currentLevelExp / currentNeededLevelExp) * 100),
            "%",
            UIDefinition.TextTransformingSpeed)
        );
    }
}