using System;
using UnityEngine;
using mvscs.model;

public class RegionWidget : MonoBehaviour
{
    [SerializeField] MeshRenderer background;

    const string ItemsPath = "world/items/{0}/{0}";

    public RegionModel Region { get; private set; }

    int cellSize;

    public void Init (int _cellSize, int _regionSize)
    {
        cellSize = _cellSize;
        var colliderSize = _cellSize / 100f * _regionSize;

        var bg = background.gameObject.transform;
        bg.localScale = new Vector3 (colliderSize, colliderSize);
        bg.position = new Vector3 (colliderSize / 2, colliderSize / 2, 1);
        background.material.mainTextureScale = new Vector2 (_regionSize, _regionSize);
    }

    public void Draw (MapItem _item)
    {
        var itemId = string.Format (ItemsPath, _item.Def.Name);
        var item = GameUtils.InstantiateAt (itemId, gameObject);
        var cellMultiplier = cellSize / 100f;
        var pos = new Vector3 (_item.Position.X * cellMultiplier, _item.Position.Y * cellMultiplier);
        item.transform.localPosition = pos;
    }

    public void SetRegion (RegionModel _region)
    {
        Region = _region;
        foreach (var item in _region.MapItems) {
            Draw (item);
        }
    }
}