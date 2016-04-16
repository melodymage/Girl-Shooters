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
    class Bullets
    {
        public Texture2D item;
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
        public int index = 0;
        public bool spritesBool = false;
        public bool animation;
        public Texture2D dummyTexture;


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
                center = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
                //objectRectangle.Width = sourceRectangle.Width;
                //objectRectangle.Height = sourceRectangle.Height;
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
                objectRectangle.Width = (int)(item.Width * value);
                objectRectangle.Height = (int)(item.Height * value);
            }
            get
            {
                return scale;
            }
        }

        public Bullets(Texture2D texture, GraphicsDeviceManager graphics)
        {
            item = texture;
            scale = 1.0f;
            position = Vector2.Zero;
            rotation = 0;
            center = new Vector2(item.Width / 2, item.Height / 2);
            moveInDirection = Vector2.Zero;
            objectRectangle = new Rectangle(0, 0, item.Width, item.Height);
            sourceRectangle = new Rectangle(0, 0, item.Width, item.Height);
            speed = 7;
            alive = true;
            visible = true;
            spriteEffects = SpriteEffects.None;
            layer = 1.0f;
            dummyTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            animation = false;
        }
        public void update(GameTime gameTime)
        {
            position.X -= speed;
        }
        
        public void draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(dummyTexture, ObjectRectangle, Color.White);
            if (visible)
            {
                spriteBatch.Draw(item, position, null, Color.White, rotation, center, scale, spriteEffects, layer);
            }
        }
        public bool checkCollideWith(Enemies object2)
        {
            return ObjectRectangle.Intersects(object2.ObjectRectangle);
        }
        public bool checkCollideWith(Boss object2)
        {
            return ObjectRectangle.Intersects(object2.ObjectRectangle);
        }
        public bool checkCollideWith(Bullets object2)
        {
            return ObjectRectangle.Intersects(object2.ObjectRectangle);
        }
    }
}
