using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : UIBase {
    public PuzzleMap PuzzleMap { get { return puzzleMap; } }
    [SerializeField]
    private Text textTime;

    [SerializeField]
    private Button buttonAction;

    [SerializeField]
    private InteractionMap interactionMap;

    [SerializeField]
    private PuzzleMap puzzleMap;

    [SerializeField]
    private Player player;

    [SerializeField]
    private bool finished = false;

    [SerializeField]
    private GameState prevState;

    [SerializeField]
    private Vector3 prevPlayerLocation;

    private void Start() {
        interactionMap.gameObject.SetActive(true);
        puzzleMap.gameObject.SetActive(false);

        UIManager.Instance.SetStage(UIStageType.Interaction);
        UIManager.Instance.ActiveUI = this;

        prevPlayerLocation = player.transform.position;
        prevState = puzzleMap.MainView;

        VirtualMachine.Instance.CleanUp();
    }

    private void FixedUpdate() {
        Value tempValue = null;
        if (VirtualMachine.Instance.TryGetVar("Time", ref tempValue)) {
            TimeValue timeValue = null;
            if (tempValue != null && tempValue is TimeValue) {
                timeValue = tempValue as TimeValue;
                textTime.text = string.Format("DAY {0} {1:00}:00 {2}", timeValue.Days, timeValue.MeridiemHours, timeValue.Meridiem);

                if (!finished) {
                    if (!(finished = CheckFailure(timeValue))) {
                        finished = CheckSuccess();
                    }
                }
            }
        }
    }

    public void ToggleStage() {
        switch (UIManager.Instance.Stage) {
            case UIStageType.Puzzle:
                SetInteractionStage();
                break;

            case UIStageType.Interaction:
                SetPuzzleStage();
                break;
        }
    }

    public void SetStage(UIStageType stage) {
        if (UIManager.Instance.Stage == stage) {
            return;
        }

        switch (stage) {
            case UIStageType.Puzzle:
                SetPuzzleStage();
                break;

            case UIStageType.Interaction:
                SetInteractionStage();
                break;
        }
    }

    protected void SetPuzzleStage() {
        if (player.interact != null) {
            return;
        }

        UIManager.Instance.SetStage(UIStageType.Puzzle);
        
        {
            var old = player.transform.position;
            player.transform.position = prevPlayerLocation;
            prevPlayerLocation = old;
        }

        {
            var old = GameStateManager.instance.currentState;
            GameStateManager.instance.Change(prevState);
            prevState = old;
        }

        interactionMap.gameObject.SetActive(false);
        puzzleMap.gameObject.SetActive(true);
    }

    protected void SetInteractionStage() {
        UIManager.Instance.SetStage(UIStageType.Interaction);
        {
            var old = player.transform.position;
            player.transform.position = prevPlayerLocation;
            prevPlayerLocation = old;
        }

        {
            var old = GameStateManager.instance.currentState;
            GameStateManager.instance.Change(prevState);
            prevState = old;
        }

        interactionMap.gameObject.SetActive(true);
        puzzleMap.gameObject.SetActive(false);
    }

    private bool CheckSuccess() {
        Value tempValue = null;
        if (VirtualMachine.Instance.TryGetVar("Weather", ref tempValue)) {
            if (tempValue != null && tempValue is BoolValue) {
                var weatherValue = tempValue as BoolValue;
                if (weatherValue.Value) {
                    UIManager.Instance.SetEnding(EndingType.ItWasSummer);
                    StartCoroutine(WaitScene("Ending", FadeType.FadeOut));

                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckFailure(TimeValue timeValue) {
        if (timeValue.Days > 31) {
            UIManager.Instance.SetEnding(EndingType.ItIsSummer);
            StartCoroutine(WaitScene("Ending", FadeType.FadeOut));
            return true;
        }
        return false;
    }
}
