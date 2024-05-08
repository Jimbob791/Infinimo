using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckShopGenerator : MonoBehaviour
{
    [SerializeField] GameObject offerPrefab;
    [SerializeField] GameObject offerParent;
    [SerializeField] TextMeshProUGUI reloadCostText;

    List<ShopOffer> offers = new List<ShopOffer>();
    List<GameObject> offerObjects = new List<GameObject>();

    public int lifetimePurchases = 0;
    public int lifetimeReloads = 0;

    [SerializeField] PrestigeUpgrade boneyardDiscount;

    bool generating;
    ShopOffer pressedOffer;

    private void Start()
    {
        GenerateNewOffers(true);
        lifetimeReloads = 0;
    }

    void Update()
    {
        for (int i = 0; i < offerObjects.Count; i++)
        {
            if (offerObjects[i] != null)
            {
                offers[i].cost = GetOfferCost(offers[i]);
                offerObjects[i].GetComponent<OfferController>().costText.text = DominoScore.instance.FormatLargeNumber(offers[i].cost);
            }
        }
    }

    public void AttemptBuy(GameObject pressedObj)
    {
        int index = 0;

        for (int i = 0; i < offerObjects.Count; i++)
        {
            if (pressedObj == offerObjects[i])
            {
                pressedOffer = offers[i];
                index = i;
                break;
            }
        }

        if (DominoScore.instance.score < System.Math.Floor(GetOfferCost(pressedOffer)))
            return;

        DominoScore.instance.score -= System.Math.Floor(GetOfferCost(pressedOffer));

        lifetimePurchases += 1;
        pressedObj.GetComponent<Animator>().SetBool("buy", true);

        DominoManager.instance.AddDomino(pressedOffer.leftNum, pressedOffer.rightNum);

        StatManager.instance.SaveData();

        Destroy(pressedObj, 5f);
    }

    public void GenerateNewOffers(bool ignoreCost)
    {        
        if (generating)
            return;

        if (!ignoreCost)
        {
            double generateCost = System.Math.Floor(Mathf.Pow(1.8f, lifetimeReloads) * 81);

            if (DominoScore.instance.score < System.Math.Floor(Mathf.Pow(1.8f, lifetimeReloads) * 81))
                return;

            DominoScore.instance.score -= System.Math.Floor(Mathf.Pow(1.8f, lifetimeReloads) * 81);
        }

        if (!ignoreCost)
            lifetimeReloads += 1;
        
        reloadCostText.text = DominoScore.instance.FormatLargeNumber(System.Math.Floor(Mathf.Pow(1.8f, lifetimeReloads) * 81));

        offers = new List<ShopOffer>();
        for (int i = offerObjects.Count - 1; i >= 0; i--)
        {
            Destroy(offerObjects[i]);
            offerObjects.RemoveAt(i);
        }

        generating = true;
        StartCoroutine(GenerateOffersDelay());
    }

    IEnumerator GenerateOffersDelay()
    {
        for (int i = 0; i < 6; i++)
        {
            ShopOffer newOffer = new ShopOffer();

            newOffer.leftNum = Mathf.FloorToInt(Mathf.Pow(Random.Range(0, 100), 2) / 1000);
            newOffer.rightNum = Mathf.FloorToInt(Mathf.Pow(Random.Range(0, 100), 2) / 1000);

            newOffer.cost = GetOfferCost(newOffer);
            if (newOffer.leftNum == newOffer.rightNum)
            {
                newOffer.cost = newOffer.cost * 2;
            }

            newOffer.cost = newOffer.cost * (Random.Range(90f, 110f) / 100f);
            if (newOffer.cost < 1)
            {
                newOffer.cost = 1;
            }

            offers.Add(newOffer);

            GameObject newObj = Instantiate(offerPrefab, new Vector3(-240 + (240 * (i % 3)), 100 - (200 * (Mathf.FloorToInt(i / 3))), 0), Quaternion.identity);
            newObj.transform.SetParent(offerParent.transform, false);
            
            OfferController newController = newObj.GetComponent<OfferController>();
            newController.dominoObj.GetComponent<DominoUIRenderer>().leftNum = newOffer.leftNum;
            newController.dominoObj.GetComponent<DominoUIRenderer>().rightNum = newOffer.rightNum;

            offerObjects.Add(newObj);

            yield return new WaitForSeconds(0.15f);
        }

        generating = false;
    }

    private double GetOfferCost(ShopOffer offer)
    {
        double cost = (System.Math.Pow(1.8f, lifetimePurchases + 1) * (offer.leftNum + 1) * (offer.rightNum + 1) / 4) * (10f / (boneyardDiscount.level + 10f));
        return System.Math.Ceiling(cost);
    }
}

public class ShopOffer
{
    public int leftNum;
    public int rightNum;

    public double cost;
}