using UnityEngine;

using TMPro;

public class PopUpScore : MonoBehaviour
{
    private static readonly Vector3 popUpScoreTextLocalScale = new Vector3(0.2f, 0.2f, 1);

    [SerializeField]
    private Color[] _popUpScoreTextColors;
    [SerializeField]
    private float _popUpScoreTextMoveDistance = 50.0f;

    private TextMeshProUGUI _popUpScoreText;

    private void Awake()
    {
        _popUpScoreText = gameObject.GetComponent<TextMeshProUGUI>();
        EventManager.AddListener<long, int>(ScoreEventType.ShowPopUpScoreText, OnShowPopUpScoreText);
    }

    private void Start()
    {
        _popUpScoreText.transform.localScale = Vector3.zero;
    }

    private void OnShowPopUpScoreText(long addedScore, int combosCount)
    {
        _popUpScoreText.text = addedScore.ToString();
        _popUpScoreText.color = _popUpScoreTextColors[combosCount - 1];
        _popUpScoreText.transform.localScale = popUpScoreTextLocalScale;

        LeanTween.cancel(_popUpScoreText.gameObject);
        LeanTween.scale(_popUpScoreText.gameObject,
                        Vector3.one,
                        UIDefinition.TimeShowingPopUpScoreText)
                 .setEaseOutBack()
                 .setOnComplete(() =>
                 {
                     _popUpScoreText.transform.localScale = Vector3.zero;
                 });
    }
}