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
            Create();
        }

        public static void Create()
        {
            Renders = new List<IRenderable>();
            Renders.Add(new RoomRender(Game.Map.CurrentRoom));
            Renders.Add(new BossRender(Game.Map.CurrentRoom));
            Renders.Add(new LootRender(Game.Map.CurrentRoom));
            Renders.Add(new EnemiesRender(Game.Map.CurrentRoom));
            Renders.Add(new EntityRender(Game.Player, Res.Player, 3));
            Renders.Add(new ProjectilesRender(Game.Map.CurrentRoom));
            Renders.Add(new UIRender());
        }

        public static void RenderAll(Graphics g)
        {
            if (Game.Death)
                g.DrawImage(Res.System.Death, 0, 0);
            else if (Game.Win)
                g.DrawImage(Res.System.Win, 0, 0);
            else
                foreach (var render in Renders)
                    render.Paint(g);
        }
    }
}
