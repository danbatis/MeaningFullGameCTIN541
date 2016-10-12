using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class RL_ITAItemBase : MonoBehaviour
{
    [SerializeField]
    protected string m_name;
    public string Name { get { return m_name; } }

    public GameObject[] Originals;
    public GameObject[] Highlighteds;

    public enum EStatus
    {
        Origin,
        Highlighted
    }
    [SerializeField]
    protected EStatus m_status = EStatus.Origin;

    public bool CanBeChecked
    {
        get { return m_status == EStatus.Highlighted; }
    }
    protected bool m_isExplored = false;

    public virtual void Awake()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }
    public virtual void Update()
    {

    }

    public virtual void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag != "Player")
            return;

        RL_GameManager.Instance.CurItemInField = this;

        if (m_isExplored)
        {
            m_status = EStatus.Highlighted;

            foreach (GameObject original in Originals)
                original.SetActive(false);
            foreach (GameObject highlighted in Highlighteds)
                highlighted.SetActive(true);
        }
    }

    public virtual void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag != "Player")
            return;

        RL_GameManager.Instance.CurItemInField = null;
        m_status = EStatus.Origin;

        foreach (GameObject original in Originals)
            original.SetActive(true);
        foreach (GameObject highlighted in Highlighteds)
            highlighted.SetActive(false);
    }

    public void Explored()
    {
        m_isExplored = true;

        m_status = EStatus.Highlighted;
        foreach (GameObject original in Originals)
            original.SetActive(false);
        foreach (GameObject highlighted in Highlighteds)
            highlighted.SetActive(true);
    }

    public virtual void CheckingToggle(bool on)
    {
        if (on)
        {
            RL_UIManager.Instance.ShowUI(m_name);
        }
        else
        {
            RL_UIManager.Instance.HideUI();
        }
    }
}