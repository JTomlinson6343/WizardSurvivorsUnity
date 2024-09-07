using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class StateManager
{
    public enum State
    {
        NONE,
        PLAYING,
        UPGRADING,
        PAUSED,
        TUTORIAL,
        PRE_BOSS,
        BOSS,
        GAME_OVER
    }

    private static State m_CurrentState = State.NONE;

    private static State m_PreviousState = State.NONE; // State before game was paused

    public static void ChangeState(State state)
    {
        if (state == m_CurrentState) return;

        switch (m_CurrentState)
        {
            // If state is changed outside the pause menu, just change the previous state
            case State.PAUSED:
                m_PreviousState = state;
                break;
            case State.UPGRADING:
                m_PreviousState = state;
                break;
            case State.GAME_OVER:
                m_PreviousState = state;
                break;
            case State.TUTORIAL:
                m_PreviousState = state;
                break;
            // Otherwise switch the state to the new state
            default:
                m_PreviousState = m_CurrentState;
                m_CurrentState = state;
                break;
        }

        switch (state)
        {
            case State.PAUSED:
            case State.UPGRADING:
            case State.GAME_OVER:
            case State.TUTORIAL:
                Pause();
                break;
            default:
                break;
        }
        PrintState();
    }

    public static void ForceChangeState(State state)
    {
        m_CurrentState = state;
        PrintState();
    }

    private static void PrintState()
    {
        Debug.Log("Current State: " + GetCurrentState().ToString() + "\nPrevious State: " + GetPreviousState().ToString());
    }

    public static State GetCurrentState()
    {
        return m_CurrentState;
    }
    public static State GetPreviousState()
    {
        return m_PreviousState;
    }

    private static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void UnPause()
    {
        Time.timeScale = 1;

        (m_PreviousState, m_CurrentState) = (m_CurrentState, m_PreviousState);

        PrintState();
    }

    //public static void SetPreviousState(State state)
    //{
    //    m_PreviousState = state;
    //}

    //public static void ToggleUpgrading(bool toggle)
    //{
    //    if (toggle)
    //    {
    //        if (m_CurrentState != State.UPGRADING)
    //            m_PreviousState = m_CurrentState;

    //        Time.timeScale = 0;
    //        ChangeState(State.UPGRADING);
    //    }
    //    else
    //    {
    //        Time.timeScale = 1;
    //        ChangeState(m_PreviousState);
    //    }
    //}

    //public static void TogglePause(bool toggle)
    //{
    //    if (toggle)
    //    {
    //        if (m_CurrentState != State.PAUSED)
    //            m_PreviousState = m_CurrentState;

    //        Time.timeScale = 0;
    //        ChangeState(State.PAUSED);
    //    }
    //    else
    //    {
    //        Time.timeScale = 1;
    //        ChangeState(m_PreviousState);
    //    }
    //}

    public static bool IsGameplayStopped()
    {
        return m_CurrentState == State.UPGRADING || m_CurrentState == State.PAUSED || m_CurrentState == State.GAME_OVER;
    }
}
