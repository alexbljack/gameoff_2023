using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInfo : MonoBehaviour
{
    [SerializeField] ResourceType resource;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] GameManager game;
    [SerializeField] int valueStep = 1;
    [SerializeField] GameObject changeTextPrefab;
    [SerializeField] Vector3 changeTextPosition;

    float _currentValue;
    int _targetValue;

    void Awake()
    {
        _targetValue = 0;
        _currentValue = _targetValue;
    }

    void Start()
    {
        icon.sprite = resource.icon;
        // _targetValue = game.Resources[resource].Amount;
        SetAmount(game.Resources[resource].Amount);
    }
    //
    // void Update()
    // {
    //     if (Mathf.Abs(_currentValue - _targetValue) > Time.deltaTime)
    //     {
    //         int sign = _currentValue < _targetValue ? 1 : -1;
    //         _currentValue += sign * valueStep * Time.deltaTime;
    //         _currentValue = Mathf.Clamp(_currentValue, 0, _targetValue);
    //         SetAmount((int) _currentValue);
    //     }
    //     else
    //     {
    //         _currentValue = _targetValue;
    //     }
    // }

    void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    void OnResourceUpdated(Resource res, int change)
    {
        if (res.Type == resource)
        {
            var changeTxt = Instantiate(changeTextPrefab);
            changeTxt.transform.SetParent(transform);
            RectTransform rect = changeTxt.GetComponent<RectTransform>();
            rect.localScale = new Vector3(1, 1, 1);
            rect.localPosition = changeTextPosition;
            changeTxt.GetComponent<ResourceChangeText>().Init(change);
            SetAmount(res.Amount);
        }
    }

    void OnEnable()
    {
        Resource.AmountChanged += OnResourceUpdated;
    }

    void OnDisable()
    {
        Resource.AmountChanged -= OnResourceUpdated;
    }
}
