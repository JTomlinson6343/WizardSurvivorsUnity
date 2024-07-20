using UnityEngine;

public class Knight : Boss
{
    public override void Enraged(int bossNumber)
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        if (Player.m_Instance == null) return;
        if (m_IsMidAnimation) return;

        Brain();
    }

    private void Brain()
    {

    }
}
