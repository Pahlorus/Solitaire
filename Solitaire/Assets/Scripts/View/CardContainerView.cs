using UnityEngine;

public class CardContainerView : MonoBehaviour
{
    [SerializeField] private Transform _container;

    public int Count => _container.childCount;
    public Vector3 Pos => transform.position;

    public void Push(CardView card, bool toBegin = false)
    {
        card.SetParent(_container, toBegin);
    }

    public void Push(CardView card)
    {
        SwitchActiveTopCard(false);
        card.SetParent(_container);
    }

    public CardView Get()
    {
        var cardTr = GetTopCard();
        return cardTr == default ? default : cardTr.GetComponent<CardView>();
    }

    public void SwitchActiveTopCard(bool active)
    {
        var cardGo = GetTopCard();
        if (cardGo != default) cardGo.gameObject.SetActive(active);
    }

    private Transform GetTopCard()
    {
        var count = Count;
        if (count > 0)
        {
            return _container.GetChild(count - 1);
        }
        else return default;
    }
}

