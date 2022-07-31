using System.Collections.Generic;
using UnityEngine;
using CommonLib;
using System;

public static class CardUtils
{
    public static int id = 0;
    public static int GetId()
    {
        id += 1;
        return id;
    }

    public static void Reset()
    {
        id = 0;
    }
}

public class Board : MonoBehaviour
{
    private CardStack _bankStack;
    private CardStack _playedStack;

    private Stack<int> _history;
    private List<CardStack> _gameCardStacks;

    [SerializeField] private ProbabilityConfig _probabilityConfig;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private CardsConfig _cardsConfig;
    [SerializeField] private Core _core;

    public event Action onCardsDealt;
    public event Action<bool, int> onCardSelectResult;
    public event Action<int> onCardBackReturn;

    public ICardStack BankStack => _bankStack;
    public ICardStack PlayedStack => _playedStack;
    public List<ICardStack> GameCardStack => new List<ICardStack>(_gameCardStacks);

    public void Init()
    {

        _gameCardStacks = new List<CardStack>();
        for (int i = 0; i < _gameConfig.stackCardCount; i++)
        {
            _gameCardStacks.Add(new CardStack(i));
        }
        _bankStack = new CardStack(_gameCardStacks.Count);
        _playedStack = new CardStack(_gameCardStacks.Count + 1);
        _gameCardStacks.Add(_bankStack);
        _gameCardStacks.Add(_playedStack);
        _history = new Stack<int>();
    }

    public void StartDeal()
    {
        ClearData();
        DealCards();
        onCardsDealt?.Invoke();
    }

    public void BackStep()
    {
        if (_history.Count == 0) return;

        var stackIndex = _history.Pop();

        _gameCardStacks[stackIndex].Push(_playedStack.Get());

        onCardBackReturn?.Invoke(stackIndex);
    }

    public void SelectCard(int stackIndex)
    {
        var result = CheckCardCanMove(_playedStack.Peek(), _gameCardStacks[stackIndex].Peek());
        if (result)
        {
            _playedStack.Push(_gameCardStacks[stackIndex].Get());
            _history.Push(stackIndex);
        }
        onCardSelectResult?.Invoke(result, stackIndex);
    }

    private bool CheckCardCanMove(Card card, Card selectedCard)
    {
        if (selectedCard.StackIndex == _gameConfig.stackCardCount) return true;
        var activeCardRaiting = card.GetRaiting();
        var selectedCardRaiting = selectedCard.GetRaiting();
        var dealta = Mathf.Abs(activeCardRaiting - selectedCardRaiting);
        return dealta == _gameConfig.cardDifferenceRaitingMin || dealta == _gameConfig.cardDifferenceRaitingMax;
    }

    public void CheckEnd()
    {
        var isWin = true;
        var isHaveStep = false;
        var isHaveBank = _bankStack.Count != 0;
        var plaingCard = _playedStack.Peek();
        for (int i = 0; i < _gameConfig.stackCardCount; i++)
        {
            var stack = _gameCardStacks[i];
            if (stack.Count > 0)
            {
                isWin = false;
                var card = stack.Peek();
                isHaveStep = CheckCardCanMove(plaingCard, card);
                if (isHaveStep) break;
            }
        }
        if (!isWin && (isHaveBank || isHaveStep)) return;
        else _core.GameResult(isWin);
    }

    private void DealCards()
    {
        var combs = CreateCombs(CreateCombsData());
        DistributeCombs(combs);
        StartFirstComb();
    }

    private void DistributeCombs(List<Comb> combs)
    {
        for (int i = 0; i < combs.Count; i++)
        {
            DistributeComb(combs[i]);
        }
    }

    private void DistributeComb(Comb comb)
    {
        var list = comb.list;
        var lastIndex = list.Count - 1;
        for (int i = 0; i < lastIndex; i++)
        {
            var stackIndex = ProbabilityUtils.GetRandomIntByRange(0, _gameConfig.stackCardCount - 1);
            _gameCardStacks[stackIndex].Push(list[i]);
        }
        _bankStack.Push(list[lastIndex]);
    }

    private void StartFirstComb()
    {
        _playedStack.Push(_bankStack.Get());
    }

    private List<Comb> CreateCombs(List<CombsData> dataList)
    {
        var list = new List<Comb>();
        foreach (var item in dataList)
        {
            list.Add(new Comb(item, _cardsConfig));
        }
        return list;
    }

    private List<CombsData> CreateCombsData()
    {
        var list = new List<CombsData>();
        var cardCount = _gameConfig.baseDeckCardCount;
        while (cardCount > _gameConfig.maxCombCardCount)
        {
            var combData = GetRandomCombsData();

            list.Add(combData);
            cardCount -= combData.count;
        }
        if (cardCount > 0)
        {
            if (cardCount >= 2)
            {
                list.Add(GetRandomCombsData(cardCount));
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.count < _gameConfig.maxCombCardCount)
                    {
                        item.count += 1;
                        break;
                    }
                }
            }
        }
        return list;
    }

    private CombTypes GetRandomCombsType()
    {
        if (ProbabilityUtils.GetRandomBoolByProbability(_probabilityConfig.ascendingProbabilty))
            return CombTypes.Ascending;
        else return CombTypes.Descending;
    }

    private CombsData GetRandomCombsData(int forceCount = 0)
    {
        var count = forceCount > 0 ? forceCount :
            ProbabilityUtils.GetRandomIntByRange(_gameConfig.minCombCardCount, _gameConfig.maxCombCardCount);

        var type = GetRandomCombsType();

        var data = new CombsData()
        {
            isChanging = count <= 2 ? false : ProbabilityUtils.GetRandomBoolByProbability(_probabilityConfig.changingProbabilty),
            startRaiting = ProbabilityUtils.GetRandomIntByRange(1, _cardsConfig.cardRaiting.Count - 1),
            count = count,
            type = type
        };
        return data;
    }

    private void ClearData()
    {
        _history.Clear();
        CardUtils.Reset();
        _bankStack.Clear();
        _playedStack.Clear();
        foreach (var item in _gameCardStacks) item.Clear();
    }
}
