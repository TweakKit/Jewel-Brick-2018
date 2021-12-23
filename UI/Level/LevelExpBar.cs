using UnityEngine;
using UnityEngine.UI;

public class LevelExpBar : MonoBehaviour
{
    private Image _progressBarImage;

    private void Awake()
    {
        _progressBarImage = gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        _progressBarImage.fillAmount = 0;
    }

    public void OnNormalUpdate(int currentLevelExp, int addedExp, int currentNeededLevelExp)
    {
        StartCoroutine(Lerp.TransformSlider
        (
            _progressBarImage,
            (float)(currentLevelExp - addedExp) / currentNeededLevelExp,
            (float)currentLevelExp / currentNeededLevelExp,
             UIDefinition.ImageFillingSpeed)
        );
    }

    public void OnLevelUpUpdate(int currentLevelExp, int currentNeededLevelExp)
    {
        StartCoroutine(Lerp.TransformSlider
        (
            _progressBarImage,
            0.0f,
            (float)currentLevelExp / currentNeededLevelExp,
            UIDefinition.ImageFillingSpeed)
        );
    }
}