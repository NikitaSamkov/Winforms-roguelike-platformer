using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winforms_platformer.Model;

namespace Winforms_platformer.View
{
    public static class GameRender
    {
        public static List<IRenderable> Renders;

        static GameRender()
        {
            Renders = new List<IRenderable>();
            Renders.Add(new RoomRender(Game.Map.CurrentRoom));
            Renders.Add(new TreasuresRender(Game.Map.CurrentRoom));
            Renders.Add(new EntityRender(Game.Player, Resources.Player, 3));
            Renders.Add(new EnemysRender(Game.Map.CurrentRoom));
            Renders.Add(new ProjectilesRender(Game.Map.CurrentRoom));
        }

        public static void RenderAll(Graphics g)
        {
            if (Game.GameOver)
                g.DrawImage(Resources.System.Death, 0, 0);
            else
                foreach (var render in Renders)
                    render.Paint(g);
        }
    }
}
