using UnityEngine;

public class Score
{
    public int Data { get; private set; }

    private Score()
    {
    }

    public static Score Initialize()
    {
        return new Score();
    }

    public void AddData(int amount)
    {
        Data += amount;
    }
}