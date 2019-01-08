using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FToolsClient
{
    public class FToolsClient : BaseScript
    {
        private List<MarkerEvent> markerEvents;


        public FToolsClient()
        {
            Tick += OnTick;

            markerEvents = new List<MarkerEvent>();

            addMarkerEvent(
                MarkerType.VerticalCylinder,
                new Vector3 { X = -976.93f, Y = -2996.31f, Z = 12.95f },
                new Vector3 { X = 3.0f, Y = 3.0f, Z = 1.0f },
                new Color(255, 0, 0),
                new Text3D("Something", 0, new Color(255, 255, 255), new Vector2 { X = 0.7f, Y = 0.7f }, new Vector3 { X = -976.93f, Y = -2996.31f, Z = 14.45f }),
                new EventAction(EventActionType.PressControl, Control.Pickup, "Press ~INPUT_PICKUP~ to do something")
                );
        }

        private async Task OnTick()
        {
            markerEvents.ForEach(delegate (MarkerEvent marker)
            {
                marker.Draw();
                marker.Check();
            });
        }

        private bool addMarkerEvent(MarkerType type, Vector3 pos, Vector3 scale, Color color, Text3D text3D, EventAction eventAction)
        {
            try
            {
                markerEvents.Add(new MarkerEvent(type, pos, scale, color, text3D, eventAction));
                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Error: " + Ex);
                return false;
            }
        }

        public static void ShowNotification(string text)
        {
            API.SetNotificationTextEntry("STRING");
            API.AddTextComponentSubstringWebsite(text);
            API.DrawNotification(false, true);
        }
    }
}
