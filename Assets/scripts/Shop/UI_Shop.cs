using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(true);
    }

    private void Start()
    {
        CreateItemButton(Item.ItemType.Boots, Item.GetSprite(Item.ItemType.Boots), "Boots", Item.GetCost(Item.ItemType.Boots), 0);
        CreateItemButton(Item.ItemType.HealthPotion, Item.GetSprite(Item.ItemType.HealthPotion), "Health Potion", Item.GetCost(Item.ItemType.HealthPotion), 1);
        CreateItemButton(Item.ItemType.MagicMushroom, Item.GetSprite(Item.ItemType.MagicMushroom), "Magic Mushroom!!", Item.GetCost(Item.ItemType.MagicMushroom), 2); //creating UI buttons

        Hide();

    }
    private void CreateItemButton(Item.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 70f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //Being able to press the button and get reaction
            TryBuyItem(itemType);
        };
    }

    private void TryBuyItem(Item.ItemType itemType)
    {
        if (shopCustomer.TrySpendGoldAmount(Item.GetCost(itemType))) // Checks if you have the balance to buy item
        {      
            
            shopCustomer.BoughtItem(itemType);
        }
    }

    public void Show(IShopCustomer shopCustomer)
    {
        this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
