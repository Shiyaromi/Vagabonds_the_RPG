using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellBook : MonoBehaviour
{
    [SerializeField] private Image castingBar;

    [SerializeField] private TextMeshProUGUI spellName;

    [SerializeField] private TextMeshProUGUI castTime;

    [SerializeField] private Image icon;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Spell[] spells;

    private Coroutine spellRoutine;
    private Coroutine fadeRoutine;

    public Spell CastSpell(int index)
    {
        castingBar.fillAmount = 0;
        castingBar.color = spells[index].BarColor;
        spellName.text = spells[index].Name;
        icon.sprite = spells[index].Icon;

        spellRoutine = StartCoroutine(Progress(index));

        fadeRoutine = StartCoroutine(FadeBar());

        return spells[index];
    }

    private IEnumerator Progress(int index)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / spells[index].CastTime;
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (spells[index].CastTime - timePassed).ToString("F1");

            if (spells[index].CastTime - timePassed < 0) castTime.text = "0.0";

            yield return null;
        }

        StopCasting();
    }

    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.25f;
        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    public void StopCasting()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }
}
