using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderButtons : MonoBehaviour
{
    bool upgradesOpen = false;
    bool shopOpen = false;

    [SerializeField] Animator upgradesAnim;
    [SerializeField] Animator shopAnim;

    public void UpgradesPressed()
    {
        upgradesOpen = !upgradesOpen;
        upgradesAnim.SetBool("display", upgradesOpen);
    }

    public void ShopPressed()
    {
        shopOpen = !shopOpen;
        shopAnim.SetBool("display", shopOpen);
    }
}
