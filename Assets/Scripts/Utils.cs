
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static T PickRandomFromList<T> (List<T> array)
    {
        return array[Random.Range(0, array.Count)];
    }
}