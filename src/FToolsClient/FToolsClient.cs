using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FToolsClient.Area;
using FToolsShared;

namespace FToolsClient
{
    public class FToolsClient : BaseScript
    {
        private Dictionary<string, MarkerEvent> markerEvents;
        private Dictionary<string, Text3D> texts;
        private Dictionary<string, AreaBase> areas;
        private List<CustomPickup> pickups;
        private bool FirstSpawn = true;


        public FToolsClient()
        {
            Tick += OnTick;
            Tick += OnTick500;
            Tick += OnTick10000;

            markerEvents = new Dictionary<string, MarkerEvent>();
            texts = new Dictionary<string, Text3D>();
            areas = new Dictionary<string, AreaBase>();
            pickups = new List<CustomPickup>();
            

            Exports.Add("CreateMarkerEvent", new Func<string, int, dynamic, dynamic, dynamic, float, bool>(
            (identifier, type, pos, scale, color, maxDistance) =>
            {
                return CreateMarkerEvent(
                        identifier,
                        (MarkerType)type,
                        new Vector3 { X = float.Parse(pos.X.ToString()), Y = float.Parse(pos.Y.ToString()), Z = float.Parse(pos.Z.ToString()) },
                        new Vector3 { X = float.Parse(scale.X.ToString()), Y = float.Parse(scale.Y.ToString()), Z = float.Parse(scale.Z.ToString()) },
                        Color.FromArgb(int.Parse(color.R.ToString()), int.Parse(color.G.ToString()), int.Parse(color.B.ToString())),
                        maxDistance
                    );
            }));

            Exports.Add("CreateMarkerEventExtended", new Func<string, int, dynamic, dynamic, dynamic, float, bool, bool, bool, int, bool>(
            (identifier, type, pos, scale, color, maxDistance, bobUpAndDown, faceCamera, rotate, accessibility) =>
            {
                return CreateMarkerEvent(
                        identifier,
                        (MarkerType) type,
                        new Vector3 { X = float.Parse(pos.X.ToString()), Y = float.Parse(pos.Y.ToString()), Z = float.Parse(pos.Z.ToString()) },
                        new Vector3 { X = float.Parse(scale.X.ToString()), Y = float.Parse(scale.Y.ToString()), Z = float.Parse(scale.Z.ToString()) },
                        Color.FromArgb(int.Parse(color.R.ToString()), int.Parse(color.G.ToString()), int.Parse(color.B.ToString())),
                        maxDistance,
                        (bool)bobUpAndDown,
                        (bool)faceCamera,
                        (bool)rotate,
                        (int)accessibility
                    );
            }));

            Exports.Add("DeleteMarkerEvent", new Action<string>(
            (identifier) =>
            {
                if (identifier != null && markerEvents.ContainsKey(identifier))
                    markerEvents.Remove(identifier);

            }));
            
            Exports.Add("AddTextToMarkerEvent", new Func<string, string, int, dynamic, dynamic, dynamic, float, bool>(
            (markerEventId, text, font, textColor, textScale, textPos, maxDistance) =>
            {
                return AddTextToMarkerEvent(
                        markerEventId,
                        new Text3D {
                            TextString = text,
                            Font = (CitizenFX.Core.UI.Font)font,
                            Color = Color.FromArgb(int.Parse(textColor.R.ToString()), int.Parse(textColor.G.ToString()), int.Parse(textColor.B.ToString())),
                            Scale = new Vector2 { X = float.Parse(textScale.X.ToString()), Y = float.Parse(textScale.Y.ToString()) },
                            Pos = new Vector3 { X = float.Parse(textPos.X.ToString()), Y = float.Parse(textPos.Y.ToString()), Z = float.Parse(textPos.Z.ToString()) },
                            MaxDistance = maxDistance
                        }
                    );
            }));

            Exports.Add("AddActionToMarkerEvent", new Func<string, int, int?, string, dynamic, dynamic, bool>(
            (markerEventId, eventActionType, control, helpText, callBack, parameters) =>
            {
                EventAction action = new EventAction
                {
                    Type = (EventActionType)eventActionType,
                    Callback = callBack
                };

                if (!String.IsNullOrEmpty(helpText))
                {
                    action.HelpText = helpText;
                }

                if (control != null)
                {
                    action.Control = (Control)control;
                }
                
                if (parameters != null)
                {
                    action.Params = parameters;
                }

                return AddActionToMarkerEvent(
                    markerEventId,
                    action
                );               
            }));

            
            Exports.Add("CreateText3D", new Func<string, string, int, dynamic, dynamic, dynamic, float, bool>(
            (identifier, text, font, textColor, textScale, textPos, maxDistance) =>
            {
                return CreateText3D(
                        identifier,
                        text,
                        (CitizenFX.Core.UI.Font)font,
                        Color.FromArgb(int.Parse(textColor.R.ToString()), int.Parse(textColor.G.ToString()), int.Parse(textColor.B.ToString())),
                        new Vector2 { X = float.Parse(textScale.X.ToString()), Y = float.Parse(textScale.Y.ToString()) },
                        new Vector3 { X = float.Parse(textPos.X.ToString()), Y = float.Parse(textPos.Y.ToString()), Z = float.Parse(textPos.Z.ToString()) },
                        maxDistance
                    );
            }));

            Exports.Add("DeleteText3d", new Action<string>(
            (identifier) =>
            {
                if (identifier != null && texts.ContainsKey(identifier))
                    texts.Remove(identifier);
            }));

            Exports.Add("CreateArea", new Func<string, int, dynamic, dynamic, dynamic, dynamic, bool, bool>(
            (identifier, type, data, onEnter, onExit, parameters, debug) =>
            {
                return CreateArea(
                        identifier,
                        (AreaType)type,
                        data,
                        onEnter,
                        onExit,
                        parameters,
                        debug
                    );
            }));

            Exports.Add("IsEntityInArea", new Func<string, int, bool>(
            (identifier, handle) =>
            {
                if (identifier != null && areas.ContainsKey(identifier))
                {
                    Entity entity = Entity.FromHandle(handle);
                    AreaBase area = areas[identifier];
                    return area.CoordsInside(entity.Position);
                }
                return false;
            }));

            Exports.Add("IsCoordsInArea", new Func<string, float, float, float, bool>(
            (identifier, x, y, z) =>
            {
                if (identifier != null && areas.ContainsKey(identifier))
                {
                    AreaBase area = areas[identifier];
                    return area.CoordsInside(new Vector3 { X = x, Y = y, Z = z });
                }
                return false;
            }));

            Exports.Add("GetFirstAreaEntityIsIn", new Func<int, string>(
            (handle) =>
            {
                Entity entity = Entity.FromHandle(handle);
                
                foreach(KeyValuePair<string, AreaBase> pair in areas)
                {
                    if (pair.Value.CoordsInside(entity.Position))
                    {
                        return pair.Key;
                    }
                }

                return null;
            }));

            Exports.Add("GetFirstAreaCoordsIsIn", new Func<float, float, float, string>(
            (x, y, z) =>
            {
                Vector3 coords = new Vector3 { X = x, Y = y, Z = z };

                foreach (KeyValuePair<string, AreaBase> pair in areas)
                {
                    if (pair.Value.CoordsInside(coords))
                    {
                        return pair.Key;
                    }
                }

                return null;
            }));

            Exports.Add("GetAreasEntityIsIn", new Func<int, object[]>(
            (handle) =>
            {
                Entity entity = Entity.FromHandle(handle);
                List<string> result = new List<string>();

                foreach (KeyValuePair<string, AreaBase> pair in areas)
                {
                    if (pair.Value.CoordsInside(entity.Position))
                    {
                        result.Add(pair.Key);
                    }
                }

                return result.ToArray();
            }));

            Exports.Add("GetAreasCoordsIsIn", new Func<float, float, float, object[]>(
            (x, y, z) =>
            {
                Vector3 coords = new Vector3 { X = x, Y = y, Z = z };
                List<string> result = new List<string>();

                foreach (KeyValuePair<string, AreaBase> pair in areas)
                {
                    if (pair.Value.CoordsInside(coords))
                    {
                        result.Add(pair.Key);
                    }
                }

                return result.ToArray();
            }));

            Exports.Add("DeleteArea", new Action<string>(
            (identifier) =>
            {
                if (identifier != null && areas.ContainsKey(identifier))
                    areas.Remove(identifier);
            }));

            Exports.Add("CreatePickup", new Func<dynamic, string, bool, bool, bool, int, int?, string, string, dynamic, int>(
            (position, model, isDynamic, onGround, deleteOnAction, eventActionType, control, helpText, callBack, parameters) =>
            {
                EventAction action = new EventAction
                {
                    Type = (EventActionType)eventActionType,
                    Callback = callBack
                };

                if (!String.IsNullOrEmpty(helpText))
                {
                    action.HelpText = helpText;
                }

                if (control != null)
                {
                    action.Control = (Control)control;
                }

                if (parameters != null)
                {
                    action.Params = parameters;
                }

                return CreatePickup(
                    new Vector3 { X = float.Parse(position.X.ToString()), Y = float.Parse(position.Y.ToString()), Z = float.Parse(position.Z.ToString()) },
                    new Model(model),
                    isDynamic,
                    onGround,
                    deleteOnAction,
                    action
                );
            }));

            Exports.Add("DeletePickup", new Action<int>(
            (NetHandle) =>
            {
                CustomPickup pickup = pickups.SingleOrDefault(p => p.NetHandle == NetHandle);

                if (pickup != null)
                {
                    pickup.Delete();
                }
            }));

            Exports.Add("ShowNotification", new Action<string>(
            (text) =>
            {
                ShowNotification(text);
            }));
            

            EventHandlers["getMapDirectives"] += new Action<dynamic>(this.GetMapDirective);
            EventHandlers["FTools:PickupCreated"] += new Action<dynamic>(this.CreateNetPickup);
            EventHandlers["FTools:PickupDeleted"] += new Action<int>(this.DeleteNetPickup);
            EventHandlers["FTools:PickupTriggered"] += new Action<int>(this.PickupTriggered);
            EventHandlers["playerSpawned"] += new Action<dynamic>(this.PlayerSpawned);            
        }

