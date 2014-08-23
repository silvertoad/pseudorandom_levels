using strange.extensions.mediation.impl;
using UnityEngine;

namespace view.gui
{
    public class MainMenuView: View
    {
        [SerializeField] public UIButton startBtn;
        [SerializeField] public UIButton loadBtn;
        [SerializeField] public UIButton saveBtn;
        [SerializeField] public UIButton continueBtn;

        public void SetButtonActive (UIButton _button, bool _isEnabled)
        {
            _button.isEnabled = _isEnabled;
        }
    }
}