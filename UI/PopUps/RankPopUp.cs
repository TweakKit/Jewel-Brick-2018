using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class RankPopUp : PopUp
{
    public static bool HasLoadedLeaderboardOnce = false;

    private static readonly int limitedNameCharacters = 12;
    private static readonly int maximumScorePanels = 10;

    [SerializeField]
    private TMP_InputField _nameInputField;
    [SerializeField]
    private TextMeshProUGUI _userNameText;
    [SerializeField]
    private GameObject _loadingPanel;
    [SerializeField]
    private GameObject _localScorePanel;
    [SerializeField]
    private GameObject[] _internetScorePanels;
    [SerializeField]
    private Sprite[] _topSprites;

    public override void Init(BaseScene parentScene)
    {
        base.Init(parentScene);

        _nameInputField.characterLimit = limitedNameCharacters;
        LoadDefaultData();
    }

    public void OnLoadLeaderboardSucceeded(UserScoreManager userScoreManager)
    {
        UpdateLocalUserScoreData(userScoreManager.localUserScore);

        if (userScoreManager.internetUserScores.Count < maximumScorePanels)
        {
            for (int i = 0; i < userScoreManager.internetUserScores.Count; i++)
                UpdateInternetUserScoreData(_internetScorePanels[i], userScoreManager.internetUserScores[i], userScoreManager.localUserScore);
        }
        else
        {
            for (int i = 0; i < maximumScorePanels; i++)
                UpdateInternetUserScoreData(_internetScorePanels[i], userScoreManager.internetUserScores[i], userScoreManager.localUserScore);
        }

        if (!HasLoadedLeaderboardOnce)
            _loadingPanel.SetActive(false);
        HasLoadedLeaderboardOnce = true;
    }

    public void OnLoadLeaderboardFailed()
    {
        _loadingPanel.SetActive(false);
    }

    public void LoadDefaultData()
    {
        if (!HasLoadedLeaderboardOnce)
            for (int i = 0; i < _internetScorePanels.Length; i++)
                _internetScorePanels[i].SetActive(false);

        UserScore localUserScore = new UserScore("",
                                                PlayerPrefsManager.Instance.savedData.UserName,
                                                PlayerPrefsManager.Instance.savedData.BestScore,
                                                PlayerPrefsManager.Instance.savedData.Rank);
        UpdateLocalUserScoreData(localUserScore);
    }

    private void UpdateLocalUserScoreData(UserScore localUserScore)
    {
        if (localUserScore.rank >= 1 && localUserScore.rank <= 3)
        {
            _localScorePanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
            _localScorePanel.transform.GetChild(0).GetComponent<Image>().sprite = _topSprites[localUserScore.rank - 1];
        }
        else _localScorePanel.transform.GetChild(0).GetComponent<Image>().enabled = false;

        _localScorePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = localUserScore.rank.ToString();
        _localScorePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPrefsManager.Instance.savedData.UserName;
        _localScorePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = localUserScore.score.ToString();
        _localScorePanel.SetActive(true);

        PlayerPrefsManager.Instance.savedData.Rank = localUserScore.rank;
    }

    private void UpdateInternetUserScoreData(GameObject internetScorePanel, UserScore internetUserScore, UserScore localUserScore)
    {
        if (internetUserScore.rank >= 1 && internetUserScore.rank <= 3)
        {
            internetScorePanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
            internetScorePanel.transform.GetChild(0).GetComponent<Image>().sprite = _topSprites[internetUserScore.rank - 1];
        }
        else internetScorePanel.transform.GetChild(0).GetComponent<Image>().enabled = false;

        internetScorePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = internetUserScore.rank.ToString();
        internetScorePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = internetUserScore.name.ToString();
        internetScorePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = internetUserScore.score.ToString();
        internetScorePanel.SetActive(true);

        if (internetUserScore.id == localUserScore.id)
            internetScorePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlayerPrefsManager.Instance.savedData.UserName;
    }

    public void Rename()
    {
        if (_nameInputField.text.Length != 0)
        {
            _userNameText.text = _nameInputField.text;
            PlayerPrefsManager.Instance.savedData.UserName = _nameInputField.text.ToString();
        }
    }
}