using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _endButton;
    [SerializeField] private ViewConfig _config;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Init(UnityAction startAction, UnityAction endAction)
    {
        _startButton.onClick.AddListener(startAction);
        _endButton.onClick.AddListener(endAction);
    }

    public void SetState(GameState state)
    {
        SetDescription(state);
        SetActive(state);
        SetButtons(state);
    }

    private void SetActive(GameState state)
    {
        var active = false;
        switch (state)
        {
            default:
            case GameState.Win:
            case GameState.Defeat:
            case GameState.Ready: active = true; break;
            case GameState.Game: active = false; break;
        }
        if (active != gameObject.activeSelf) gameObject.SetActive(active);
    }

    private void SetDescription(GameState state)
    {
        var text = string.Empty;
        switch (state)
        {
            default: text = "undefined"; break;
            case GameState.Ready: text = _config.readyDescription; break;
            case GameState.Game: text = string.Empty; break;
            case GameState.Win: text = _config.winDescription; break;
            case GameState.Defeat: text = _config.defeatDescription; break;
        }
        _descriptionText.text = text;
    }

    private void SetButtons(GameState state)
    {
        var startButtonActive = false;
        var endButtonActive = false;
        switch (state)
        {
            default:
            case GameState.Game: break;
            case GameState.Ready: startButtonActive = true; break;
            case GameState.Win:
            case GameState.Defeat: endButtonActive = true; break;
        }
        _startButton.gameObject.SetActive(startButtonActive);
        _endButton.gameObject.SetActive(endButtonActive);
    }
}
