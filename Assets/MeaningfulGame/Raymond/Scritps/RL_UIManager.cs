using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Pseudo Singleton
public class RL_UIManager : MonoBehaviour
{
    private static RL_UIManager m_instance;
    public static RL_UIManager Instance
    {
        get
        {
            if (m_instance == null)
                throw new System.Exception("[UI Manager] Instance not created.");

            return m_instance;
        }
    }

    [SerializeField]
    private Camera mainCamera;
    private BlurOptimized blurEffect;
    [SerializeField]
    private Image ui_progressRadial;
    [SerializeField]
    private Image ui_Newspaper;
    [SerializeField]
    private Image ui_postIt;
    [SerializeField]
    private Image ui_bookShelf;
    [SerializeField]
    private Image ui_blueprint;
    [SerializeField]
    private InputField ui_password;

    void Start()
    {
        m_instance = this;

        if (mainCamera == null)
            throw new System.Exception("[UI Manager] Camera not assigned.");
        else
        {
            blurEffect = mainCamera.gameObject.GetComponent<BlurOptimized>();
            if (blurEffect == null)
                throw new System.Exception("[UI Manager] Camera's Blur Effect not assigned.");
            else
                blurEffect.enabled = false;
        }

        if (ui_progressRadial == null)
            throw new System.Exception("[UI Manager] Progress Radial not assigned.");
        else
            ui_progressRadial.gameObject.SetActive(false);

        if (ui_Newspaper == null)
            throw new System.Exception("[UI Manager] Newspaper not assigned.");
        else
            ui_Newspaper.gameObject.SetActive(false);

        if (ui_postIt == null)
            throw new System.Exception("[UI Manager] Post-it not assigned.");
        else
            ui_postIt.gameObject.SetActive(false);

        if (ui_bookShelf == null)
            throw new System.Exception("[UI Manager] Bookshelf not assigned.");
        else
            ui_bookShelf.gameObject.SetActive(false);

        if (ui_blueprint == null)
            throw new System.Exception("[UI Manager] Blueprint not assigned.");
        else
            ui_blueprint.gameObject.SetActive(false);

        if (ui_password == null)
            throw new System.Exception("[UI Manager] Password not assigned.");
        else
            ui_password.gameObject.SetActive(false);
    }

    public void ShowProgressRadial(float percentage)
    {
        ui_progressRadial.gameObject.SetActive(true);
        ui_progressRadial.fillAmount = percentage;
    }

    public void ShowUI(string name)
    {
        blurEffect.enabled = false;

        switch (name)
        {
            //case "Progress Radial":
            //    ui_progressRadial.gameObject.SetActive(true);
            //    break;

            case "Newspaper":
                ui_Newspaper.gameObject.SetActive(true);
                break;

            case "Post-it":
                ui_postIt.gameObject.SetActive(true);
                break;

            case "Bookshelf":
                ui_bookShelf.gameObject.SetActive(true);
                break;

            case "Blueprint":
                ui_blueprint.gameObject.SetActive(true);
                break;

            case "Password":
                ui_password.gameObject.SetActive(true);
                break;
        }
    }

    public void HideUI()
    {
        blurEffect.enabled = false;

        ui_progressRadial.gameObject.SetActive(false);
        ui_Newspaper.gameObject.SetActive(false);
        ui_postIt.gameObject.SetActive(false);
        ui_bookShelf.gameObject.SetActive(false);
        ui_blueprint.gameObject.SetActive(false);
        ui_password.gameObject.SetActive(false);
    }
}