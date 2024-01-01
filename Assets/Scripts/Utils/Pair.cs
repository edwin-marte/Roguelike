using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pair<U, V>
{
    public U first;
    public V second;

    public Pair(U first, V second)
    {
        this.first = first;
        this.second = second;
    }
}
