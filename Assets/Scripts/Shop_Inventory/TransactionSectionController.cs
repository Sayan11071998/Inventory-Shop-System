using UnityEngine;
using TMPro;
using System;

public class TransactionSectionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI priceText;

    // Delegates that the parent view/controller sets up.
    public Func<int> GetAvailableQuantity;
    public Func<int> GetUnitPrice;
    public Action PlayQuantityChangedSound;
    public Action PlayNonClickableSound;

    // Resets the section to zero.
    public void ResetSection()
    {
        quantityText.text = "0";
        priceText.text = "0";
    }

    // Adds one unit if available.
    public void AddValue()
    {
        int availableQuantity = GetAvailableQuantity?.Invoke() ?? 0;
        int currentQuantity = int.Parse(quantityText.text);
        int currentPrice = int.Parse(priceText.text);
        int unitPrice = GetUnitPrice?.Invoke() ?? 0;

        if (currentQuantity < availableQuantity)
        {
            PlayQuantityChangedSound?.Invoke();
            quantityText.text = (currentQuantity + 1).ToString();
            priceText.text = (currentPrice + unitPrice).ToString();
        }
        else
        {
            PlayNonClickableSound?.Invoke();
        }
    }

    // Reduces one unit if above zero.
    public void ReduceValue()
    {
        int currentQuantity = int.Parse(quantityText.text);
        int currentPrice = int.Parse(priceText.text);
        int unitPrice = GetUnitPrice?.Invoke() ?? 0;

        if (currentQuantity > 0)
        {
            PlayQuantityChangedSound?.Invoke();
            quantityText.text = (currentQuantity - 1).ToString();
            priceText.text = (currentPrice - unitPrice).ToString();
        }
        else
        {
            PlayNonClickableSound?.Invoke();
        }
    }

    // Helper methods to get the current values.
    public string GetQuantityText() => quantityText.text;
    public string GetPriceText() => priceText.text;
}