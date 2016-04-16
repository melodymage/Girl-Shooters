using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Girl_Shooters
{
    class Girls
    {
        public Texture2D player;
        public Vector2 position;
        public Vector2 center;
        public float rotation;
        public float speed;
        private float scale;
        public Vector2 moveInDirection;
        public bool alive;
        public bool visible;
        private Rectangle objectRectangle;
        private Rectangle sourceRectangle;
        public SpriteEffects spriteEffects;
        public float layer;
        public Point framesize;
        public Point currentframe;
        public Point sheetsize;

        public Rectangle ObjectRectangle
        {
            get
            {
                objectRectangle.X = (int)(position.X - (objectRectangle.Width / 2));
                objectRectangle.Y = (int)(position.Y - (objectRectangle.Height / 2));
                return objectRectangle;
            }
        }
        public Rectangle SourceRectangle
        {
            set
            {
                sourceRectangle = value;
                objectRectangle = sourceRectangle;
                center = new Vector2(32 / 2, 48 / 2);
                objectRectangle.Width = sourceRectangle.Width;
                objectRectangle.Height = sourceRectangle.Height;
            }
            get
            {
                return sourceRectangle;
            }
        }
        public float Scale
        {
            set
            {
                scale = value;
                objectRectangle.Width = (int)(32 * value);
                objectRectangle.Height = (int)(48 * value);
            }
            get
            {
                return scale;
            }
        }

        public Girls(Texture2D texture)
        {
            player = texture;
            scale = 1.0f;
            position = Vector2.Zero;
            rotation = 0;
            center = new Vector2(32 / 2, 48 / 2);
            moveInDirection = Vector2.Zero;
            objectRectangle = new Rectangle(0, 0, 32, 48);
            sourceRectangle = new Rectangle(0, 0, player.Width, player.Height);
            speed = 5;
            alive = true;
            visible = true;
            spriteEffects = SpriteEffects.None;
            layer = 1.0f;
            
        }
        public void update(GameTime gametime)
        {
           
        }
        public void draw(SpriteBatch spriteBatch)
        {
            if (visible)
                spriteBatch.Draw(player, position, new Rectangle(currentframe.X * framesize.X,
                    currentframe.Y * framesize.Y, framesize.X, framesize.Y),
                    Color.White, rotation, center, scale,
                    spriteEffects, layer);
        }
        public bool checkCollideWith(Enemies object2)
        {
            return ObjectRectangle.Intersects(object2.ObjectRectangle);
        }
        public bool checkCollideWith(Boss object2)
        {
            return ObjectRectangle.Intersects(object2.TouchReactangle);
        }
        public bool checkCollideWith(Bullets object2)
        {
            return ObjectRectangle.Intersects(object2.ObjectRectangle);
        }
    }
}
