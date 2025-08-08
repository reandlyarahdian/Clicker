using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("Components")]
    public TMP_Text priceText;
    public Button buyButton;
    public Image iconImage;

    [Header("Item Data")]
    public int startPrice = 10;
    public Sprite icon;

    [Header("Item Type")]
    public float globalClickMultiplier = 1f;
    public float globalCookiesPerSecond = 0f;

    [Header("Managers")]
    public GameManager gameManager;
    public QuestListener questListener;


    [HideInInspector] public bool purchased = false;
    [HideInInspector] public int level = 0;

    private void Start()
    {
        iconImage.sprite = icon;
        UpdateUI();
    }

    public void ClickBuy()
    {
        if (purchased)
            return;

        bool success = gameManager.PurchaseAction(startPrice);

        if (success)
        {
            level++;
            purchased = true;
            questListener.OnItemBuy();
            ApplyEffect();
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        priceText.text = startPrice.ToString();
        buyButton.interactable = !purchased;
        gameObject.SetActive(!purchased);
    }

    // update every effect after item bought
    public void ApplyEffect()
    {
        gameManager.ClickPerSecond(this);
    }
}
