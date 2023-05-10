using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerCollider : MonoBehaviour
{

    [SerializeField] private UI_Shop uiShop;
    bool insideRange=false;
    IShopCustomer range;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        range = collider.GetComponent<IShopCustomer>();
        if(range != null )
        {
            insideRange = true;
        }

    }
    private void Update()
    {
            if(insideRange && Input.GetKeyDown(KeyCode.F))
            {
                uiShop.Show(range);
            }
            //if(Input.GetKeyDown(KeyCode.U))
            //{
            //    uiShop.Hide();
            //}


    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        range = collider.GetComponent<IShopCustomer>();
        if(range != null)
        {
            uiShop.Hide();
            insideRange = false;
        }
    }

}
