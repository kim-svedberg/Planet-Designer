using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Seed
{
    public int value;

    public int New()
    {
        return value = Random.Range(int.MinValue, int.MaxValue);
    }
}
