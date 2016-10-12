using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CinemaDirector;

[RequireComponent(typeof(AudioSource))]
public class RL_ITAItemPassword : RL_ITAItemBase
{
    [SerializeField]
    private InputField passwordInputField;
    private EventSystem eventSystem;

    [SerializeField]
    private Image backgroundOrigin;
    [SerializeField]
    private Image backgroundWrong;

    [SerializeField]
    private string correctCode = "1618";

    [SerializeField]
    private Cutscene bedroomCut02;
    private bool m_levelUp = false;

    private AudioSource audioSrc;
    [SerializeField]
    private AudioClip passwordFailClip;
    [SerializeField]
    private AudioClip passwordSucceedClip;

    public override void Awake()
    {
        base.Awake();

        m_name = "Password";
        eventSystem = FindObjectOfType<EventSystem>().GetComponent<EventSystem>();

        if (passwordInputField == null)
            Debug.LogError("Password Item has not been assigned the input field");
        if (backgroundOrigin == null)
            Debug.LogError("Password Item has not been assigned the background (original)");
        if (backgroundWrong == null)
            Debug.LogError("Password Item has not been assigned the background (wrong)");

        backgroundWrong.enabled = false;

        audioSrc = GetComponent<AudioSource>();
        if (passwordFailClip == null)
            Debug.LogError("Password Item has not been assigend the fail audio clip");
        if (passwordSucceedClip == null)
            Debug.LogError("Password Item has not been assigend the succeed audio clip");
    }

    public override void Update()
    {
        if (m_status == EStatus.Highlighted && 
            RL_GameManager.Instance.Status == RL_GameManager.EStatus.Checking)
        {
            eventSystem.SetSelectedGameObject(passwordInputField.gameObject);

            // Check Password
            string input = passwordInputField.text;
            
            if (input.Length == 4)
            {
                if (input != correctCode)
                {
                    backgroundOrigin.enabled = false;
                    backgroundWrong.enabled = true;

                    audioSrc.clip = passwordFailClip;
                    audioSrc.Play();
                }
                else
                {
                    //Succeed in escaping!
                    StartCoroutine(LevelUpAnimation());

                    audioSrc.clip = passwordSucceedClip;
                    audioSrc.Play();
                }
            }
            else
            {
                backgroundOrigin.enabled = true;
                backgroundWrong.enabled = false;
            }
        }
    }

    public override void OnTriggerStay(Collider col)
    {
        if (m_levelUp)
        { 
            foreach (GameObject original in Originals)
                original.SetActive(true);
            foreach (GameObject highlighted in Highlighteds)
                highlighted.SetActive(false);

            return;
        }

        base.OnTriggerStay(col);
    }

    public override void CheckingToggle(bool on)
    {
        base.CheckingToggle(on);

        if (on)
        {
            
        }
        else
        {
            eventSystem.SetSelectedGameObject(null);

            backgroundOrigin.enabled = true;
            backgroundWrong.enabled = false;
            passwordInputField.text = "";
        }
    }

    IEnumerator LevelUpAnimation()
    {
        m_levelUp = true;
        yield return new WaitForSeconds(2.0f);

        CheckingToggle(false);

        bedroomCut02.Play();

        yield return new WaitForSeconds(14.0f);
        LevelUp();
    }
    public void LevelUp()
    {
        SceneManager.LoadScene("maze");
    }
}
