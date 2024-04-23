using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public UIController() { instance = this; }
    public List<UImodule> uiModules;

    private Dictionary<string, Dictionary<string, UIComponent>> modulesDic = new Dictionary<string, Dictionary<string, UIComponent>>();
    private GameStateController gameStateController;

    [HideInInspector] public UIComponent helpButton;

    private void Start()
    {
        Init();
    }

    public void ChangeUIState(GameState nextState)
    {
        switch (nextState)
        {
            case GameState.Menu:
                modulesDic["menu"]["menuModule"].On();
                modulesDic["game"]["gameModule"].Off();
                modulesDic["saveLoad"]["saveModule"].Off();
                modulesDic["saveLoad"]["previews"].Off();
                break;
            case GameState.Game:
                modulesDic["menu"]["menuModule"].Off();
                modulesDic["game"]["gameModule"].On();
                modulesDic["game"]["brushModule"].Off();
                break;
            case GameState.BrushSettings:
                modulesDic["game"]["brushModule"].On();
                break;
            case GameState.SaveLoadTab:
                modulesDic["menu"]["menuModule"].Off();
                modulesDic["saveLoad"]["saveModule"].On();
                modulesDic["saveLoad"]["previews"].On();
                break;
        }
    }


    public void Init()
    {
        for (int i = 0; i < uiModules.Count; i++)
        {
            Dictionary<string, UIComponent> componentsDic = new Dictionary<string, UIComponent>();
            for (int j = 0; j < uiModules[i].uiComponents.Count; j++)
            {
                componentsDic.Add(uiModules[i].uiComponents[j].componentName, uiModules[i].uiComponents[j].uiComponent);
            }
            modulesDic.Add(uiModules[i].moduleName, componentsDic);
        }

        gameStateController = GameStateController.instance;


        modulesDic["menu"]["menu>game"].GetButtonComponent().onClick.AddListener(() => gameStateController.ChangeState(GameState.Game));
        modulesDic["menu"]["menu>saveLoad"].GetButtonComponent().onClick.AddListener(() => gameStateController.ChangeState(GameState.SaveLoadTab));
        modulesDic["game"]["brushButton"].GetButtonComponent().onClick.AddListener(() => gameStateController.ChangeState(GameState.BrushSettings));
        modulesDic["game"]["brushDone"].GetButtonComponent().onClick.AddListener(() => { gameStateController.ChangeState(GameState.Game); BrushSettings.instance.Done(); });
        modulesDic["game"]["game>menu"].GetButtonComponent().onClick.AddListener(() => gameStateController.ChangeState(GameState.Menu));
        modulesDic["saveLoad"]["save>menu"].GetButtonComponent().onClick.AddListener(() => gameStateController.ChangeState(GameState.Menu));
        modulesDic["saveLoad"]["delete"].GetButtonComponent().onClick.AddListener(() => PreviewController.instance.DeleteSelected());
        modulesDic["game"]["save"].GetButtonComponent().onClick.AddListener(() => SaveController.instance.Save());
        modulesDic["game"]["clear"].GetButtonComponent().onClick.AddListener(() => SaveController.instance.ClearImage());


    }    

}







[Serializable]
public struct UIcomp
{
    public string componentName;
    public UIComponent uiComponent;
}

[Serializable]
public struct UImodule
{
    public string moduleName;
    public List<UIcomp> uiComponents;
}


