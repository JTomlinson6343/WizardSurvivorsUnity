using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    NONE,
    PLAYING,
    UPGRADING,
    PAUSED,
    PRE_BOSS,
    BOSS,
    GAME_OVER
}

public static class StateManager
{
    private static State m_CurrentState = State.NONE;

    private static State m_PreviousState = State.NONE; // State before game was paused

    public static void ChangeState(State state)
    {
        m_CurrentState = state;
    }

    public static State GetCurrentState()
    {
        return m_CurrentState;
    }
    public static State GetPreviousState()
    {
        return m_PreviousState;
    }

    public static void ToggleUpgrading(bool toggle)
    {
        if (toggle)
        {
            if (m_CurrentState != State.UPGRADING)
                m_PreviousState = m_CurrentState;

            Time.timeScale = 0;
            ChangeState(State.UPGRADING);
        }
        else
        {
            Time.timeScale = 1;
            ChangeState(m_PreviousState);
        }
    }

    public static void TogglePause(bool toggle)
    {
        if (toggle)
        {
            if (m_CurrentState != State.PAUSED)
                m_PreviousState = m_CurrentState;

            Time.timeScale = 0;
            ChangeState(State.PAUSED);
        }
        else
        {
            Time.timeScale = 1;
            ChangeState(m_PreviousState);
        }
    }

    public static bool IsGameplayStopped()
    {
        return m_CurrentState == State.UPGRADING || m_CurrentState == State.PAUSED || m_CurrentState == State.GAME_OVER;
    }
}
