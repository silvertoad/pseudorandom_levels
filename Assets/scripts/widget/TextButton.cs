using System;
using UnityEngine;

[ExecuteInEditMode]
public class TextButton : MonoBehaviour
{
    public UIButton button;
    public UILabel label;

    public string Text {
        get {
            return label.text;
        }
        set {
            label.text = value;
        }
    }
}