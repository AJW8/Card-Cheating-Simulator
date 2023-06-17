using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public int players;
    public int decks;
    public int handCount;
    public float cardGap;
    public Vector2 tableSize;
    public GameObject cardPrefab;
    private Card[] cards;
    private int[] playerHand;
    private int hoverCard;
    private int selectedCard;
    private int heldCard;

    void Awake()
    {
        cards = new Card[decks * 52];
        for (int i = 0; i < decks; i++)
        {
            for (int j = 0; j < 52; j++)
            {
                cards[i * 52 + j] = Instantiate(cardPrefab, transform.position, transform.rotation).GetComponent<Card>();
                cards[i * 52 + j].SetSuit(j / 13);
                cards[i * 52 + j].SetValue(j % 13);
            }
        }
        hoverCard = -1;
        selectedCard = -1;
        heldCard = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        Deal();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null)
        {
            if (hoverCard >= 0)
            {
                cards[hoverCard].SetHover(false);
                hoverCard = -1;
            }
        }
        else
        {
            int newHoverCard = hoverCard;
            Card c = hit.collider.GetComponent<Card>();
            for (int i = 0; i < cards.Length; i++) if (cards[i] == c) newHoverCard = i;
            if (hoverCard != newHoverCard)
            {
                if(hoverCard >= 0) cards[hoverCard].SetHover(false);
                cards[newHoverCard].SetHover(true);
                hoverCard = newHoverCard;
            }
        }
        if (Input.GetMouseButtonDown(0) && hoverCard >= 0 && hoverCard != selectedCard)
        {
            if (selectedCard >= 0) cards[selectedCard].SetSelected(false);
            cards[hoverCard].SetSelected(true);
            selectedCard = hoverCard;
        }
    }

    public void Deal()
    {
        if (selectedCard >= 0)
        {
            cards[selectedCard].SetSelected(false);
            selectedCard = -1;
        }
        playerHand = new int[handCount];
        bool[] dealt = new bool[decks * 52];
        if (heldCard >= 0) dealt[heldCard] = true;
        foreach (Card c in cards)
        {
            c.gameObject.transform.position = transform.position;
            c.SetPeeked(false);
        }
        for (int i = 0; i < players; i++)
        {
            Vector3 handPosition = transform.position + new Vector3(tableSize.x * 0.5f * Mathf.Sin(i * Mathf.PI * 2f / players), tableSize.y * 0.5f * -Mathf.Cos(i * Mathf.PI * 2f / players), 0);
            for (int j = 0; j < handCount; j++)
            {
                int r;
                do r = Random.Range(0, cards.Length); while (dealt[r]);
                cards[r].gameObject.transform.position = handPosition + new Vector3(cardGap * ((1 - handCount) * 0.5f + j), 0, 0);
                cards[r].GetComponent<SpriteRenderer>().sortingOrder = j;
                dealt[r] = true;
                if (i == 0) playerHand[j] = r;
            }
        }
        for (int i = 0; i < cards.Length; i++) if(!dealt[i]) cards[i].gameObject.transform.position = transform.position + new Vector3(0, 10, 0);
    }

    public Card GetSelectedCard()
    {
        return selectedCard >= 0 ? cards[selectedCard] : null;
    }

    public bool GetInPlayerHand()
    {
        if (selectedCard < 0) return false;
        for (int i = 0; i < playerHand.Length; i++) if (playerHand[i] == selectedCard) return true;
        return false;
    }

    public bool GetHoldingCard()
    {
        return heldCard >= 0;
    }

    public void PeekCard()
    {
        if (selectedCard < 0) return;
        cards[selectedCard].SetPeeked(true);
    }

    public void MarkCard()
    {
        if (selectedCard < 0) return;
        cards[selectedCard].Mark();
    }

    public void HoldCard()
    {
        if (heldCard >= 0) return;
        for (int i = 0; i < playerHand.Length; i++) if (playerHand[i] == selectedCard) heldCard = playerHand[i];
        if (heldCard < 0) return;
        cards[heldCard].SetSelected(false);
        cards[heldCard].gameObject.transform.position = transform.position + new Vector3(0, 10, 0);
        selectedCard = -1;
        List<int> newPlayerHand = new List<int>();
        for (int i = 0; i < playerHand.Length; i++) if(playerHand[i] != heldCard) newPlayerHand.Add(playerHand[i]);
        playerHand = newPlayerHand.ToArray();
        for (int i = 0; i < playerHand.Length; i++)
        {
            cards[playerHand[i]].gameObject.transform.position = transform.position + new Vector3(cardGap * ((2 - handCount) * 0.5f + i), -tableSize.y / 2, 0);
            cards[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    public void SwitchCard()
    {
        if (heldCard < 0) return;
        if (selectedCard >= 0) cards[selectedCard].SetSelected(false);
        cards[heldCard].SetSelected(true);
        selectedCard = heldCard;
        int[] newPlayerHand = new int[handCount];
        for (int i = 0; i < handCount - 1; i++) newPlayerHand[i] = playerHand[i];
        newPlayerHand[handCount - 1] = heldCard;
        heldCard = -1;
        playerHand = newPlayerHand;
        for (int i = 0; i < playerHand.Length; i++) cards[playerHand[i]].gameObject.transform.position = transform.position + new Vector3( cardGap * ((1 - handCount) * 0.5f + i), -tableSize.y / 2, i * 1f / handCount);
    }
}
