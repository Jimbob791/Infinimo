using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfferController : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public GameObject dominoObj;

    [SerializeField] Image outline;

    public void MouseEnter()
    {
        outline.enabled = true;
    }

    public void MouseExit()
    {
        outline.enabled = false;
    }

    public void ClickOffer(GameObject pressedObj)
    {
        transform.parent.transform.parent.GetComponent<DeckShopGenerator>().AttemptBuy(pressedObj);
    }
}
