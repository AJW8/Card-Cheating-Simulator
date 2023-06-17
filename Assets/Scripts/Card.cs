using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Color hoverColour;
    public Color selectedColour;
    private SpriteRenderer spriteRenderer;
    private Color defaultColour;
    private int suit;
    private int value;
    private bool hover;
    private bool selected;
    private bool peeked;
    private bool marked;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColour = spriteRenderer.color;
        hover = false;
        selected = false;
        peeked = false;
        marked = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSuit(int s)
    {
        suit = s;
    }

    public void SetValue(int v)
    {
        value = v;
    }

    public void SetHover(bool h)
    {
        spriteRenderer.color = selected ? selectedColour : h ? hoverColour : defaultColour;
        hover = h;
    }

    public void SetSelected(bool s)
    {
        spriteRenderer.color = s ? selectedColour : hover ? hoverColour : defaultColour;
        selected = s;
    }

    public void SetPeeked(bool p)
    {
        peeked = p;
    }

    public void Mark()
    {
        marked = true;
    }

    public string GetSuit()
    {
        return (new string[] { "Clubs", "Diamonds", "Spades", "Hearts" })[suit];
    }

    public string GetValue()
    {
        return (new string[] { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" })[value];
    }

    public bool GetPeeked()
    {
        return peeked;
    }

    public bool GetMarked()
    {
        return marked;
    }
}
