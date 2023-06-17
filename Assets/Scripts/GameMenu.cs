using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject buttonPeekCardObject;
    public GameObject buttonMarkCardObject;
    public GameObject buttonHoldCardObject;
    public GameObject buttonSwitchCardObject;
    public GameObject buttonDealObject;
    public GameObject textSelectedCardObject;
    public GameObject cardManagerObject;
    private CardManager cardManager;
    private Button buttonPeekCard;
    private Button buttonMarkCard;
    private Button buttonHoldCard;
    private Button buttonSwitchCard;
    private Button buttonDeal;
    private Text textSelectedCard;
    private Card selectedCard;

    // Start is called before the first frame update
    void Start()
    {
        cardManager = cardManagerObject.GetComponent<CardManager>();
        buttonPeekCard = buttonPeekCardObject.GetComponent<Button>();
        buttonMarkCard = buttonMarkCardObject.GetComponent<Button>();
        buttonHoldCard = buttonHoldCardObject.GetComponent<Button>();
        buttonSwitchCard = buttonSwitchCardObject.GetComponent<Button>();
        buttonDeal = buttonDealObject.GetComponent<Button>();
        textSelectedCard = textSelectedCardObject.GetComponent<Text>();
        selectedCard = null;
        UpdateView();
    }

    // Update is called once per frame
    void Update()
    {
        Card newSelectedCard = cardManager.GetSelectedCard();
        if (selectedCard != newSelectedCard)
        {
            selectedCard = newSelectedCard;
            UpdateView();
        }
    }

    private void UpdateView()
    {
        bool holdingCard = cardManager.GetHoldingCard();
        if (selectedCard == null)
        {
            buttonPeekCardObject.SetActive(false);
            buttonMarkCardObject.SetActive(false);
            buttonHoldCardObject.SetActive(false);
            textSelectedCard.text = "Click on any card to select it.";
        }
        else
        {
            bool inPlayerHand = cardManager.GetInPlayerHand();
            bool marked = selectedCard.GetMarked();
            bool peeked = selectedCard.GetPeeked();
            if (inPlayerHand) textSelectedCard.text = "The selected card is in your hand.\n\nIt is: " + selectedCard.GetValue() + " of " + selectedCard.GetSuit() + ".\n\n" + (marked ? "This card has been marked." : "Mark this card to keep track of it permanently.") + "\n\n" + (holdingCard ? "You cannot hold this card until you have switched in the card currently being held." : "Hold this card to remove it from your hand. You can switch it back into your hand at any time.");
            else if (marked) textSelectedCard.text = "The selected card has been marked.\n\nIt is: " + selectedCard.GetValue() + " of " + selectedCard.GetSuit() + ".";
            else if (peeked) textSelectedCard.text = "The selected card has been peeked at.\nIt is: " + selectedCard.GetValue() + " of " + selectedCard.GetSuit() + ".";
            else textSelectedCard.text = "Peek at the selected card to keep track of it until the next dealing.";
            buttonPeekCardObject.SetActive(!inPlayerHand && !peeked);
            buttonMarkCardObject.SetActive(inPlayerHand && !marked);
            buttonHoldCardObject.SetActive(inPlayerHand && !holdingCard);
        }
        buttonSwitchCardObject.SetActive(holdingCard);
    }

    public void PeekCard()
    {
        cardManager.PeekCard();
        UpdateView();
    }

    public void MarkCard()
    {
        cardManager.MarkCard();
        UpdateView();
    }

    public void HoldCard()
    {
        cardManager.HoldCard();
        UpdateView();
    }

    public void SwitchCard()
    {
        cardManager.SwitchCard();
        UpdateView();
    }

    public void Deal()
    {
        cardManager.Deal();
        UpdateView();
    }
}
