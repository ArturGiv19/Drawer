using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter.Sample;
using Es.InkPainter;

public class GameStateController : MonoBehaviour
{
    public static GameStateController instance;
    public GameStateController() { instance = this; }
    public GameState curGameState;

    public void ChangeState(GameState gameState)
    {
        MousePainter.instance.enabled = false;
        switch (gameState)
        {
            case GameState.Menu:
                PreviewController.instance.DeselectCur();
                break;
            case GameState.Game:
                MousePainter.instance.enabled = true;
                break;
            case GameState.BrushSettings:
                break;
            case GameState.SaveLoadTab:
                break;
        }
        UIController.instance.ChangeUIState(gameState);
    }
}

public enum GameState
{
    Menu,
    Game,
    BrushSettings,
    SaveLoadTab
}
