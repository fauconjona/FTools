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
        public EventActionType Type { get; set; }
        public Control Control { get; set; }
        public string HelpText { get; set; }
        public dynamic Callback { get; set; }
        public dynamic Params { get; set; }

        public void Draw()
        {
            if (!String.IsNullOrEmpty(this.HelpText))
            {
                API.SetTextComponentFormat("STRING");
                API.AddTextComponentString(this.HelpText);
                API.DisplayHelpTextFromStringLabel(0, false, true, -1);
            }
        }

        public bool CheckControl()
        {
            if (this.Callback == null)
                return false;

            if (this.Type == EventActionType.PressControl)
            {
                if (Game.IsControlJustReleased(1, this.Control) && !Game.IsPaused)
                {
                    this.TriggerAction();
                    return true;
                }
            }
            else if (this.Type == EventActionType.Auto)
            {
                this.TriggerAction();
                return true;
            }

            return false;
        }

        public bool CheckPickupControl(int NetHandle)
        {
            if (this.Callback == null)
                return false;

            if (this.Type == EventActionType.PressControl)
            {
                if (Game.IsControlJustReleased(1, this.Control) && !Game.IsPaused)
                {
                    BaseScript.TriggerServerEvent("FTools:PickupTriggered", NetHandle);
                    return true;
                }
            }
            else if (this.Type == EventActionType.Auto)
            {
                BaseScript.TriggerServerEvent("FTools:PickupTriggered", NetHandle);
                return true;
            }

            return false;
        }

        public void TriggerAction()
        {
            

            if (this.Callback.GetType() == typeof(System.String))
            {
                BaseScript.TriggerEvent((String)this.Callback, this.Params);
            }
            else if (this.Callback.GetType() == typeof(CitizenFX.Core.CallbackDelegate))
            {
                if (this.Params != null)
                {
                    ((CallbackDelegate)this.Callback).Invoke(this.Params);
                }
                else
                {
                    ((CallbackDelegate)this.Callback)();
                }                
            }
        }
    }
}
