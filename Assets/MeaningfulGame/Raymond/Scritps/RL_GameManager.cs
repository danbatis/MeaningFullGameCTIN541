using UnityEngine;
using System.Collections;

// Pseudo Singleton
public class RL_GameManager : MonoBehaviour
{
    private static RL_GameManager m_instance;
    public static RL_GameManager Instance
    {
        get
        {
            if (m_instance == null)
                throw new System.Exception("[Game Manager] Instance not created.");

            return m_instance;
        }
    }

    public enum EStatus
    {
        Cinematic,
        Normal,
        Exploring,
        Checking
    }
    private EStatus m_status;
    public EStatus Status
    {
        get { return m_status; }
    }

    [SerializeField]
    private float exploreInverval = 1.0f;

    private RL_ITAItemBase m_curItemInField;
    public RL_ITAItemBase CurItemInField
    {
        set { m_curItemInField = value; }
    }
    private float m_exploredTime;

    void Start()
    {
        m_instance = this;
    }

    void Update()
    {
        if (m_status == EStatus.Cinematic)
            return;

        // control movement (tempory)
        RL_MainCharacterController controller = 
            GameObject.FindGameObjectWithTag("Player").GetComponent<RL_MainCharacterController>();

        // Process Input (the order is crucial)
        if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (m_curItemInField != null && m_curItemInField.CanBeChecked)
            {
                if (m_status != EStatus.Checking)
                {
                    m_status = EStatus.Checking;
                    m_curItemInField.CheckingToggle(true);

                    //controller.enabled = false;
                    controller.SetControllable(false);
                }
                else
                {
                    m_status = EStatus.Normal;
                    m_curItemInField.CheckingToggle(false);

                    //controller.enabled = true;
                    controller.SetControllable(true);
                }
            }

            return;
        }

        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
        {
            // Freeze movement (tempory)
            //controller.enabled = false;
            controller.SetControllable(false);

            if (m_status == EStatus.Normal)
            {
                // In an item's area, if the item is already explored, do not explore again
                if (m_curItemInField == null || !m_curItemInField.CanBeChecked)
                {
                    m_exploredTime = 0f;
                    StartCoroutine(Exploring());
                    m_status = EStatus.Exploring;
                }
            }
            else if (m_status == EStatus.Exploring)
            {
                if (m_exploredTime < exploreInverval)
                {
                    RL_UIManager.Instance.ShowProgressRadial(m_exploredTime / exploreInverval);
                }
                else
                {
                    StopCoroutine(Exploring());

                    // Find Something!!
                    if (m_curItemInField != null)
                        m_curItemInField.Explored();

                    RL_UIManager.Instance.HideUI();
                }
            }

            return;
        }

        if (Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (m_status != EStatus.Checking)
                // Free movement (tempory)
                //controller.enabled = true;
                controller.SetControllable(true);

            if (m_status == EStatus.Exploring)
            {
                StopCoroutine(Exploring());
                RL_UIManager.Instance.HideUI();

                m_status = EStatus.Normal;
            }
        }

    }

    public void CinematicOn()
    {
        RL_MainCharacterController controller =
            GameObject.FindGameObjectWithTag("Player").GetComponent<RL_MainCharacterController>();
        controller.SetControllable(false);

        m_status = EStatus.Cinematic;
    }
    public void CinematicOff()
    {
        RL_MainCharacterController controller =
            GameObject.FindGameObjectWithTag("Player").GetComponent<RL_MainCharacterController>();
        controller.SetControllable(true);

        m_status = EStatus.Normal;
    }

    protected IEnumerator Exploring()
    {
        while (m_exploredTime < exploreInverval)
        {
            m_exploredTime += Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }
    }
}