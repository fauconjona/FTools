using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FToolsClient
{
    class MarkerEvent
    {

        #region marker attributes

        private MarkerType type;
        private Vector3 pos;
        private Vector3 dir;
        private Vector3 rot;
        private Vector3 scale;
        private Color color;
        private bool bobUpAndDown;
        private bool faceCamera;
        private bool rotate;

        #endregion

        private Text3D text3D;

        private EventAction eventAction;

        public MarkerEvent(MarkerType _type, Vector3 _pos, Vector3 _scale, Color _color, Text3D _text3D, EventAction _eventAction)
        {
            this.type = _type;
            this.pos = _pos;
            this.scale = _scale;
            this.color = _color;
            this.dir = new Vector3 { X = 0, Y = 0, Z = 0 };
            this.rot = new Vector3 { X = 0, Y = 0, Z = 0 };
            this.bobUpAndDown = false;
            this.faceCamera = false;
            this.rotate = false;
            this.text3D = _text3D;
            this.eventAction = _eventAction;
        }

        public MarkerEvent(MarkerType _type, Vector3 _pos, Vector3 _dir, Vector3 _rot, Vector3 _scale, Color _color, Text3D _text3D, EventAction _eventAction)
        {
            this.type = _type;
            this.pos = _pos;
            this.dir = _dir;
            this.rot = _rot;
            this.scale = _scale;
            this.color = _color;
            this.bobUpAndDown = false;
            this.faceCamera = false;
            this.rotate = false;
            this.text3D = _text3D;
            this.eventAction = _eventAction;
        }

        public MarkerEvent(MarkerType _type, Vector3 _pos, Vector3 _dir, Vector3 _rot, Vector3 _scale, Color _color, bool _bobUpAndDown, bool _faceCamera, bool _rotate, Text3D _text3D, EventAction _eventAction)
        {
            this.type = _type;
            this.pos = _pos;
            this.dir = _dir;
            this.rot = _rot;
            this.scale = _scale;
            this.color = _color;
            this.bobUpAndDown = _bobUpAndDown;
            this.faceCamera = _faceCamera;
            this.rotate = _rotate;
            this.text3D = _text3D;
            this.eventAction = _eventAction;
        }

        public void Draw()
        {
            API.DrawMarker((int) this.type, this.pos.X, this.pos.Y, this.pos.Z, this.dir.X, this.dir.Y, this.dir.Z,
                this.rot.X, this.rot.Y, this.rot.Z, this.scale.X, this.scale.Y, this.scale.Z,
                this.color.R, this.color.G, this.color.B, this.color.A, this.bobUpAndDown, this.faceCamera,
                2, this.rotate, null, null, false);

            if (this.text3D != null)
            {
                this.text3D.Draw();
            }           

        }

        public void Check()
        {
            Vector3 playerCoords = API.GetEntityCoords(API.GetPlayerPed(API.PlayerId()), false);
            float distance = API.Vdist(this.pos.X, this.pos.Y, this.pos.Z, playerCoords.X, playerCoords.Y, playerCoords.Z);

            if (distance <= this.scale.X / 2 && distance <= this.scale.Y / 2 && this.eventAction != null)
            {
                this.eventAction.Draw();
                this.eventAction.CheckControl();
            }
        }
        
        
    }
}
