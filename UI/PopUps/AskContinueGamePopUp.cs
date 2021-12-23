using System.Collections;

using UnityEngine;

using TMPro;

public class AskContinueGamePopUp : PopUp
{
    private static readonly float openGameOverPopUpDelayTime = 0.5f;

    [SerializeField]
    private int _timeCount;
    [SerializeField]
    private TextMeshProUGUI _countTimeText;
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void UpdateData(long score)
    {
        _scoreText.text = score.ToString();
        StartCoroutine(CountTime());
    }

    public void AcceptToLose()
    {
        EventManager.Invoke(GameEventType.GameFail);
        StopAllCoroutines();
        Close();
    }

    public void WatchAds()
    {
        AdmobController.Instance.ShowRewardVideoAd();
        StopAllCoroutines();
        Close();
    }

    private IEnumerator CountTime()
    {
        float timeValue = 0;
        int timeCount = _timeCount;
        _countTimeText.text = timeCount.ToString();

        while (true)
        {
            timeValue += Time.deltaTime;
            if (timeValue >= 1)
            {
                timeCount--;
                _countTimeText.text = timeCount.ToString();
                if (timeCount == 0)
                {
                    yield return new WaitForSeconds(openGameOverPopUpDelayTime);
                    AcceptToLose();
                }

                timeValue = 0;
            }

            yield return null;
        }
    }
}