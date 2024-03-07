using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField] float m_Speed;

    [SerializeField] Button m_BackButton;
    [SerializeField] Button m_UnlockButton;
    [SerializeField] Button m_RespecButton;

    Vector3 m_StartPos;

    private void Awake()
    {
        m_StartPos = transform.position;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleButtonInput();
    }
    private void HandleMovementInput()
    {
        Vector3 moveDir = new Vector2(Input.GetAxis("HorizontalDPAD"), Input.GetAxis("VerticalDPAD"));

        GetComponent<Rigidbody2D>().velocity = moveDir.normalized * m_Speed;
    }

    private void HandleButtonInput()
    {
        if (Input.GetButtonDown("Cancel")) m_BackButton.onClick.Invoke();
        if (Input.GetButtonDown("Submit"))
        {
            if (m_UnlockButton.interactable && m_UnlockButton.isActiveAndEnabled)
                m_UnlockButton.onClick.Invoke();
        }
        if (Input.GetButtonDown("Respec"))
        {
            m_RespecButton.onClick.Invoke();
            transform.position = m_StartPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<SkillIcon>().OnClick();
    }
}