using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using FToolsShared;

namespace FToolsServer
{
    public class FToolsServer : BaseScript
    {
        private List<CustomPickupInfo> Pickups;

        public FToolsServer()
        {
            Pickups = new List<CustomPickupInfo>();

            EventHandlers["FTools:PickupCreated"] += new Action<dynamic>(this.PickupCreated);
            EventHandlers["FTools:PickupDeleted"] += new Action<int>(this.PickupDeleted);
            EventHandlers["FTools:PickupTriggered"] += new Action<Player, int>(this.PickupTriggered);
            EventHandlers["FTools:GetPickups"] += new Action<Player>(this.GetPickups);
        }

        private void PickupCreated(dynamic info)
        {
            CustomPickupInfo pickup = new CustomPickupInfo
            {
                NetHandle = (int)info.NetHandle,
                Dynamic = (bool)info.Dynamic,
                OnGround = (bool)info.OnGround,
                DeleteOnAction = (bool)info.DeleteOnAction,
                EventActionType = (int)info.EventActionType,
                Control = (int)info.Control,
                HelpText = (string)info.HelpText,
                CallBack = info.CallBack,
                Parameters = info.Parameters,
                Created = (bool)info.Created
            };
            Pickups.Add(pickup);

            TriggerClientEvent("FTools:PickupCreated", pickup);
        }

        private void PickupDeleted(int netHandle)
        {
            Pickups.RemoveAll(p => p.NetHandle == netHandle);

            TriggerClientEvent("FTools:PickupDeleted", netHandle);
        }

        private void PickupTriggered([FromSource]Player source, int netHandle)
        {
            if (Pickups.SingleOrDefault(p => p.NetHandle == netHandle) == null)
            {
                return;
            }

            Pickups.RemoveAll(p => p.NetHandle == netHandle);

            source.TriggerEvent("FTools:PickupTriggered", netHandle);
        }

        private async void GetPickups([FromSource]Player source)
        {
            await BaseScript.Delay(10000);
            foreach (CustomPickupInfo pick in Pickups)
            {
                source.TriggerEvent("FTools:PickupCreated", pick);
            }
        }
    }
}
