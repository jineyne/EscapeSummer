using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuUI : UIBase {
    [SerializeField]
    private GameObject settings;

    [SerializeField]
    private GameObject howToPlays;

    [SerializeField]
    private GameObject howToPlaysNextButton;

    [SerializeField]
    private GameObject howToPlaysPrevButton;

    [SerializeField]
    private List<GameObject> howToPlaysPages;

    [SerializeField]
    private int howToPlaysPageIndex = 0;

    [SerializeField]
    private Dropdown resolutionDropdown;

    [SerializeField]
    bool isFullScreenMode = false;

    // Start is called before the first frame update
    void Start() {
        UIManager.Instance.ActiveUI = this;
        AudioManager.Instance.Play("Miss_You_Love_-_Patino", true);

        SetResolution(new Vector2Int(1920, 1080));

        //var resolutions = Screen.resolutions;
        //resolutionDropdown.options.Clear();

        //var targetAspect = 640.0f / 360.0f;

        //for (int i = 0; i < resolutions.Length; i++) {
        //    var resolution = resolutions[i];

        //    var aspect = (float)resolution.width / (float)resolution.height;
        //    if (aspect != targetAspect) {
        //        continue;
        //    }

        //    resolutionDropdown.options.Add(new Dropdown.OptionData(string.Format("{0}x{1} {2}Hz", resolution.width, resolution.height, resolution.refreshRate)));

        //    if (Screen.currentResolution.width == resolution.width && Screen.currentResolution.height == resolution.height && Screen.currentResolution.refreshRate == resolution.refreshRate) {
        //        resolutionDropdown.value = (resolutionDropdown.options.Count - 1);
        //    }
        //}

        //resolutionDropdown.onValueChanged.AddListener(SetResolutionDropdown);
    }

    public void StartGame() {
        StartCoroutine(WaitScene("Stage"));
    }

    public void Settings() {
        settings.SetActive(true);
    }

    public void BackToMenu() {
        settings.SetActive(false);
    }

    public void Exit() {
        StartCoroutine(WaitAction(
            delegate {
                Application.Quit();
            }
        ));
    }

    public void ShowHowToPlays() {
        howToPlays.SetActive(true);
        SetHowToPlayPageIndex(0);
    }

    public void HideHowToPlays() {
        howToPlays.SetActive(false);
    }

    public void PassHowToPlaysPage(bool forward) {
        if (forward) {
            if (howToPlaysPageIndex + 1 < howToPlaysPages.Count) {
                SetHowToPlayPageIndex(howToPlaysPageIndex + 1);
            }
        } else {
            if (howToPlaysPageIndex - 1 >= 0) {
                SetHowToPlayPageIndex(howToPlaysPageIndex - 1);
            }
        }
    }

    public void SetResolutionButton(bool isLeft) {
        int count = resolutionDropdown.options.Count;
        int newIndex = (((isLeft) ? -1 : 1) + resolutionDropdown.value + count) % count;
        resolutionDropdown.value = newIndex;
    }

    public void SetResolutionDropdown(int index) {
        List<Dropdown.OptionData> options = resolutionDropdown.options;
        string option = options[index % options.Count].text;

        var hzs = option.Split(' ');
        var hz = hzs[1].Replace("hz", "");

        var array = hzs[0].Split('x');
        Vector2Int resolution = new Vector2Int();
        if (array.Length > 1) {
            int buffer = 0;
            if (int.TryParse(array[0], out buffer))
                resolution.x = buffer;
            if (int.TryParse(array[1], out buffer))
                resolution.y = buffer;

            SetResolution(resolution);
        }
    }

    public void ToggleFullScreen(bool isOn) {
        isFullScreenMode = isOn;

        if (resolutionDropdown.options.Count > 0) {
            var selected = resolutionDropdown.options[resolutionDropdown.value];
            string option = selected.text;

            var hzs = option.Split(' ');
            var hz = hzs[1].Replace("hz", "");

            var array = hzs[0].Split('x');

            Vector2Int resolution = new Vector2Int();
            if (array.Length > 1) {
                int buffer = 0;
                if (int.TryParse(array[0], out buffer)) {
                    resolution.x = buffer;
                }

                if (int.TryParse(array[1], out buffer)) {
                    resolution.y = buffer;
                }

                SetResolution(resolution);
            }
        }
    }

    private void SetResolution(Vector2Int resolution) {
        Screen.SetResolution(resolution.x, resolution.y, isFullScreenMode);
    }

    private void SetHowToPlayPageIndex(int index) {
        if (howToPlaysPages.Count <= index) {
            return;
        }

        howToPlaysPageIndex = index;
        foreach (var page in howToPlaysPages) {
            page.SetActive(false);
        }

        howToPlaysPages[howToPlaysPageIndex].SetActive(true);

        howToPlaysNextButton.SetActive(howToPlaysPageIndex > 0);
        howToPlaysPrevButton.SetActive(howToPlaysPageIndex < howToPlaysPages.Count - 1);

    }
}
