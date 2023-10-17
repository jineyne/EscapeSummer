using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : UIBase {
    [SerializeField]
    private Text textDescription;

    [SerializeField]
    private float textSwitchingDelay = 5;

    [SerializeField]
    [TextArea]
    private List<string> gameOverTexts;

    [SerializeField]
    [TextArea]
    private List<string> gameClearTexts;

    [SerializeField]
    private GameObject endingText;

    [SerializeField]
    private GameObject endingCredit;

    private void Start() {
        UIManager.Instance.ActiveUI = this;

        var type = UIManager.Instance.EndingType;

        ShowEndingText(type, 0);
    }

    public void Title() {
        StartCoroutine(WaitScene("TitleMenu"));
    }

    void ShowEndingText(EndingType type, int index) {
        var list = GetText(type);

        textDescription.text = list[index];
        if (list.Count <= index + 1) {
            StartCoroutine(WaitAction(delegate {
                if (type == EndingType.ItWasSummer) {
                    endingText.SetActive(false);
                    endingCredit.SetActive(true);
                } else {
                    StartCoroutine(WaitScene("TitleMenu"));
                }
            }, FadeType.None, textSwitchingDelay));
        } else {
            StartCoroutine(WaitAction(delegate {
                StartCoroutine(Dessolve(delegate {
                    ShowEndingText(type, index + 1);
                }));
            }, FadeType.None, textSwitchingDelay));
        }
    }

    IEnumerator Dessolve(Action action, float delay = 1.0f) {
        float timer = 0f;
        float limit = delay;
        while (timer < limit) {
            timer += Time.unscaledDeltaTime;
            if (canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp(timer, 0f, limit) / limit);
            }
            yield return new WaitForEndOfFrame();
        }

        if (action != null) {
            action();
        }

        timer = 0f;
        while (timer < limit) {
            timer += Time.unscaledDeltaTime;
            if (canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp(timer, 0f, limit));
            }
            yield return new WaitForEndOfFrame();
        }
    }

    List<string> GetText(EndingType type) {
        switch (type) {
            case EndingType.ItIsSummer:
                return gameOverTexts;

            case EndingType.ItWasSummer:
                return gameClearTexts;
        }

        return null;
    }
}
