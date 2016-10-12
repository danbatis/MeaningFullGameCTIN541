using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RL_AnimateUIText : MonoBehaviour
{
    public float interval;
    private Text textComp;
    private string oriText;

    void OnEnable()
    {
        textComp = GetComponent<Text>();
        oriText = textComp.text;

        textComp.text = "";
    }

    public void StartToAnimateUI()
    {
        StartCoroutine(AnimateUIText());
    }

    IEnumerator AnimateUIText()
    {
        string[] oriTextSplit = oriText.Split(new char[] { '\n' });
        string curText = "";

        int i = 0;
        while (i < oriTextSplit.Length)
        {
            curText += oriTextSplit[i++] + "\n";
            textComp.text = curText;
            yield return new WaitForSeconds(interval);
        }

    }
}
