using UnityEngine;
using System.Linq;

public class GameUtils
{
    public static GameObject InstantiateAt (string _prefabId, GameObject _parent)
    {
        var map = Resources.Load <GameObject> (_prefabId);
        var instance = Object.Instantiate (map) as GameObject;
        instance.transform.parent = _parent.transform;
        instance.name = _prefabId.Split ('/').Last ();
        return instance;
    }
}