        private async Task OnTick()
        {
            foreach(MarkerEvent marker in markerEvents.Values.ToList())
            {
                marker.Draw();
                if (marker.EventAction != null && marker.EventAction.Type == EventActionType.PressControl)
                {
                    marker.Check();
                }
            }

            foreach (Text3D text in texts.Values.ToList())
            {
                text.Draw();
            }

            foreach (AreaBase area in areas.Values.ToList())
            {
                if (area.Debug)
                    area.Draw();
            }

            foreach (CustomPickup pick in pickups.ToList())
            {
                if (pick.EventAction != null && pick.EventAction.Type == EventActionType.PressControl)
                {
                    if (pick.Check())
                    {
                        break;
                    }
                }
            }
        }

        private async Task OnTick500()
        {
            await Delay(500);
            foreach (MarkerEvent marker in markerEvents.Values.ToList())
            {
                if (marker.EventAction == null || marker.EventAction.Type != EventActionType.PressControl)
                {
                    marker.Check();
                }
            }

            foreach (AreaBase area in areas.Values.ToList())
            {
                area.Check();
            }

            foreach (CustomPickup pick in pickups.ToList())
            {
                if (pick.EventAction == null || pick.EventAction.Type != EventActionType.PressControl)
                {
                    if (pick.Check())
                    {
                        break;
                    }
                }
            }
        }

