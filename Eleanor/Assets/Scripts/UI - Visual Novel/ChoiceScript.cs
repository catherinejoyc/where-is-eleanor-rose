using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceScript : MonoBehaviour
{
    public string path1;
    public string path2;

    public GameObject path1Script;
    public GameObject path2Script;

    private void Awake()
    {
        // activate choice Panel
        UIManager.Instance.choicePanel.SetActive(true);

        // set button texts
        UIManager.Instance.path1.text = path1;
        UIManager.Instance.path2.text = path2;

        // set methods
        UIManager.Instance.path1Btn.onClick.AddListener(Path1);
        UIManager.Instance.path2Btn.onClick.AddListener(Path2);
    }

    public void Path1()
    {
        // activate path1
        path1Script.SetActive(true);

        // deactivate choicePanel
        UIManager.Instance.choicePanel.SetActive(false);

        // remove listeners
        UIManager.Instance.path1Btn.onClick.RemoveAllListeners();
        UIManager.Instance.path2Btn.onClick.RemoveAllListeners();
    }

    public void Path2()
    {
        // activate path2
        path2Script.SetActive(true);

        // deactivate choicePanel
        UIManager.Instance.choicePanel.SetActive(false);

        // remove listeners
        UIManager.Instance.path1Btn.onClick.RemoveAllListeners();
        UIManager.Instance.path2Btn.onClick.RemoveAllListeners();
    }
}
