using System;
using UnityEngine;

public class Core : MonoBehaviour
{
    private GameState _state;
    [SerializeField] private Board _board;

    public event Action<GameState> onGameState;

    public GameState State => _state;

    public void Start()
    {
        _board.Init();
    }

    public void Ready()
    {
        onGameState?.Invoke(_state);
    }

    public void StartGame()
    {
        _state = GameState.Game;
        onGameState?.Invoke(_state);
    }

    public void GameResult(bool isWin)
    {
        _state = isWin ? GameState.Win : GameState.Defeat;
        onGameState?.Invoke(_state);
    }

    public void EndGame()
    {
        _state = GameState.Ready;
        onGameState?.Invoke(_state);
    }
}