        private async Task OnTick10000()
        {
            await Delay(10000);

            pickups.RemoveAll(pick => pick.Deleted || (pick.Created && !pick.Exist));
        }

        private bool CreateText3D(string identifier, string text, CitizenFX.Core.UI.Font font, Color color, Vector2 scale, Vector3 pos, float maxDistance)
        {
            try
            {
                if (identifier == null || texts.ContainsKey(identifier))
                    return false;

                texts.Add(identifier, new Text3D {
                    TextString = text,
                    Font = font,
                    Color = color,
                    Scale = scale,
                    Pos = pos,
                    MaxDistance = maxDistance
                });
                return true;
            }
            catch (Exception Ex)
            {
                Debug.WriteLine("Error: " + Ex);
                return false;
            }
        }

        private bool CreateMarkerEvent(string identifier, MarkerType type, Vector3 pos, Vector3 scale, Color color, float maxDistance, bool bobUpAndDown = false, bool faceCamera = false, bool rotate = false, int accessibility = 0)
        {
            try
            {
                if (identifier == null || markerEvents.ContainsKey(identifier))
                    return false;

                markerEvents.Add(identifier, new MarkerEvent {
                    Identifier = identifier,
                    Type = type,
                    Pos = pos,
                    Scale = scale,
                    Color = color,
                    MaxDistance = maxDistance,
                    BobUpAndDown = bobUpAndDown,
                    FaceCamera = faceCamera,
                    Rotate = rotate,
                    Accessibility = accessibility
                });
                return true;
            }
            catch (Exception Ex)
            {
                Debug.WriteLine("Error: " + Ex);
                return false;
            }
        }

