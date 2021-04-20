using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winforms_platformer
{
    class Sprite
    {
        public readonly Bitmap idleSheet;
        private int idleMaxFrames;
        public readonly Bitmap moveSheet;
        private int moveMaxFrames;
        public Bitmap fullSize { get; private set; }
        public int spriteWidth { get; private set; }
        public int spriteHeight { get; private set; }
        public int currentFrame { get; private set; }
        private int currentMaxFrames;
        public Status currentStatus { get; private set; }

        public Sprite(Bitmap fullSizeSprite, Bitmap idleSheet = null, Bitmap moveSheet = null)
        {
            fullSize = fullSizeSprite;
            if (idleSheet == null)
                this.idleSheet = fullSize;
            else
                this.idleSheet = idleSheet;
            this.moveSheet = moveSheet;
            spriteWidth = fullSize.Width;
            spriteHeight = fullSize.Height;
            if (idleSheet != null)
                idleMaxFrames = idleSheet.Width / fullSizeSprite.Width;
            if (moveSheet != null)
                moveMaxFrames = moveSheet.Width / fullSizeSprite.Width;
            SetIdle();
        }

        public void StepFrame()
        {
            currentFrame++;
            if (currentFrame >= currentMaxFrames)
                currentFrame = 0;
        }

        public void SetIdle()
        {
            currentStatus = Status.Idle;
            currentMaxFrames = idleMaxFrames;
            currentFrame = 0;
        }

        public void SetMoving()
        {
            currentStatus = Status.Move;
            currentMaxFrames = moveMaxFrames;
            currentFrame = 0;
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
