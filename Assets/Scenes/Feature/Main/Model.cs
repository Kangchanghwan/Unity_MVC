using UnityEngine;

public class Model
{
    public int Data { get; private set; }

    public void AddData(int amount)
    {
        Data += amount;
    }
}