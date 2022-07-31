using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    private bool _isDebug;

    private int _id;
    Sprite _suitSprite;
    Sprite _reverseSprite;

    [SerializeField] private Image _cardImage;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI _raitingText;
    [SerializeField] private TextMeshProUGUI _raitingInverseText;
    [SerializeField] private TextMeshProUGUI _centerText;
    [SerializeField] private BaseAnimModule _animModule;
    [SerializeField] private ScaleAnimData _rotateAnimData;
    [SerializeField] private ScaleAnimData _bubleAnimData;

    private int _stackIndex;
    public int StackIndex => _stackIndex;
    public Vector3 Pos => transform.position;

    public event Action<int> onClickCard;

    public void Init(Sprite reverseSprite, bool isDebug = false)
    {
        _isDebug = isDebug;
        idText.gameObject.SetActive(_isDebug);
        _button.onClick.AddListener(() => { onClickCard?.Invoke(_stackIndex); });
        _reverseSprite = reverseSprite;
        _raitingInverseText.enabled =
        _raitingText.enabled =
        _centerText.enabled = false;
        FlipCardInstant(revers: true);
    }

    public void SetCard(int id, int index, string name, CardVisual visual)
    {
        if (_isDebug)
        {
            _id = id;
            idText.text = _id.ToString();
        }

        _stackIndex = index;
        SetSuit(name, visual);
    }

    private void SetSuit(string name, CardVisual visual)
    {
        _suitSprite = visual.frontSprite;

        _raitingText.color =
        _raitingInverseText.color =
        _centerText.color = visual.cardColor;

        _raitingText.text =
        _raitingInverseText.text =
        _centerText.text = name;
    }

    public void FlipCardInstant(bool revers)
    {
        _raitingInverseText.enabled =
        _raitingText.enabled =
        _centerText.enabled = !revers;
        _cardImage.sprite = revers ? _reverseSprite : _suitSprite;
    }

    public void SetParent(Transform parent, bool toBegin = false)
    {
        transform.SetParent(parent);
        if (toBegin) transform.SetAsFirstSibling();
    }

    public async UniTask RotateCardAnim(bool revers)
    {
        var ct = transform.GetCancellationTokenOnDestroy();
        if (ct.IsCancellationRequested) return;
        await _animModule.Scale(up: true, _rotateAnimData, ct);
        FlipCardInstant(revers);
        if (ct.IsCancellationRequested) return;
        await _animModule.Scale(up: false, _rotateAnimData, ct);
    }

    public async UniTask MoveCardAnim(Vector3 dest, Vector3 source, float duration, Ease ease)
    {
        var ct = transform.GetCancellationTokenOnDestroy();
        if (ct.IsCancellationRequested) return;
        await _animModule.Move(dest, source, duration, ct, ease: ease);
        _animModule.transform.localPosition = Vector3.zero;
    }
}
