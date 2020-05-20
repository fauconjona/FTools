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

        public string Identifier { get; set; }
        public MarkerType Type { get; set; } = MarkerType.VerticalCylinder;
        public Vector3 Pos { get; set; } = new Vector3 { X = 0, Y = 0, Z = 0 };
        public Vector3 Dir { get; set; } = new Vector3 { X = 0, Y = 0, Z = 0 };
        public Vector3 Rot { get; set; } = new Vector3 { X = 0, Y = 0, Z = 0 };
        public Vector3 Scale { get; set; } = new Vector3 { X = 0, Y = 0, Z = 0 };
        public System.Drawing.Color Color { get; set; } = System.Drawing.Color.Empty;
        public bool BobUpAndDown { get; set; } = false;
        public bool FaceCamera { get; set; } = false;
        public bool Rotate { get; set; } = false;
        public float MaxDistance { get; set; } = 50.0f;
        public bool AllowInVehicle { get; set; } = true;   

        #endregion

        public Text3D Text3D { get; set; } = null;

        public EventAction EventAction { get; set; } = null;

        public void Draw()
        {
            double distance = Math.Sqrt(GameplayCamera.Position.DistanceToSquared(this.Pos));
            if (distance <= this.MaxDistance && (AllowInVehicle || !Game.PlayerPed.IsInVehicle()))
            {
                World.DrawMarker(this.Type, this.Pos, this.Dir, this.Rot, this.Scale, this.Color, this.BobUpAndDown, this.FaceCamera, this.Rotate);
            }
            
            if (this.Text3D != null && distance <= this.MaxDistance && (AllowInVehicle || !Game.PlayerPed.IsInVehicle()))
            {
                this.Text3D.Draw(this.Pos);
            }           

        }

        public void Check()
        {
            if (this.EventAction != null && Math.Sqrt(Game.PlayerPed.Position.DistanceToSquared(this.Pos)) <= this.Scale.X && (AllowInVehicle || !Game.PlayerPed.IsInVehicle()))
            {                
                this.EventAction.Draw();
                this.EventAction.CheckControl();
            }
        }
        
        
    }
}
