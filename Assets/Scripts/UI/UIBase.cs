using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum FadeType {
    None,
    FadeIn,
    FadeOut,
}

public class UIBase : MonoBehaviour {
    [SerializeField]
    protected CanvasGroup canvasGroup;

    private AsyncOperation sceneLoad = null;

    protected IEnumerator FadeIn(float delay = 1f) {
        float timer = 0f;
        float limit = delay;
        while (timer < limit) {
            timer += Time.unscaledDeltaTime;
            if (canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp(timer, 0f, limit));
            }
            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator FadeOut(float delay = 1f) {
        float timer = 0f;
        float limit = delay;
        while (timer < limit) {
            timer += Time.unscaledDeltaTime;
            if (canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp(timer, 0f, limit) / limit);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator WaitAction(Action action, FadeType fade = FadeType.FadeOut, float delay = 1f) {
        float timer = 0f;
        float limit = delay;
        while (timer < limit) {
            timer += Time.unscaledDeltaTime;
            if (fade == FadeType.FadeOut && canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp(timer, 0f, limit) / limit);
            } else if (fade == FadeType.FadeIn && canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp(timer, 0f, limit));
            }
            yield return new WaitForEndOfFrame();
        }

        if (action != null) {
            action();
        }
    }

    protected IEnumerator WaitScene(string sceneName, FadeType fade = FadeType.FadeOut, float delay = 1f) {
        float timer = 0f;
        float limit = delay;

        sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        sceneLoad.allowSceneActivation = false;

        while (timer < limit) {
            timer += Time.unscaledDeltaTime;
            if (fade == FadeType.FadeOut && canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp(timer, 0f, limit) / limit);
            } else if (fade == FadeType.FadeIn && canvasGroup != null) {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp(timer, 0f, limit));
            }
            yield return new WaitForEndOfFrame();
        }

        while (sceneLoad.progress < 0.9f) {
            yield return new WaitForEndOfFrame();
        }

        sceneLoad.allowSceneActivation = true;
    }
}
