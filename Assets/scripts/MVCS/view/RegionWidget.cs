using System;
using UnityEngine;
using mvscs.model;

[RequireComponent (typeof(BoxCollider))]
public class RegionWidget : MonoBehaviour
{
    const string ItemsPath = "world/items/{0}/{0}";
    [SerializeField] MeshRenderer background;
    BoxCollider Collider;

    void Awake ()
    {
        Collider = gameObject.GetComponent<BoxCollider> ();
    }

    public void Init (int _cellSize, int _regionSize)
    {
        var colliderSize = _cellSize / 100f * _regionSize;
        Collider.size = new Vector3 (colliderSize, colliderSize);
        Collider.center = new Vector3 (colliderSize / 2, colliderSize / 2);

        var bg = background.gameObject.transform;
        bg.localScale = Collider.size;
        bg.position = new Vector3 (colliderSize / 2, colliderSize / 2) + new Vector3 (0, 0, 1);
        background.material.mainTextureScale = new Vector2 (_regionSize, _regionSize);
    }

    public void Draw (MapItem _item, int _cellSize)
    {
        var itemId = string.Format (ItemsPath, _item.Def.Name);
        var item = GameUtils.InstantiateAt (itemId, gameObject);
        var cellMultiplier = _cellSize / 100f;
        var pos = new Vector3 (_item.Position.X * cellMultiplier, _item.Position.Y * cellMultiplier);
        item.transform.position = pos;
    }
}