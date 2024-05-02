using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckShopGenerator : MonoBehaviour
{
    [SerializeField] GameObject offerPrefab;
    [SerializeField] GameObject offerParent;

    List<ShopOffer> offers = new List<ShopOffer>();
    List<GameObject> offerObjects = new List<GameObject>();

    public int lifetimePurchases = 0;

    public void GenerateNewOffers()
    {
        offers = new List<ShopOffer>();
        for (int i = offerObjects.Count - 1; i >= 0; i--)
        {
            Destroy(offerObjects[i]);
            offerObjects.RemoveAt(i);
        }


        for (int i = 0; i < 6; i++)
        {
            ShopOffer newOffer = new ShopOffer();

            newOffer.leftNum = Mathf.FloorToInt(Mathf.Pow(Random.Range(0, 100), 2.5f) / 10000);
            newOffer.rightNum = Mathf.FloorToInt(Mathf.Pow(Random.Range(0, 100), 2.5f) / 10000);

            newOffer.cost = Mathf.Pow(1.8f, lifetimePurchases + 1) * (newOffer.leftNum + 1) * (newOffer.rightNum + 1);
            if (newOffer.leftNum == newOffer.rightNum)
            {
                newOffer.cost = newOffer.cost * 2;
            }

            newOffer.cost = newOffer.cost * (Random.Range(90f, 110f) / 100f);

            offers.Add(newOffer);

            GameObject newObj = Instantiate(offerPrefab, new Vector3(-240 + (240 * (i % 3)), 100 - (200 * (Mathf.FloorToInt(i / 3))), 0), Quaternion.identity);
            newObj.transform.SetParent(offerParent.transform, false);
            newObj.transform.GetChild(0).GetComponent<DominoUIRenderer>().leftNum = newOffer.leftNum;
            newObj.transform.GetChild(0).GetComponent<DominoUIRenderer>().rightNum = newOffer.rightNum;
            newObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = DominoScore.instance.FormatLargeNumber(newOffer.cost);

            offerObjects.Add(newObj);
        }
    }
}

public class ShopOffer
{
    public int leftNum;
    public int rightNum;

    public double cost;
}