
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Core _core;
    [SerializeField] private Board _board;
    [SerializeField] private BoardView _boardView;
    [SerializeField] private PanelView _panelView;

    public void Start()
    {
        Init();
    }
    public void Init()
    {
        _boardView.Init();
        _panelView.Init(StartGameButtonHandler, EndGameButtonHandler);
        EventsSubscribe();
        GameReadyHandler();
    }

    public void EventsSubscribe()
    {
        _core.onGameState += _panelView.SetState;
        _core.onGameState += SetState;
        _board.onCardSelectResult += _boardView.CardSelectResultHandler;
        _board.onCardBackReturn += _boardView.BackStepHandler;
        _boardView.onActionEnd += _board.CheckEnd;
    }

    private void SetState(GameState state)
    {
        switch (state)
        {
            default:
            case GameState.Win:
            case GameState.Defeat:
            case GameState.Ready: break;
            case GameState.Game:
                {
                    _board.StartDeal();
                    _boardView.CardDeal(_board.GameCardStack);
                    break;
                }
        }
    }

    public void GameReadyHandler()
    {
        _core.Ready();
    }

    private void StartGameButtonHandler()
    {
        _core.StartGame();
    }

    private void EndGameButtonHandler()
    {
        _core.EndGame();
    }

    public void CardClickHandler(int stackIndex)
    {
        _board.SelectCard(stackIndex);
    }

    public void BackStep()
    {
        _board.BackStep();
    }
}
