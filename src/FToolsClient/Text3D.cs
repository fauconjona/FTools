using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace FToolsClient
{
    class Text3D
    {
        public string TextString { get; set; } = "";
        public Font Font { get; set; } = 0;
        public System.Drawing.Color Color { get; set; } = System.Drawing.Color.Empty;
        public Vector2 Scale { get; set; } = new Vector2 { X = 0, Y = 0 };
        public Vector3 Pos { get; set; } = new Vector3 { X = 0, Y = 0, Z = 0 };
        public float MaxDistance { get; set; } = 20.0f;

        public void Draw()
        {
            float distance = (float)Math.Sqrt(GameplayCamera.Position.DistanceToSquared(this.Pos));
            float _2dScale = ((1 / distance) * 2) * GameplayCamera.FieldOfView / 20.0f;

            if (distance > this.MaxDistance)
            {
                return;
            }

            //display
            API.SetTextScale(this.Scale.X * _2dScale, this.Scale.Y * _2dScale);
            API.SetTextFont((int)this.Font);
            API.SetTextProportional(true);
            API.SetTextColour(this.Color.R, this.Color.G, this.Color.B, this.Color.A);
            API.SetTextDropshadow(0, 0, 0, 0, 255);
            API.SetTextEdge(2, 0, 0, 0, 150);
            API.SetTextDropShadow();
            API.SetTextOutline();

            API.SetDrawOrigin(this.Pos.X, this.Pos.Y, this.Pos.Z, 0);

            API.SetTextEntry("STRING");
            API.SetTextCentre(true);
            API.AddTextComponentString(this.TextString);
            API.EndTextCommandDisplayText(0, 0);
            API.ClearDrawOrigin();            
        }
        public void Draw(Vector3 relativePos)
        {
            float distance = (float)Math.Sqrt(GameplayCamera.Position.DistanceToSquared(new Vector3 { X = relativePos.X + this.Pos.X, Y = relativePos.Y + this.Pos.Y, Z = relativePos.Z + this.Pos.Z }));
            float _2dScale = ((1 / distance) * 2) * GameplayCamera.FieldOfView / 20.0f;

            if (distance > this.MaxDistance)
            {
                return;
            }

            //display
            API.SetTextScale(this.Scale.X * _2dScale, this.Scale.Y * _2dScale);
            API.SetTextFont((int)this.Font);
            API.SetTextProportional(true);
            API.SetTextColour(this.Color.R, this.Color.G, this.Color.B, this.Color.A);
            API.SetTextDropshadow(0, 0, 0, 0, 255);
            API.SetTextEdge(2, 0, 0, 0, 150);
            API.SetTextDropShadow();
            API.SetTextOutline();

            API.SetDrawOrigin(relativePos.X + this.Pos.X, relativePos.Y + this.Pos.Y, relativePos.Z + this.Pos.Z, 0);

            API.SetTextEntry("STRING");
            API.SetTextCentre(true);
            API.AddTextComponentString(this.TextString);
            API.EndTextCommandDisplayText(0, 0);
            API.ClearDrawOrigin();
        }
    }
}
