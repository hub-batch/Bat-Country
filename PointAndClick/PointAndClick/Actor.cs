using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointAndClick
{
    public class Actor
    {
        public string ActorName { get; set; }
        public int DialogResponse { get; set; }
        public Texture2D ActorTex { get; set; }
        public int ActorID { get; set; }
        public List<string> ActorDialog = new List<string>();
        public bool IsTalking { get; set; }
    }
}
