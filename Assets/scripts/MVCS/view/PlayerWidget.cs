using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerWidget : MonoBehaviour
{
    const float RotationDelta = 5f;
    const float MoveDelta = 0.025f;

    delegate void KeyHandler (float _axis);

    List<KeyValuePair<KeyCode, KeyHandler>> keyMap = new List<KeyValuePair<KeyCode, KeyHandler>> ();
    Camera camera;

    void Awake ()
    {
        camera = GameObject.Find ("WorldCamera").GetComponent<Camera> ();

        MapKey (MoveForward, KeyCode.W);
        MapKey (MoveBackward, KeyCode.S);
        MapKey (RotateLeft, KeyCode.A);
        MapKey (RotateRight, KeyCode.D);
    }

    void Update ()
    {
        Vector3 pos = transform.position;
        pos.z = camera.transform.position.z;
        camera.transform.position = pos;
    }

    void FixedUpdate ()
    {
        foreach (var kvp in keyMap) {
            if (Input.GetKey (kvp.Key))
                kvp.Value (Input.GetAxis ("Horizontal"));
        }
    }

    void MoveForward (float _axis)
    {
        transform.Translate (MoveDelta, 0f, 0f);
    }

    void MoveBackward (float _axis)
    {
        transform.Translate (-MoveDelta, 0f, 0f);
    }

    void RotateLeft (float _axis)
    {
        transform.Rotate (Vector3.forward * RotationDelta);
    }

    void RotateRight (float _axis)
    {
        transform.Rotate (Vector3.forward * -RotationDelta);
    }

    void MapKey (KeyHandler _handler, KeyCode _keyCode)
    {
        keyMap.Add (new KeyValuePair<KeyCode, KeyHandler> (_keyCode, _handler));
    }
}