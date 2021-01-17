using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statValue;
    [SerializeField] private float lerpSpeed;

    private Image content;

    private float currentFill;
    public float MyMaxValue { get; set; }

    private bool IsPercent; // jelenlegi érték/érték vagy % érték váltás

    private float currentValue;
    public float MyCurrentValue {
        get => currentValue;
        set {
            if (value > MyMaxValue) currentValue = MyMaxValue;
            else if (value < 0) currentValue = 0;
            else currentValue = value;

            currentFill = currentValue / MyMaxValue;

            if (statValue != null) if (!IsPercent) statValue.text = currentValue + " / " + MyMaxValue;
            
            if (IsPercent)
            {
                float tempValue = ((currentValue / MyMaxValue) * 100);

                if (tempValue != tempValue || tempValue <= 0) statValue.text = 0 + "%";
                else statValue.text = ((currentValue / MyMaxValue) * 100) + "%";
            }
        }
    }

    private void Awake()
    {
        content = GetComponent<Image>();
    }

    private void Update()
    {
        if (currentFill != content.fillAmount) content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
    }

    public void Initialize(float currentValue, float maxValue)
    {
		if (content == null) content = GetComponent<Image>();
		
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
		content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void ClickToStat()
    {
        if (IsPercent) IsPercent = false;
        else if (!IsPercent) IsPercent = true; 
        
        if (!IsPercent) statValue.text = currentValue + " / " + MyMaxValue;
        if (IsPercent)
        {
            float tempValue = ((currentValue / MyMaxValue) * 100);

            if (tempValue != tempValue || tempValue <= 0) statValue.text = 0 + "%";
            else statValue.text = ((currentValue / MyMaxValue) * 100) + "%";
        }
    }
}
