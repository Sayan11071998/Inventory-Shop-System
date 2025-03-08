using System.Collections.Generic;

public abstract class BaseItemModel
{
    protected ItemDatabase itemDatabase;
    protected List<ItemProperty> items;

    public BaseItemModel(ItemDatabase _itemDatabase)
    {
        itemDatabase = _itemDatabase;
        items = new List<ItemProperty>();
    }

    public List<ItemProperty> GetItemDatabase()
    {
        if (items.Count == 0)
            items.AddRange(itemDatabase.items);
        return items;
    }
}