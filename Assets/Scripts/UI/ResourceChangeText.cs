using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceChangeText : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float moveSpeed;
    [SerializeField] Color plusColor;
    [SerializeField] Color minusColor;
    
    TextMeshProUGUI _text;
    RectTransform _rect;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _rect = GetComponent<RectTransform>();
    }

    public void Init(int changeValue)
    {
        string prefix = changeValue > 0 ? "+" : "-";
        _text.color = changeValue > 0 ? plusColor : minusColor;
        _text.text = $"{prefix}{Mathf.Abs(changeValue)}";
        StartCoroutine(TextFlowRoutine());
    }

    IEnumerator TextFlowRoutine()
    {
        float timePassed = 0;
        while (timePassed < lifeTime)
        {
            _rect.position += new Vector3(0, Time.deltaTime * moveSpeed, 0);
            timePassed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}