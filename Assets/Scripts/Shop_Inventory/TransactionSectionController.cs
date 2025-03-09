using UnityEngine;
using TMPro;
using System;

public class TransactionSectionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI priceText;

    public Func<int> GetAvailableQuantity;
    public Func<int> GetUnitPrice;
    public Action PlayNonClickableSound;

    public void ResetSection()
    {
        quantityText.text = "0";
        priceText.text = "0";
    }

    public void AddValue()
    {
        int availableQuantity = GetAvailableQuantity?.Invoke() ?? 0;
        int currentQuantity = int.Parse(quantityText.text);
        int currentPrice = int.Parse(priceText.text);
        int unitPrice = GetUnitPrice?.Invoke() ?? 0;

        if (currentQuantity < availableQuantity)
        {
            EventService.Instance.OnQuantityChanged?.InvokeEvent();
            quantityText.text = (currentQuantity + 1).ToString();
            priceText.text = (currentPrice + unitPrice).ToString();
        }
        else
        {
            PlayNonClickableSound?.Invoke();
        }
    }

    public void ReduceValue()
    {
        int currentQuantity = int.Parse(quantityText.text);
        int currentPrice = int.Parse(priceText.text);
        int unitPrice = GetUnitPrice?.Invoke() ?? 0;

        if (currentQuantity > 0)
        {
            EventService.Instance.OnQuantityChanged?.InvokeEvent();
            quantityText.text = (currentQuantity - 1).ToString();
            priceText.text = (currentPrice - unitPrice).ToString();
        }
        else
        {
            PlayNonClickableSound?.Invoke();
        }
    }

    public string GetQuantityText() => quantityText.text;
    public string GetPriceText() => priceText.text;
}