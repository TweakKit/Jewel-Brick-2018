using UnityEngine;

public class LevelExpManager : MonoBehaviour
{
    private const int levelBaseExperience = 200;
    private const float levelMultiplyConstant = 1.5f;

    [SerializeField]
    private LevelUp _levelUp;
    [SerializeField]
    private LevelExpBar _levelExpBar;
    [SerializeField]
    private LevelExpPercentage _levelExpPercentage;

    private int _currentLevelExp;
    private int _currentNeededLevelExp;

    private void Awake()
    {
        EventManager.AddListener<int, int>(GameEventType.ClearRows, OnClearRows);
    }

    private void Start()
    {
        int savedLevel = PlayerPrefsManager.Instance.savedData.Level;
        _currentNeededLevelExp = (int)(levelBaseExperience * Mathf.Pow(levelMultiplyConstant, savedLevel - 1));
        _currentLevelExp = 0;
    }

    private void OnClearRows(int clearRows, int combosCount)
    {
        int addedExp = 0;
        switch (clearRows)
        {
            case 1:
                addedExp = 10 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
            case 2:
                addedExp = 25 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
            case 3:
                addedExp = 75 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
            case 4:
            default:
                addedExp = 300 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
        }

        addedExp *= combosCount;
        _currentLevelExp += addedExp;

        if (_currentLevelExp >= _currentNeededLevelExp)
        {
            _currentLevelExp -= _currentNeededLevelExp;
            _currentNeededLevelExp = (int)(_currentNeededLevelExp * levelMultiplyConstant);

            _levelUp.OnLevelUpUpdate();
            _levelExpBar.OnLevelUpUpdate(_currentLevelExp, _currentNeededLevelExp);
            _levelExpPercentage.OnLevelUpUpdate(_currentLevelExp, _currentNeededLevelExp);
        }
        else
        {
            _levelExpBar.OnNormalUpdate(_currentLevelExp, addedExp, _currentNeededLevelExp);
            _levelExpPercentage.OnNormalUpdate(_currentLevelExp, addedExp, _currentNeededLevelExp);
        }
    }
}