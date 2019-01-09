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


            Exports.Add("addMarkerEvent", new Action<int, dynamic, dynamic, dynamic, string, int, dynamic, dynamic, dynamic, float, int, int?, string, CallbackDelegate>(
            (type, pos, scale, color, text, font, textColor, textScale, textPos, maxDistance, eventActionType, control, helpText, callBack) =>
            {
                //addMarkerEvent(
                //    (MarkerType)type,
                //    new Vector3 { X = float.Parse(pos.X), Y = float.Parse(pos.Y), Z = float.Parse(pos.Z) },
                //    new Vector3 { X = float.Parse(scale.X), Y = float.Parse(scale.Y), Z = float.Parse(scale.Z) },
                //    new Color(int.Parse(color.R), int.Parse(color.G), int.Parse(color.B)),
                //    new Text3D(text, font, new Color(int.Parse(textColor.R), int.Parse(textColor.G), int.Parse(textColor.B)), new Vector2 { X = float.Parse(textScale.X), Y = float.Parse(textScale.Y) }, new Vector3 { X = float.Parse(textPos.X), Y = float.Parse(textPos.Y), Z = float.Parse(textPos.Z) }, maxDistance),
                //    new EventAction((EventActionType)eventActionType, (Control)control, helpText, callBack)
                //);
                addMarkerEvent(
                    (MarkerType)type,
                    new Vector3 { X = float.Parse(pos.X.ToString()), Y = float.Parse(pos.Y.ToString()), Z = float.Parse(pos.Z.ToString()) },
                    new Vector3 { X = float.Parse(scale.X.ToString()), Y = float.Parse(scale.Y.ToString()), Z = float.Parse(scale.Z.ToString()) },
                    new Color(int.Parse(color.R.ToString()), int.Parse(color.G.ToString()), int.Parse(color.B.ToString())),
                    new Text3D(text, font, new Color(int.Parse(textColor.R.ToString()), int.Parse(textColor.G.ToString()), int.Parse(textColor.B.ToString())), new Vector2 { X = float.Parse(textScale.X.ToString()), Y = float.Parse(textScale.Y.ToString()) }, new Vector3 { X = float.Parse(textPos.X.ToString()), Y = float.Parse(textPos.Y.ToString()), Z = float.Parse(textPos.Z.ToString()) }, maxDistance),
                    new EventAction((EventActionType)eventActionType, (Control)control, helpText, callBack)
                );
            }));

            //Exports.Add("addMarkerEvent", new Action<float, float, float, CallbackDelegate>(
            //(x, y, z, callBack) =>
            //{
            //    addMarkerEvent(
            //        MarkerType.VerticalCylinder,
            //        new Vector3 { X = x, Y = y, Z = z },
            //        new Vector3 { X = 3.0f, Y = 3.0f, Z = 1.0f },
            //        new Color(255, 0, 0),
            //        new Text3D("Something", 0, new Color(255, 255, 255), new Vector2 { X = 0.8f, Y = 0.8f }, new Vector3 { X = x, Y = y, Z = z + 1.5f }, 40.0f),
            //        new EventAction(EventActionType.PressControl, Control.Pickup, "Press", callBack)
            //    );
            //}));
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
