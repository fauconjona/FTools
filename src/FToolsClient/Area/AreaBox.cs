using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FToolsClient.Area
{
    class AreaBox : AreaBase
    {
        public Vector3 Pos1 { get; set; }
        public Vector3 Pos2 { get; set; }

        public override void Check()
        {
            if (API.IsEntityInArea(Game.PlayerPed.Handle, Pos1.X, Pos1.Y, Pos1.Z, Pos2.X, Pos2.Y, Pos2.Z, false, false, -1) && !this.PlayerInside)
            {
                this.PlayerInside = true;
                this.TriggerEnter();
            }
            else if (!API.IsEntityInArea(Game.PlayerPed.Handle, Pos1.X, Pos1.Y, Pos1.Z, Pos2.X, Pos2.Y, Pos2.Z, false, false, -1) && this.PlayerInside)
            {
                this.PlayerInside = false;
                this.TriggerExit();
            }
        }

        public override void Draw()
        {
            if (Math.Sqrt(Game.PlayerPed.Position.DistanceToSquared((Pos1 + Pos2) / 2)) > Math.Sqrt(Pos1.DistanceToSquared(Pos2)) * 5)
                return;

            World.DrawLine(Pos1, new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = Pos1.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(Pos1, new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = Pos1.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(Pos1, new Vector3 { X = Pos1.X, Y = Pos1.Y, Z = Pos2.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(Pos2, new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = Pos2.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(Pos2, new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = Pos2.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(Pos2, new Vector3 { X = Pos2.X, Y = Pos2.Y, Z = Pos1.Z }, System.Drawing.Color.FromArgb(255, 0, 0));

            World.DrawLine(new Vector3 { X = Pos2.X, Y = Pos2.Y, Z = Pos1.Z }, new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = Pos1.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos2.X, Y = Pos2.Y, Z = Pos1.Z }, new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = Pos1.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = Pos2.Z }, new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = Pos1.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos2.X, Y = Pos1.Y, Z = Pos2.Z }, new Vector3 { X = Pos1.X, Y = Pos1.Y, Z = Pos2.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = Pos2.Z }, new Vector3 { X = Pos1.X, Y = Pos1.Y, Z = Pos2.Z }, System.Drawing.Color.FromArgb(255, 0, 0));
            World.DrawLine(new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = Pos2.Z }, new Vector3 { X = Pos1.X, Y = Pos2.Y, Z = Pos1.Z }, System.Drawing.Color.FromArgb(255, 0, 0));

        }
    }
}
