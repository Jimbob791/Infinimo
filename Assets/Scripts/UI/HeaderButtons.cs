using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderButtons : MonoBehaviour
{
    bool upgradesOpen = false;
    bool shopOpen = false;

    [SerializeField] DeckMenuController deckController;

    [SerializeField] Animator upgradesAnim;
    [SerializeField] Animator shopAnim;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
            CloseUpgrade();
            deckController.CloseDeck();
        }
    }

    public void UpgradesPressed()
    {
        StatManager.instance.Click();
        upgradesOpen = !upgradesOpen;
        upgradesAnim.SetBool("display", upgradesOpen);
        CloseShop();
        deckController.CloseDeck();
    }

    public void ShopPressed()
    {
        StatManager.instance.Click();
        shopOpen = !shopOpen;
        shopAnim.SetBool("display", shopOpen);
        CloseUpgrade();
        deckController.CloseDeck();
    }

    public void CloseShop()
    {
        shopOpen = false;
        shopAnim.SetBool("display", shopOpen);
    }

    public void CloseUpgrade()
    { 
        upgradesOpen = false;
        upgradesAnim.SetBool("display", upgradesOpen);
    }
}
