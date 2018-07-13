using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUI : BaseUI
{
    Button BtnSingle;
    Button BtnMuti;
    Button BtnQuit;

    protected override void InitUiOnAwake()
    {
        BtnSingle = GameTool.GetTheChildComponent<Button>(gameObject, "SingleButton");
        BtnMuti = GameTool.GetTheChildComponent<Button>(gameObject, "MutiButton");
        BtnQuit = GameTool.GetTheChildComponent<Button>(gameObject, "QuitButton");

        BtnSingle.onClick.AddListener(Single);
        BtnMuti.onClick.AddListener(Muti);
        BtnQuit.onClick.AddListener(Quit);
    }

    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.StartUI;
    }

    private void Single()
    {    
        StartCoroutine(LoadScene());
        UIManager.Instance.ShowUI(UIid.HUDUI);
        
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(2);
        yield return async;
        ResourcesManager.Instance.IsStandalone = true;
    }

    private void Muti()
    {
        UIManager.Instance.ShowUI(UIid.LoadingUI);
        ResourcesManager.Instance.IsStandalone = false;
    }

    private void Quit()
    {
        Application.Quit();
    }
}
