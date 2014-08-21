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
        var instance = GameUtils.InstantiateAt ("world/region", gameObject);
        return instance.GetComponent<RegionWidget> ();
    }
}