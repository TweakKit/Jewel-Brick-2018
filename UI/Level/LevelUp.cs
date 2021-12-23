using UnityEngine;

using TMPro;

public class LevelUp : MonoBehaviour
{
    [SerializeField]
    private float _levelUpEffectDuration;
    private TextMeshProUGUI _levelText;

    private void Awake()
    {
        _levelText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _levelText.text = PlayerPrefsManager.Instance.savedData.Level.ToString();
    }

    public void OnLevelUpUpdate()
    {
        PlayerPrefsManager.Instance.savedData.Level += 1;
        _levelText.text = PlayerPrefsManager.Instance.savedData.Level.ToString();

        SoundManager.Instance.PlaySound(SoundManager.Instance.levelUpSFXClip);

        LeanTween.scale(_levelText.gameObject, Vector3.one * 1.4f, _levelUpEffectDuration)
                 .setEaseOutBack()
                 .setOnComplete(() => _levelText.transform.localScale = Vector3.one);
    }
}