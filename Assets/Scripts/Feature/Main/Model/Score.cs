using UnityEngine;

public class Score
{
    public int Data { get; private set; }


    public void AddData(int amount)
    {
        Data += amount;
    }
}