using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] private Controller _controller;

    private CardContainerView _bankList;
    private CardContainerView _playedList;
    private List<CardContainerView> _gameContainersList;
    private List<CardView> _cardViewList;

    [SerializeField] private Transform _mainCardContainer;
    [SerializeField] private Transform _deckCardContainer;
    [SerializeField] private Transform _bankCardContainer;
    [SerializeField] private Transform _playedCardContainer;
    [SerializeField] private Button _backButton;
    [SerializeField] private EntityCreator _entityCreator;

    [Header("Data")]
    [SerializeField] private CardsConfig _cardConfig;
    [SerializeField] private GameConfig _gameConfig;


    public event Action onActionEnd;


    public bool Anim { get; private set; }

    public void Init()
    {
        _backButton.onClick.AddListener(BackStep);
        SwitchBackButton(false);
        _gameContainersList = new List<CardContainerView>();
        _cardViewList = new List<CardView>();
        CreateCards();
        CreateStacksView();
    }

    public void CardDeal(List<ICardStack> stacks)
    {
        SwitchBackButton(false);
        ReturnCardToDeck();
        Anim = true;
        StacksDeal(stacks).ContinueWith(() => { Anim = false; }).Forget();
    }

    private async UniTask StacksDeal(List<ICardStack> stacks)
    {
        var cardDeckIndex = 0;
        var visualDic = _cardConfig.spriteDic;
        for (int i = 0; i < stacks.Count; i++)
        {
            var stack = stacks[i];
            var stackIndex = stack.StackIndex;
            var cards = stack.Cards;
            var stackView = _gameContainersList[stackIndex];
            for (int j = cards.Length - 1; j >= 0; j--)
            {
                var card = cards[j];
                var cardView = _cardViewList[cardDeckIndex];
                //var stackIndex = card.StackIndex;
                cardView.SetCard(card.ID, stackIndex, card.Value.raiting.shortName, visualDic[card.Value.suit]);
                await UniTask.Delay(10);
                DealCard(cardView, stackView, j == 0).Forget();
                cardDeckIndex++;
            }
        }
    }

    private async UniTask DealCard(CardView cardView, CardContainerView stackView, bool isFirst = false)
    {
        await cardView.MoveCardAnim(stackView.Pos, cardView.Pos, _cardConfig.cardDealDuration, _cardConfig.moveEase);
        if (isFirst && !IsBankStak(cardView.StackIndex)) cardView.FlipCardInstant(revers: false);
        stackView.Push(cardView, toBegin: false);
    }
    private void CreateStacksView()
    {
        for (int i = 0; i < _gameConfig.stackCardCount; i++)
        {
            var stack = _entityCreator.CreateCardCantainerView(_mainCardContainer);
            _gameContainersList.Add(stack);
        }
        _bankList = _entityCreator.CreateCardCantainerView(_bankCardContainer);
        _playedList = _entityCreator.CreateCardCantainerView(_playedCardContainer);
        _gameContainersList.Add(_bankList);
        _gameContainersList.Add(_playedList);
    }

    private void CreateCards()
    {
        for (int i = 0; i < _gameConfig.baseDeckCardCount; i++)
        {
            var card = _entityCreator.CreateCardView(_deckCardContainer);
            card.onClickCard += CardClick;
            card.Init(_cardConfig.reversCardSprite, _gameConfig.isDebug);
            _cardViewList.Add(card);
        }
    }

    private void ReturnCardToDeck()
    {
        foreach (var item in _cardViewList)
        {
            item.FlipCardInstant(revers: true);
            item.transform.SetParent(_deckCardContainer);
            var itemGO = item.transform.gameObject;
            if (!itemGO.activeSelf) itemGO.SetActive(true);
        }
    }

    private bool IsBankStak(int index)
    {
        return index == _gameConfig.stackCardCount;
    }

    private void CardClick(int stackIndex)
    {
        if (Anim) return;
        _controller.CardClickHandler(stackIndex);
    }

    private void BackStep()
    {
        _controller.BackStep();
    }

    public void BackStepHandler(int stackIndex)
    {

        var isBankStack = IsBankStak(stackIndex);
        var destStack = _gameContainersList[stackIndex];
        var destStackTopCard = destStack.Get();
        destStackTopCard.FlipCardInstant(true);
        var cardView = _playedList.Get();
        destStack.Push(cardView, false);

        if (isBankStack) cardView.FlipCardInstant(true);
        _playedList.SwitchActiveTopCard(true);
        if (_playedList.Count < 2) SwitchBackButton(false);
    }

    public void CardSelectResultHandler(bool result, int stackIndex)
    {
        Anim = true;
        CardSelectResultAnim(result, stackIndex).ContinueWith(() => { Anim = false; }).Forget();
    }

    private async UniTask CardSelectResultAnim(bool result, int stackIndex)
    {
        if (result)
        {
            var isBankStack = IsBankStak(stackIndex);
            var sourceStack = _gameContainersList[stackIndex];
            var cardView = sourceStack.Get();
            if (isBankStack) await cardView.RotateCardAnim(false);
            await cardView.MoveCardAnim(_playedList.Pos, cardView.Pos, _cardConfig.cardMoveDuration, _cardConfig.moveEase);

            _playedList.Push(cardView);

            if (!isBankStack)
            {
                var cardViewNext = sourceStack.Get();
                if (cardViewNext == default) await UniTask.CompletedTask;
                else await cardViewNext.RotateCardAnim(false);
            }
            if (_playedList.Count > 1) SwitchBackButton(true);
            await UniTask.Delay(_gameConfig.timeDelayAfterAction);
            onActionEnd?.Invoke();
        }
    }

    private void SwitchBackButton(bool active)
    {
        if (_backButton.gameObject.activeSelf != active)
            _backButton.gameObject.SetActive(active);
    }
}
