using UnityEngine;
using strange.extensions.mediation.impl;
using System.Collections.Generic;
using mvscs.model;

public class MapView : View
{
    [SerializeField] List<GameObject> Regions;

    [Inject]
    public GameDefs defs { get; set; }

    public void DrawRegions (RegionModel[] _regions)
    {
        //foreach (var region in _regions)
        DrawRegion (_regions [0]);
    }

    public void InitPlayer (Point<int> _playerPos)
    {
        var player = GameUtils.InstantiateAt ("world/tank/tank", gameObject);
        var realCellSize = defs.CellSize / 100;
        var realPos = new Vector3 (_playerPos.X * realCellSize + realCellSize / 2, _playerPos.Y * realCellSize + realCellSize / 2);
        player.transform.position = realPos;
    }

    void DrawRegion (RegionModel _region)
    {
        var regionView = GetRegion ();
        foreach (var item in _region.MapItems)
            regionView.Draw (item, defs.CellSize);

        var pos = new Vector3 (defs.CellSize * defs.RegionSize / -2f / 100f, defs.CellSize * defs.RegionSize / -2f / 100f);
        regionView.gameObject.transform.position = pos;
    }

    RegionWidget GetRegion ()
    {
        var instance = GameUtils.InstantiateAt ("world/region/region", gameObject);
        var regionWidget = instance.GetComponent<RegionWidget> ();
        regionWidget.Init (defs.CellSize, defs.RegionSize);
        return regionWidget;
    }
}