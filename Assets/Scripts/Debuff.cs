using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [SerializeField] float m_DebuffTime;
    float m_LastTick;
    [SerializeField] float m_TickInterval;
    [SerializeField] Actor m_ActorComponent;

    // Start is called before the first frame update
    void Start()
    {
        var debuffsOfThisType = GetComponents(GetType());
        if (debuffsOfThisType.Length>1)
        {
            Destroy(debuffsOfThisType[0]);
        }
        Invoke(nameof(EndDebuff),m_DebuffTime);
    }

    // Update is called once per frame
    void Update()
    {
        TickRoutine();
    }

    void TickRoutine()
    {
        float now = Time.realtimeSinceStartup;

        if (now - m_LastTick < m_TickInterval)
        {
            return;
        }
        OnTick();
        m_LastTick = now;
    }

    void EndDebuff()
    {
        Destroy(this);
    }

    virtual protected void OnTick()
    {

    }
}
