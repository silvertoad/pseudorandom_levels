using UnityEngine;
using strange.extensions.mediation.impl;
using System.Collections.Generic;
using mvscs.model;
using strange.extensions.signal.impl;
using System;
using System.Linq;

public class MapView : View
{
    [SerializeField] List<GameObject> Regions;

    [Inject]
    public GameDefs defs { get; set; }

    public GameObject player;

    public Point<int> currentPosition;
    List<RegionWidget> currentRegions = new List<RegionWidget> ();

    public Signal<Point<int>> onPositionChange = new Signal<Point<int>> ();

    void Update ()
    {
        var actualPosition = GetPlayerPosition ();
        if (currentPosition != actualPosition) {
            onPositionChange.Dispatch (actualPosition);
        }
    }

    Point<int> GetPlayerPosition ()
    {
        var regionRealSize = defs.CellSize * defs.RegionSize / 100f;
        var x = (player.transform.position.x) / regionRealSize;
        var y = (player.transform.position.y) / regionRealSize;
        x += x < 0 ? -1 : 0;
        y += y < 0 ? -1 : 0;
        return new Point<int> ((int)x, (int)y);
    }

    public void SetDefaultRegion (Point<int> _defaultPosition)
    {
        currentPosition = _defaultPosition;
    }

    public void DrawRegions (RegionModel[] _regions, Point<int> _startPos)
    {
        var regionsToDraw = new List<RegionModel> ();
        foreach (var region in _regions) {
            var isUnique = true;
            foreach (var view in currentRegions) {
                if (region.Position == view.Region.Position) {
                    isUnique = false;
                    break;
                }
            }
            if (isUnique)
                regionsToDraw.Add (region);
        }

        currentPosition = _startPos;
        foreach (var region in regionsToDraw) {
            DrawRegion (region);
        }
        CropInvisible ();
    }

    void CropInvisible ()
    {
        var newRegions = currentRegions.ToArray ().ToList ();
        foreach (var regionView in currentRegions) {
            var delta = GetAbsPoint (regionView.Region.Position) - GetAbsPoint (currentPosition);
            if (Math.Abs (delta.X) >= 2 || Math.Abs (delta.Y) >= 2) {
                newRegions.Remove (regionView);
                Destroy (regionView.gameObject);
            }
        }
        currentRegions = newRegions;
    }

    Point<int> GetAbsPoint (Point<int> _pos)
    {
        return new Point<int> (Math.Abs (_pos.X), Math.Abs (_pos.Y));
    }

    public void InitPlayer (Point<int> _playerPos)
    {
        player = GameUtils.InstantiateAt ("world/tank/tank", gameObject);
        var realCellSize = defs.CellSize / 100f;

        var xPosOnRegion = _playerPos.X * realCellSize + realCellSize / 2;
        var yPosOnRegion = _playerPos.Y * realCellSize + realCellSize / 2;

        var x = xPosOnRegion + currentPosition.X * defs.RegionSize * realCellSize;
        var y = yPosOnRegion + currentPosition.Y * defs.RegionSize * realCellSize;

        player.transform.position = new Vector3 (x, y);
    }

    void DrawRegion (RegionModel _region)
    {
        var regionView = GetRegion ();
        regionView.SetRegion (_region);

        var regionRealSize = defs.CellSize * defs.RegionSize / 100f;
        var pos = new Vector3 (_region.Position.X * regionRealSize, _region.Position.Y * regionRealSize);
        regionView.gameObject.transform.position = pos;
        currentRegions.Add (regionView);
    }

    public void UpdateRegion (RegionModel _region)
    {
        foreach (var regionView in currentRegions)
            if (regionView.Region.Position == _region.Position)
                regionView.Draw (_region.MapItems.Last ());
    }

    RegionWidget GetRegion ()
    {
        var instance = GameUtils.InstantiateAt ("world/region/region", gameObject);
        var regionWidget = instance.GetComponent<RegionWidget> ();
        regionWidget.Init (defs.CellSize, defs.RegionSize);
        return regionWidget;
    }
}