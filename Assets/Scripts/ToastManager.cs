using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;

    [Header("Regular toast setup: ")]
    public GameObject toastPanel;
    public TMP_Text toastText;
    public float displayTime = 2f;
    public Color toastColor = Color.white;
    public float fadeDuration = 0.5f;

    [Header("Important toast setup: ")]
    public GameObject importantToastPanel;
    public TMP_Text importantToastText;
    public float importantToasDisplayTime = 5f;
    public Color importantToastColor = Color.red;
    public float importantFadeDuration = 1f;

    private void Awake()
    {
        Instance = this;
        toastPanel.SetActive(false);
        importantToastPanel.SetActive(false);
    }

    // Call this method when you want to output text
    // ToastManager.Instance.ShowToast("Text!");
    public void ShowToast(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        StopAllCoroutines();
        StartCoroutine(ShowToastCoroutine(toastPanel, toastText, message, displayTime, toastColor, fadeDuration));
    }

    // Call this method when you want to output important text
    // ToastManager.Instance.ShowImportantToast("Important Text!");
    public void ShowImportantToast(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        StopAllCoroutines();
        StartCoroutine(ShowToastCoroutine(importantToastPanel, importantToastText, message, importantToasDisplayTime, importantToastColor, importantFadeDuration));
    }

    private IEnumerator ShowToastCoroutine(GameObject panel, TMP_Text text, string message, float duration, Color textColor ,float fadeDuration)
    {
        text.text = message;
        text.color = textColor;
        panel.SetActive(true);

        // Fade in
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if(canvasGroup == null ) canvasGroup = panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        float elapsedTime = 0f;

        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(duration);

        // Fade out
        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null;
        }
        panel.SetActive(false);
    }
}
