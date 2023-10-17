using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameState currentState;

    public void Start()
    {
        currentState.OnStateEnter();
    }

    public void Change(GameState newState)
    {
        if (currentState == newState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = newState;
        UIManager.Instance.Offset = currentState.transform.position;

        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }
    public void Next()
    {
        if (currentState == null)
        {
            return;
        }

        if (currentState.nextState == null)
        {
            return;
        }

        Change(currentState.nextState);
    }
    public void Previous()
    {
        if (currentState == null)
        {
            return;
        }

        if (currentState.previousState == null)
        {
            return;
        }

        Change(currentState.previousState);
    }
}
