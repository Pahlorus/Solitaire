using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class BaseAnimModule : MonoBehaviour
{
    private Sequence _scaleSeq;

    public async UniTask Move(Vector3 destPosition,  Vector3 startPosition, float durationTime, CancellationToken ct, bool isTimeScaleIgnore = false, Ease ease = Ease.Linear)
    {
        if (ct.IsCancellationRequested) return;
        if (startPosition != transform.position) transform.position = startPosition;
        await transform.DOMove(destPosition, durationTime).SetUpdate(isTimeScaleIgnore).SetEase(ease).AsyncWaitForCompletion();
    }

    public async UniTask Move(bool to, MoveAnimData data, CancellationToken ct, bool isTimeScaleIgnore = false)
    {
        var finPos = to ? data.destPosition : data.startPosition;
        var startPos = to ? data.startPosition : data.destPosition;
        var duration = to ? data.durationTimeDest : data.durationTimeStart;
        var ease = to ? data.easeDest : data.easeStart;
        await Move(finPos, startPos, duration, ct, isTimeScaleIgnore, ease);
    }

    public async UniTask MoveLocale(Vector3 destPosition, Vector3 startPosition, float durationTime, CancellationToken ct, bool isTimeScaleIgnore = false, Ease ease = Ease.Linear)
    {
        if (ct.IsCancellationRequested) return;
        if (startPosition != transform.localPosition) transform.localPosition = startPosition;
        await transform.DOLocalMove(destPosition, durationTime).SetUpdate(isTimeScaleIgnore).SetEase(ease).AsyncWaitForCompletion();
    }

    public async UniTask MoveLocale(bool to, MoveAnimData data, CancellationToken ct, bool isTimeScaleIgnore = false)
    {
        var finPos = to ? data.destPosition : data.startPosition;
        var startPos = to ? data.startPosition : data.destPosition;
        var duration = to ? data.durationTimeDest: data.durationTimeStart;
        var ease = to ? data.easeDest : data.easeStart;
        await MoveLocale(finPos, startPos, duration, ct, isTimeScaleIgnore, ease);
    }

    public void SetInstantMoveLocale(Vector3 destPosition)
    {
        MoveAnimReset();
        transform.localPosition = destPosition;
    }

    public void SetInstantMoveLocale(bool to, MoveAnimData data)
    {
        var finPos = to ? data.destPosition : data.startPosition;
        SetInstantMoveLocale(finPos);
    }

    public void SetInstantMove(Vector3 destPosition)
    {
        MoveAnimReset();
        transform.position = destPosition;
    }

    public void SetInstantMove(bool to, MoveAnimData data)
    {
        var finPos = to ? data.destPosition : data.startPosition;
        SetInstantMove(finPos);
    }

    public async UniTask Scale(Vector3 finScale, Vector3 startScale, float durationTime, CancellationToken ct,  bool isTimeScaleIgnore = false, Ease ease = Ease.Linear)
    {
        if (ct.IsCancellationRequested) return;
        transform.localScale = startScale;
        await transform.DOScale(finScale, durationTime).SetUpdate(isTimeScaleIgnore).SetEase(ease).AsyncWaitForCompletion();
    }

    public async UniTask Scale(ScaleAnimData data, float durationTime, CancellationToken ct, bool isTimeScaleIgnore = false, Ease ease = Ease.Linear)
    {
        await Scale(data.finScale, data.startScale, durationTime, ct,  isTimeScaleIgnore, ease);
    }

    public async UniTask Scale(bool up, ScaleAnimData data, CancellationToken ct, bool isTimeScaleIgnore = false)
    {
        var finScale = up ? data.finScale : data.startScale;
        var startScale = up ? data.startScale : data.finScale;
        var duration = up ? data.scaleUpDuration : data.scaleDownDuration;
        var ease = up ? data.upEase : data.downEase;
        await Scale(finScale, startScale, duration, ct, isTimeScaleIgnore, ease);
    }

    public async UniTask Scale(bool up, SScaleAnimData data, CancellationToken ct, bool isTimeScaleIgnore = false)
    {
        var finScale = up ? data.finScale : data.startScale;
        var startScale = up ? data.startScale : data.finScale;
        var duration = up ? data.scaleUpDuration : data.scaleDownDuration;
        var ease = up ? data.upEase : data.downEase;
        await Scale(finScale, startScale, duration, ct, isTimeScaleIgnore, ease);
    }

    public async UniTask Scale(SScaleAnimData data, float durationTime, CancellationToken ct, bool isTimeScaleIgnore = false, Ease ease = Ease.Linear)
    {
        await Scale(data.finScale, data.startScale, durationTime, ct,  isTimeScaleIgnore, ease);
    }

    public void SetInstantScale(bool to, ScaleAnimData data)
    {
        var scale = to ? data.finScale : data.startScale;
        SetInstantScale(scale);
    }

    public void SetInstantScale(Vector3 finScale)
    {
        ScaleAnimReset();
        transform.localScale = finScale;
    }

    public async UniTask Bubble(Vector3 finScale, Vector3 startScale, float scaleUpDuration, float scaleDownDuration, CancellationToken ct, Ease upEase = Ease.Linear, Ease downEase = Ease.Linear)
    {
        if (ct.IsCancellationRequested) return;
        if (_scaleSeq != default) return;
        transform.localScale = startScale;
        var tweenerScaleUp = transform.DOScale(finScale, scaleUpDuration).SetEase(upEase).SetUpdate(true);
        var tweenerScaleDown = transform.DOScale(startScale, scaleDownDuration).SetEase(downEase).SetUpdate(true);
        _scaleSeq = DOTween.Sequence();
        _scaleSeq.Append(tweenerScaleUp);
        _scaleSeq.Append(tweenerScaleDown);
        _scaleSeq.SetUpdate(true);
        await _scaleSeq.AsyncWaitForCompletion();
        _scaleSeq = default;
    }

    public async UniTask Bubble(ScaleAnimData data, CancellationToken ct)
    {
        await Bubble(data.finScale, data.startScale, data.scaleUpDuration, data.scaleDownDuration, ct, data.upEase, data.downEase);
    }

    public async UniTask Bubble(SScaleAnimData data, CancellationToken ct)
    {
        await Bubble(data.finScale, data.startScale, data.scaleUpDuration, data.scaleDownDuration, ct, data.upEase, data.downEase);
    }

    public async UniTask BubbleHeight(ScaleAnimData data, CancellationToken ct)
    {
        if (ct.IsCancellationRequested) return;
        if (_scaleSeq != default) return;
        transform.localScale = data.startScale;
        var tweenerScaleUp = transform.DOScaleY(data.finScale.y, data.scaleUpDuration).SetEase(data.upEase);
        var tweenerScaleDown = transform.DOScaleY(data.startScale.y, data.scaleDownDuration).SetEase(data.downEase);
        _scaleSeq = DOTween.Sequence();
        _scaleSeq.Append(tweenerScaleUp);
        _scaleSeq.Append(tweenerScaleDown);
        await _scaleSeq.AsyncWaitForCompletion();
        _scaleSeq = default;
    }

    public virtual void AnimReset()
    {
        MoveAnimReset();
        ScaleAnimReset();
    }

    private void ScaleAnimReset()
    {
        transform.DOKill();
        if (_scaleSeq != null) { _scaleSeq.Kill(); _scaleSeq = default; }
    }

    private void MoveAnimReset()
    {
        transform.DOKill();
    }

    private void OnDestroy()
    {
        AnimReset();
    }
}
