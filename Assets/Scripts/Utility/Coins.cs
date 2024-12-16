using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    private static int amount;
    public TMP_Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        resetCoins();
    }

    public void addCoins(int n)
    {
        amount += n;
        updateText();
    }

    public void removeCoins(int n)
    {
        amount -= n;
        updateText();
    }

    public bool checkCoins(int n)
    {
        if (amount >= n)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void updateText()
    {
        coinText.text = "Coins: " + amount.ToString();
    }

    public void resetCoins()
    {
        amount = 0;
        updateText();
    }
}
