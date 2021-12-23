using UnityEngine;

public class PopUpCombo : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _popUpComboEffects;

    private void Awake()
    {
        EventManager.AddListener<int, int>(ScoreEventType.ShowPopUpScoreEffect, OnShowPopUpScoreEffect);
    }

    private void OnShowPopUpScoreEffect(int clearRows, int combosCount)
    {
        if (combosCount >= 2)
        {
            GameObject popUpComboEffect = _popUpComboEffects[combosCount - 2];
            popUpComboEffect.SetActive(true);
            popUpComboEffect.transform.localScale = Vector3.zero;
            LeanTween.scale(popUpComboEffect.transform.gameObject, Vector3.one, UIDefinition.TimeShowingPopUpComboEffect)
                     .setEaseOutQuint()
                     .setOnComplete(() =>
                     {
                         popUpComboEffect.transform.localScale = Vector3.zero;
                         popUpComboEffect.SetActive(false);
                     });

            SoundManager.Instance.PlaySound(SoundManager.Instance.blocksCombosSpeakingSFXClips, combosCount - 2);
        }
    }
}