using System;
using UnityEngine;
using mvscs.model;

public class RegionWidget : MonoBehaviour
{
    const string ItemsPath = "world/items/{0}/{0}";

    public void Draw (MapItem _item, int _cellSize)
    {
        var itemId = string.Format (ItemsPath, _item.Def.Name);
        var item = GameUtils.InstantiateAt (itemId, gameObject);
        var cellMultiplier = _cellSize / 100f;
        var pos = new Vector3 (_item.Position.X * cellMultiplier, _item.Position.Y * cellMultiplier);
        item.transform.position = pos;
    }
}