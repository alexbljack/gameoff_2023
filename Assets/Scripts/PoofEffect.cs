using System.Collections;
using UnityEngine;

public class PoofEffect : MonoBehaviour
{
    public float delay = 5f;
    
    void Start()
    {
        StartCoroutine(PoofRoutine());
    }

    IEnumerator PoofRoutine() 
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
