using UnityEngine;
using strange.extensions.mediation.impl;
using System.Collections.Generic;
using mvscs.model;

public class MapView : View
{
    [SerializeField] List<GameObject> Regions;

    [Inject]
    public GameDefs defs { get; set; }

    public GameObject player;

    public Point<int> currentPosition;
    public Point<int> defaultPosition;

    void Update ()
    {
    }

    public void SetDefaultRegion (Point<int> _defaultPosition)
    {
        defaultPosition = _defaultPosition;
    }

    public void DrawRegions (RegionModel[] _regions, Point<int> _startPos)
    {
        currentPosition = _startPos;
        foreach (var region in _regions)
            DrawRegion (region);
    }

    public void InitPlayer (Point<int> _playerPos)
    {
        player = GameUtils.InstantiateAt ("world/tank/tank", gameObject);
        var realCellSize = defs.CellSize / 100;
        var realPos = new Vector3 (_playerPos.X * realCellSize + realCellSize / 2, _playerPos.Y * realCellSize + realCellSize / 2);
        player.transform.position = realPos;
    }

    void DrawRegion (RegionModel _region)
    {
        var regionView = GetRegion ();
        foreach (var item in _region.MapItems)
            regionView.Draw (item, defs.CellSize);

        var regionRealSize = defs.CellSize * defs.RegionSize / 100f;
        var middle = defs.CellSize * defs.RegionSize / -2f / 100f;
        var delta = defaultPosition - currentPosition - _region.Position;
        var pos = new Vector3 (middle - delta.X * regionRealSize, middle - delta.Y * regionRealSize);
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