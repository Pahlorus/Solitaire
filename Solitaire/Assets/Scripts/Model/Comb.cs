using System.Collections.Generic;
using UnityEngine;
using CommonLib;

public class Comb
{
    private CardsConfig _config;
    public Comb(CombsData data, CardsConfig cardConfig)
    {
        _config = cardConfig;
        list = new List<Card>();
        CreateCards(data);
    }
    public List<Card> list;

    private void CreateCards(CombsData data)
    {
        if (data.isChanging)
        {
            var beforeCount = Mathf.RoundToInt((float)data.count / 2);
            var afterCount = data.count - beforeCount;
            var afterType = data.type == CombTypes.Ascending ? CombTypes.Descending : CombTypes.Ascending;

            var beforeEndRaiting = CreateCommonComb(data.startRaiting, beforeCount, data.type);

            switch (afterType)
            {
                case CombTypes.Ascending: beforeEndRaiting += 1; break;
                case CombTypes.Descending: beforeEndRaiting -= 1; break;
            }
            var afterRaiting = RaitingCorrect(beforeEndRaiting);
            CreateCommonComb(afterRaiting, afterCount, afterType);

        }
        else CreateCommonComb(data.startRaiting, data.count, data.type);

    }

    private int CreateCommonComb(int startRaiting, int count, CombTypes type)
    {
        for (int i = 0; i < count; i++)
        {
            var card = new Card();
            CardSuits suit = (CardSuits)(ProbabilityUtils.GetRandomIntByRange(0, 3));
            var value = new CardValue()
            {
                raiting = _config.raitingDic[startRaiting],
                suit = suit
            };
            card.SetCard(value);
            list.Add(card);

            if (i < count - 1)
            {
                switch (type)
                {
                    case CombTypes.Ascending: startRaiting += 1; break;
                    case CombTypes.Descending: startRaiting -= 1; break;
                }
                startRaiting = RaitingCorrect(startRaiting);
            }
        }
        return startRaiting;
    }

    private int RaitingCorrect(int raiting)
    {
        if (raiting > _config.maxRaiting) return _config.minRaiting;
        if (raiting < _config.minRaiting) return _config.maxRaiting;
        return raiting;
    }
}
