using System.Collections.Generic;

public class UserScoreManager
{
    public UserScore localUserScore;
    public List<UserScore> internetUserScores;

    public UserScoreManager(UserScore localUserScore, List<UserScore> internetUserScores)
    {
        this.localUserScore = localUserScore;
        this.internetUserScores = internetUserScores;
    }

    public UserScoreManager()
    {
        this.localUserScore = new UserScore();
        this.internetUserScores = new List<UserScore>();
    }

    public void SetLocalUserScore(UserScore localUserScore) => this.localUserScore = localUserScore;
    public void AddInternetUserScore(UserScore internetUserScore) => this.internetUserScores.Add(internetUserScore);

    public string GetData()
    {
        string data = "";

        data += "\n Local User: - Name: " + this.localUserScore.name + " - ID: " + this.localUserScore.id + " - Score: " + this.localUserScore.score + " - Rank: " + this.localUserScore.rank;
        data += "\n";

        foreach (UserScore userScore in internetUserScores)
        {
            data += "Internet User: - Name: " + userScore.name + " - ID: " + userScore.id + " - Score: " + userScore.score + " - Rank: " + userScore.rank;
            data += "\n";
        }

        return data;
    }
}