        private bool CreateArea(string identifier, AreaType type, dynamic data, dynamic onEnter, dynamic onExit, dynamic parameters, bool debug)
        {
            try
            {
                if (identifier == null || areas.ContainsKey(identifier))
                    return false;

                switch (type)
                {
                    case AreaType.Sphere:
                        areas.Add(identifier, new AreaSphere
                        {
                            Identifier = identifier,
                            Type = type,
                            Pos = new Vector3 { X = (float)data.Pos.X, Y = (float)data.Pos.Y, Z = (float)data.Pos.Z },
                            Radius = (float)data.Radius,
                            OnEnter = onEnter,
                            OnExit = onExit,
                            Params = parameters,
                            Debug = debug
                        });                       
                        return true;
                    case AreaType.Circle:
                        areas.Add(identifier, new AreaCircle
                        {
                            Identifier = identifier,
                            Type = type,
                            Pos = new Vector2 { X = (float)data.Pos.X, Y = (float)data.Pos.Y },
                            Radius = (float)data.Radius,
                            OnEnter = onEnter,
                            OnExit = onExit,
                            Params = parameters,
                            Debug = debug
                        });
                        return true;
                    case AreaType.Box:
                        areas.Add(identifier, new AreaBox
                        {
                            Identifier = identifier,
                            Type = type,
                            Pos1 = new Vector3 { X = (float)data.Pos1.X, Y = (float)data.Pos1.Y, Z = (float)data.Pos1.Z },
                            Pos2 = new Vector3 { X = (float)data.Pos2.X, Y = (float)data.Pos2.Y, Z = (float)data.Pos2.Z },
                            Angle = (float)Math.PI * (float)data.Angle / 180.0f,
                            OnEnter = onEnter,
                            OnExit = onExit,
                            Params = parameters,
                            Debug = debug
                        });
                        return true;
                    case AreaType.Rectangle:
                        areas.Add(identifier, new AreaRectangle
                        {
                            Identifier = identifier,
                            Type = type,
                            Pos1 = new Vector2 { X = (float)data.Pos1.X, Y = (float)data.Pos1.Y },
                            Pos2 = new Vector2 { X = (float)data.Pos2.X, Y = (float)data.Pos2.Y },
                            Angle = (float)Math.PI * (float)data.Angle / 180.0f,
                            OnEnter = onEnter,
                            OnExit = onExit,
                            Params = parameters,
                            Debug = debug
                        });
                        return true;
                    case AreaType.Custom:
                        List<dynamic> dynPoints = data.Points;
                        List<Vector2> listPoints = new List<Vector2>();

                        foreach (dynamic p in dynPoints)
                        {
                            listPoints.Add(new Vector2 { X = (float)p.X, Y = (float)p.Y });
                        }

                        areas.Add(identifier, new AreaCustom
                        {
                            Identifier = identifier,
                            Type = type,
                            Points = listPoints.ToArray(),
                            OnEnter = onEnter,
                            OnExit = onExit,
                            Params = parameters,
                            Debug = debug
                        });
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception Ex)
            {
                Debug.WriteLine("Error: " + Ex);
                return false;
            }
        }

        private int CreatePickup(Vector3 pos, Model model, bool isDynamic, bool onGround, bool deleteOnAction, EventAction action)
        {
            try
            {
                CustomPickup pickup = new CustomPickup
                {
                    Pos = pos,
                    Model = model,
                    Dynamic = isDynamic,
                    OnGround = onGround,
                    DeleteOnAction = deleteOnAction,
                    EventAction = action
                };
                pickup.Create();
                pickups.Add(pickup);
                return pickup.NetHandle;
            }
            catch (Exception Ex)
            {
                Debug.WriteLine("Error: " + Ex);
                return -1;
            }
        }

        private bool AddTextToMarkerEvent(string markerEventId, Text3D text)
        {
            try
            {
                if (markerEventId != null && markerEvents.ContainsKey(markerEventId))
                {
                    markerEvents[markerEventId].Text3D = text;
                    return true;
                }
                
                return false;
            }
            catch (Exception Ex)
            {
                Debug.WriteLine("Error: " + Ex);
                return false;
            }
        }
        
        private bool AddActionToMarkerEvent(string markerEventId, EventAction action)
        {
            try
            {
                if (markerEventId != null && markerEvents.ContainsKey(markerEventId))
                {
                    markerEvents[markerEventId].EventAction = action;
                    return true;
                }

                return false;
            }
            catch (Exception Ex)
            {
                Debug.WriteLine("Error: " + Ex);
                return false;
            }
        }

        public void GetMapDirective(dynamic addCb)
        {
            addCb("createMarkerEvent", new Func<dynamic, dynamic, Action<dynamic>>((state, arg) =>
            {
                return new Action<dynamic>(arg2 =>
                {
                    bool created = CreateMarkerEvent(
                        arg,
                        (MarkerType)arg2.Type,
                        new Vector3 { X = (float)arg2.Pos.X, Y = (float)arg2.Pos.Y, Z = (float)arg2.Pos.Z },
                        new Vector3 { X = (float)arg2.Scale.X, Y = (float)arg2.Scale.Y, Z = (float)arg2.Scale.Z },
                        Color.FromArgb(arg2.Color.R, arg2.Color.G, arg2.Color.B),
                        (float)arg2.MaxDistance,
                        IsPropertyExist(arg2, "BobUpAndDown") ? (bool)arg2.BobUpAndDown : false,
                        IsPropertyExist(arg2, "FaceCamera") ? (bool)arg2.FaceCamera : false,
                        IsPropertyExist(arg2, "Rotate") ? (bool)arg2.Rotate : false,
                        IsPropertyExist(arg2, "Accessibility") ? (int)arg2.Accessibility : 0
                    );

                    if (created)
                    {
                        state.add("markerId", arg);
                    }               
                });
            }), new Action<dynamic>(state =>
            {
                string markerId = state.markerId;

                if (markerId != null && markerEvents.ContainsKey(markerId))
                {
                    markerEvents.Remove(markerId);
                }
            }));
            
            addCb("addTextToMarkerEvent", new Func<dynamic, dynamic, Action<dynamic>>((state, arg) =>
            {
                return new Action<dynamic>(arg2 =>
                {

                    Text3D text = new Text3D {
                        TextString = arg2.Text,
                        Font = (CitizenFX.Core.UI.Font)arg2.Font,
                        Color = Color.FromArgb(arg2.Color.R, arg2.Color.G, arg2.Color.B),
                        Scale = new Vector2 { X = (float)arg2.Scale.X, Y = (float)arg2.Scale.Y },
                        Pos = new Vector3 { X = (float)arg2.Pos.X, Y = (float)arg2.Pos.Y, Z = (float)arg2.Pos.Z },
                        MaxDistance = (float)arg2.MaxDistance
                    };


                    bool done = AddTextToMarkerEvent(
                        arg,
                        text
                    );

                    if (done)
                    {
                        state.add("markerId", arg);
                    }
                });
            }), new Action<dynamic>(state =>
            {
                string markerId = state.markerId;

                if (markerId != null && markerEvents.ContainsKey(markerId))
                {
                    markerEvents[markerId].Text3D = null;
                }
            }));
            
            addCb("addActionToMarkerEvent", new Func<dynamic, dynamic, Action<dynamic>>((state, arg) =>
            {
                return new Action<dynamic>(arg2 =>
                {

                    EventAction action = new EventAction {
                        Type = (EventActionType)arg2.Action,
                        Callback = arg2.CallBack
                    };

                    if (IsPropertyExist(arg2, "HelpText"))
                    {
                        action.HelpText = arg2.HelpText;
                    }

                    if (IsPropertyExist(arg2, "Control"))
                    {
                        action.Control = (Control)arg2.Control;
                    }

                    if (IsPropertyExist(arg2, "Params"))
                    {
                        action.Params = arg2.Params;
                    }

                    bool done = AddActionToMarkerEvent(
                        arg,
                        action
                    );

                    if (done)
                    {
                        state.add("markerId", arg);
                    }
                });
            }), new Action<dynamic>(state =>
            {
                string markerId = state.markerId;

                if (markerId != null && markerEvents.ContainsKey(markerId))
                {
                    markerEvents[markerId].EventAction = null;
                }
            }));

            addCb("create3DText", new Func<dynamic, dynamic, Action<dynamic>>((state, arg) =>
            {
                return new Action<dynamic>(arg2 =>
                {
                    bool done = CreateText3D(
                        arg,
                        arg2.Text,
                        (CitizenFX.Core.UI.Font)arg2.Font,
                        Color.FromArgb(arg2.Color.R, arg2.Color.G, arg2.Color.B),
                        new Vector2 { X = (float)arg2.Scale.X, Y = (float)arg2.Scale.Y },
                        new Vector3 { X = (float)arg2.Pos.X, Y = (float)arg2.Pos.Y, Z = (float)arg2.Pos.Z },
                        (float)arg2.MaxDistance
                    );

                    if (done)
                    {
                        state.add("textId", arg);
                    }
                });
            }), new Action<dynamic>(state =>
            {
                string textId = state.textId;

                if (textId != null && texts.ContainsKey(textId))
                {
                    texts.Remove(textId);
                }
            }));

            addCb("createArea", new Func<dynamic, dynamic, Action<dynamic>>((state, arg) =>
            {
                return new Action<dynamic>(arg2 =>
                {
                    bool done = CreateArea(
                        arg,
                        (AreaType)arg2.Type,
                        arg2.Data,
                        IsPropertyExist(arg2, "OnEnter") ? arg2.OnEnter : null,
                        IsPropertyExist(arg2, "OnExit") ? arg2.OnExit : null,
                        IsPropertyExist(arg2, "Params") ? arg2.Params : null,
                        IsPropertyExist(arg2, "Debug") ? (bool)arg2.Debug : false
                    );

                    if (done)
                    {
                        state.add("areaId", arg);
                    }
                });
            }), new Action<dynamic>(state =>
            {
                string areaId = state.areaId;

                if (areaId != null && areas.ContainsKey(areaId))
                {
                    areas.Remove(areaId);
                }
            }));
        }

        public void CreateNetPickup(dynamic info)
        {
            int netHandle = (int)info.NetHandle;
            if (pickups.SingleOrDefault(p => p.Exist && p.NetHandle == netHandle) != null)
            {
                return;
            }

            CustomPickup pickup = CustomPickup.FromInfo(new CustomPickupInfo
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
            });

            if (pickup != null)
            {
                pickups.Add(pickup);
            }
        }

        public void DeleteNetPickup(int NetHandle)
        {
            pickups.RemoveAll(p => p.NetHandle == NetHandle);
            if (API.NetworkHasControlOfNetworkId(NetHandle))
            {
                int handle = API.NetworkGetEntityFromNetworkId(NetHandle);
                API.DeleteObject(ref handle);
            }            
        }

        public void PickupTriggered(int NetHandle)
        {
            CustomPickup pickup = pickups.SingleOrDefault(p => p.Exist && p.NetHandle == NetHandle);
            if (pickup == null || pickup.EventAction == null)
            {
                return;
            }

            pickup.EventAction.TriggerAction();
        }

        public void PlayerSpawned(dynamic spawnInfo)
        {
            if (FirstSpawn)
            {
                FirstSpawn = false;
                TriggerServerEvent("FTools:GetPickups");
            }
        }

        public static void ShowNotification(string text)
        {
            API.SetNotificationTextEntry("STRING");
            API.AddTextComponentSubstringWebsite(text);
            API.DrawNotification(false, true);
        }

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
    }
}
