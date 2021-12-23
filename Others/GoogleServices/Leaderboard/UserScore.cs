public struct UserScore
{
    public readonly string id;
    public readonly string name;
    public readonly long score;
    public readonly int rank;

    public UserScore(string id, string name, long score, int rank)
    {
        this.id = id;
        this.name = name;
        this.score = score;
        this.rank = rank;
    }
}