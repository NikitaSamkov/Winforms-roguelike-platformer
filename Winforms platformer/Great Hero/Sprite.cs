﻿using System;
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
        private int currentMaxFrames;
        private int currentFrameTime;
        private readonly int framePause;
        public Bitmap fullSize { get; private set; }
        public int spriteWidth { get; private set; }
        public int spriteHeight { get; private set; }
        public int currentFrame { get; private set; }
        public Status currentStatus { get; private set; }

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
