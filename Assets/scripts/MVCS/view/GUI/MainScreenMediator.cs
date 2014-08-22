using strange.extensions.mediation.impl;
using view.gui;
using appsignal;

namespace mediator
{
    public class MainScreenMediator : Mediator
    {
        [Inject] public MainScreenView view  { set; get; }

        [Inject] public ShowMenuSignal showMenu  { set; get; }

        public override void OnRegister ()
        {
            EventDelegate.Add (view.settingsBtn.onClick, showMenu.Dispatch);
        }

        public override void OnRemove ()
        {
            EventDelegate.Remove (view.settingsBtn.onClick, showMenu.Dispatch);
        }
    }
}