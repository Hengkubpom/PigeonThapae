using _321_Lab05_3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pigeonthapae
{
    internal class Car
    {
        private int speed = 5;
        private Vector2 target = new Vector2(400, 610);
        public Vector2 pos = new Vector2(1200, 610);
        private AnimatedTexture anim = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
        private float time_out;
        public Rectangle hitbox;
        public Car(Texture2D asset, float time_out)
        {
            anim.Load(asset,7,2,10,1);
            this.time_out = time_out;
        }
        public void Draw(SpriteBatch _batch)
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 138, 64);
            anim.DrawFrame(_batch, pos, true);
        }
        public void update(float elapsed)
        {
            anim.UpdateFrame(elapsed);
            time_out -= elapsed;

            if (pos != target)
            {
                if (pos.X > target.X)
                {
                    pos.X -= speed;
                }
                anim.startrow = 2;
            }
            else
            {
                anim.startrow = 1;
            }

            if (time_out <= 0)
            {
                target = new Vector2(-300, 610);
            }
        }
    }
}
