using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    NONE,
    PLAYING,
    PAUSED,
    ON_BOSS,
    GAME_OVER
}

public static class StateManager
{
    private static State m_CurrentState = State.NONE;

    public static void ChangeState(State state)
    {
        m_CurrentState = state;
    }

    public static State GetCurrentState()
    {
        return m_CurrentState;
    }
}
