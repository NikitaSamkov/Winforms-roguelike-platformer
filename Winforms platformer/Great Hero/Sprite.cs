using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    public class Sprite
    {
        private int currentFrameTime;
        private readonly int framePause;
        private Status currentStatus;
        private int currentMaxFrames;
        public readonly int idleMaxFrames;
        public readonly int moveMaxFrames;
        public readonly Bitmap fullSize;
        public readonly Bitmap idleSheet;
        public readonly Bitmap moveSheet;
        public int spriteWidth { get; private set; }
        public int spriteHeight { get; private set; }
        public int currentFrame { get; private set; }

        public Sprite(Bitmap fullSizeSprite, Bitmap idleSheet = null, Bitmap moveSheet = null, int oneFramePause = 1)
        {
            fullSize = fullSizeSprite;
            if (idleSheet == null)
                this.idleSheet = fullSize;
            else
                this.idleSheet = idleSheet;
            this.moveSheet = moveSheet;
            spriteWidth = fullSize.Width;
            spriteHeight = fullSize.Height;
            idleMaxFrames = this.idleSheet.Width / fullSize.Width;
            currentFrameTime = 0;
            framePause = oneFramePause;
            if (this.moveSheet != null)
                moveMaxFrames = this.moveSheet.Width / fullSize.Width;
            SetIdle();
        }

        public void StepFrame()
        {
            currentFrameTime++;
            if (currentFrameTime >= framePause)
            {
                currentFrame++;
                currentFrameTime = 0;
            }
            if (currentFrame >= currentMaxFrames)
                currentFrame = 0;
        }

        public void SetIdle()
        {
            currentStatus = Status.Idle;
            currentMaxFrames = idleMaxFrames;
        }

        public void SetMoving()
        {
            currentStatus = Status.Move;
            currentMaxFrames = moveMaxFrames;
        }

        public Bitmap GetSheet()
        {
            switch (currentStatus)
            {
                case Status.Idle:
                    return idleSheet;
                case Status.Move:
                    return moveSheet;
                default: return null;
            }
        }
    }
}
