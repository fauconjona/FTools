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
        public Entity Entity { get; set; } = null;
        public float MaxDistance { get; set; } = 20.0f;

        public void Draw()
        {
            Vector3 position = GetPosition();
            float distance = (float)Math.Sqrt(GameplayCamera.Position.DistanceToSquared(position));
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

            API.SetDrawOrigin(position.X, position.Y, position.Z, 0);

            API.SetTextEntry("STRING");
            API.SetTextCentre(true);
            API.AddTextComponentString(this.TextString);
            API.EndTextCommandDisplayText(0, 0);
            API.ClearDrawOrigin();            
        }

        public void Draw(Vector3 relativePos)
        {
            Vector3 position = GetPosition();
            float distance = (float)Math.Sqrt(GameplayCamera.Position.DistanceToSquared(new Vector3 { X = relativePos.X + position.X, Y = relativePos.Y + position.Y, Z = relativePos.Z + position.Z }));
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

            API.SetDrawOrigin(relativePos.X + position.X, relativePos.Y + position.Y, relativePos.Z + position.Z, 0);

            API.SetTextEntry("STRING");
            API.SetTextCentre(true);
            API.AddTextComponentString(this.TextString);
            API.EndTextCommandDisplayText(0, 0);
            API.ClearDrawOrigin();
        }

        public bool EntityExist()
        {
            return this.Entity.Exists();
        }

        public Vector3 GetPosition()
        {
            if (this.Entity != null && this.EntityExist())
            {
                // this.Pos is the offset
                return this.Entity.Position + this.Pos;
            }
            else
            {
                return this.Pos;
            }
        }
    }
}
