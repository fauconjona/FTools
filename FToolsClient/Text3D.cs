using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FToolsClient
{
    class Text3D
    {
        private string text;
        private int font;
        private Color color;
        private Vector2 scale;
        private Vector3 pos;
        private float maxDistance = 20.0f;

        public Text3D(string _text, int _font, Color _color, Vector2 _scale, Vector3 _pos)
        {
            this.text = _text;
            this.font = _font;
            this.color = _color;
            this.scale = _scale;
            this.pos = _pos;
        }
        public Text3D(string _text, int _font, Color _color, Vector2 _scale, Vector3 _pos, float _maxDistance)
        {
            this.text = _text;
            this.font = _font;
            this.color = _color;
            this.scale = _scale;
            this.pos = _pos;
            this.maxDistance = _maxDistance;
        }


        public void Draw()
        {
            Vector3 camCoords = API.GetGameplayCamCoord();
            float distance = API.Vdist(this.pos.X, this.pos.Y, this.pos.Z, camCoords.X, camCoords.Y, camCoords.Z);
            float _2dScale = ((1 / distance) * 2) * API.GetGameplayCamFov() / 20.0f;

            if (distance > this.maxDistance)
            {
                return;
            }

            //display
            API.SetTextScale(this.scale.X * _2dScale, this.scale.Y * _2dScale);
            API.SetTextFont(this.font);
            API.SetTextProportional(true);
            API.SetTextColour(this.color.R, this.color.G, this.color.B, this.color.A);
            API.SetTextDropshadow(0, 0, 0, 0, 255);
            API.SetTextEdge(2, 0, 0, 0, 150);
            API.SetTextDropShadow();
            API.SetTextOutline();

            API.SetDrawOrigin(this.pos.X, this.pos.Y, this.pos.Z, 0);

            API.SetTextEntry("STRING");
            API.SetTextCentre(true);
            API.AddTextComponentString(this.text);
            API.EndTextCommandDisplayText(0, 0);
            API.ClearDrawOrigin();            
        }
    }
}
