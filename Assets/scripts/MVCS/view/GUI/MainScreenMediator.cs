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
            view.settingsBtn.onClick += showMenu.Dispatch;
        }

        public override void OnRemove ()
        {
            view.settingsBtn.RemoveClickHandlers();
        }
    }
}