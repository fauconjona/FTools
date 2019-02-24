using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FToolsClient.Area
{
    class AreaRectangle : AreaBase
    {
        public Vector2 Pos1 { get; set; }
        public Vector2 Pos2 { get; set; }

        public override void Check()
        {
            if (API.IsEntityInArea(Game.PlayerPed.Handle, Pos1.X, Pos1.Y, 0.0f, Pos2.X, Pos2.Y, 200.0f, false, false, -1) && !this.PlayerInside)
            {
                this.PlayerInside = true;
                this.TriggerEnter();
            }
            else if (!API.IsEntityInArea(Game.PlayerPed.Handle, Pos1.X, Pos1.Y, 0.0f, Pos2.X, Pos2.Y, 200.0f, false, false, -1) && this.PlayerInside)
            {
                this.PlayerInside = false;
                this.TriggerExit();
            }
        }

        public override void Draw()
        {
            if (Math.Sqrt(Game.PlayerPed.Position.DistanceToSquared2D(((new Vector3 { X = Pos1.X, Y = Pos1.Y, Z = 0.0f }) + (new Vector3 { X = Pos2.X, Y = Pos2.Y, Z = 0.0f })) / 2)) > 
                Math.Sqrt((new Vector3 { X = Pos1.X, Y = Pos1.Y, Z = 0.0f }).DistanceToSquared2D(new Vector3 { X = Pos2.X, Y = Pos2.Y, Z = 0.0f })) * 5)
                return;

            World.DrawLine(new Vector3 { X = Pos1.X, Y = Pos1.Y, Z = 0.0f }, new Vector3 { X = Pos1.X, Y = Pos1.Y, Z = 200.0f }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = 0.0f }, new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = 200.0f }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = 0.0f }, new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = 200.0f }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos2.X, Y = Pos2.Y, Z = 0.0f }, new Vector3 { X = Pos2.X, Y = Pos2.Y, Z = 200.0f }, System.Drawing.Color.FromArgb(255, 0, 0));

        }
    }
}
