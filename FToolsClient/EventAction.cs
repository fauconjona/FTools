using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FToolsClient
{
    class EventAction
    {
        private EventActionType type;
        private Control control;
        private string helpText;

        public EventAction(EventActionType _type, Control _control, string _helpText)
        {
            this.type = _type;
            this.control = _control;
            this.helpText = _helpText;
        }

        public void Draw()
        {
            if (!String.IsNullOrEmpty(this.helpText))
            {
                API.SetTextComponentFormat("STRING");
                API.AddTextComponentString(this.helpText);
                API.DisplayHelpTextFromStringLabel(0, false, true, -1);
            }
        }

        public void CheckControl()
        {
            if (this.type == EventActionType.PressControl)
            {
                if (API.IsControlJustReleased(1, (int)this.control) && !API.IsPauseMenuActive())
                {
                    FToolsClient.ShowNotification("Did something");              
                }
            }
        }
    }
}
