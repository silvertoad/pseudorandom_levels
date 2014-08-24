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
    public Point<int> playerPos;
    List<RegionWidget> currentRegions = new List<RegionWidget> ();

    public Signal<Point<int>> onRegionPositionChange = new Signal<Point<int>> ();
    public Signal<Point<int>> onPlayerPositionChange = new Signal<Point<int>> ();

    void Update ()
    {
        var actualPosition = GetCurrentRegionPosition ();
        if (currentPosition != actualPosition) {
            onRegionPositionChange.Dispatch (actualPosition);
        }
        var actualPlayerPos = GetPlayerPosition ();
        if (playerPos != actualPlayerPos)
            onPlayerPositionChange.Dispatch (actualPlayerPos);
    }

    Point<int> GetCurrentRegionPosition ()
    {
        var regionRealSize = defs.CellSize * defs.RegionSize / 100f;
        var x = (player.transform.position.x) / regionRealSize;
        var y = (player.transform.position.y) / regionRealSize;
        x += x < 0 ? -1 : 0;
        y += y < 0 ? -1 : 0;
        return new Point<int> ((int)x, (int)y);
    }

    Point<int> GetPlayerPosition ()
    {
        var regionRealSize = defs.CellSize * defs.RegionSize / 100f;
        var x = (player.transform.position.x) / regionRealSize;
        var y = (player.transform.position.y) / regionRealSize;
        x = (player.transform.position.x - (int)x * regionRealSize) / (defs.CellSize / 100f);
        y = (player.transform.position.y - (int)y * regionRealSize) / (defs.CellSize / 100f);
        return new Point<int> (Math.Abs ((int)x), Math.Abs ((int)y));
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
        playerPos = _playerPos;
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
        var regionView = GetRegion (_region.Position);
        regionView.SetRegion (_region);

        var regionRealSize = defs.CellSize * defs.RegionSize / 100f;
        var pos = new Vector3 (_region.Position.X * regionRealSize, _region.Position.Y * regionRealSize);
        regionView.gameObject.transform.position = pos;
        currentRegions.Add (regionView);
    }

    public void UpdateRegion (RegionModel _region)
    {
        foreach (var regionView in currentRegions)
            if (regionView.Region.Position == _region.Position) {
                regionView.Draw (_region.MapItems.Last ());
            }
    }

    RegionWidget GetRegion (Point<int> _point)
    {
        var instance = GameUtils.InstantiateAt ("world/region/region", gameObject);
        instance.name = string.Format ("region_{0}:{1}", _point.X, _point.Y);
        var regionWidget = instance.GetComponent<RegionWidget> ();
        regionWidget.Init (defs.CellSize, defs.RegionSize);
        return regionWidget;
    }
}