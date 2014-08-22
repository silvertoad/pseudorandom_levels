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

    public static void AddToPath (string _path, GameObject _target)
    {
        var path = _path.Split ('.');
        var go = GameObject.Find (path [0]);
        if (go == null)
            go = new GameObject (path [0]);
        for (var i = 1; i < path.Length; i++) {
            var finded = go.transform.FindChild (path [i]);
            GameObject current;
            if (finded == null) {
                current = new GameObject (path [i]);
                current.transform.parent = go.transform;
            } else
                current = finded.gameObject;
            go = current;
        }

        _target.transform.parent = go.transform;
    }

    public static TComponent AddToPath<TComponent> (string _path) where TComponent: Component
    {
        var go = new GameObject (typeof(TComponent).Name);
        AddToPath (_path, go);
        return go.AddComponent<TComponent> ();
    }
}