using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Linq;
using System.Text;

namespace Pong
{
    class Sprite
    {
        private Texture2D sprite;
        private Texture2D sprite_debug;
        private Rectangle sprite_colbox = new Rectangle();
        private Vector2 sprite_pos = new Vector2();
        private Rectangle sprite_rect = new Rectangle();


        public void SpriteLoadPixel(GraphicsDevice graphics, ContentManager content, float x, float y, int w, int h, Color color)
        {
            sprite_debug = content.Load<Texture2D>("graphics/collisionbox");
            sprite = new Texture2D(graphics, w, h);

            Color[] Sdata = new Color[w * h];
            for (int i = 0; i < Sdata.Length; i++)
            {
                Sdata[i] = color;
            }
            sprite.SetData(Sdata);

            sprite_pos.X = x;
            sprite_pos.Y = y;

            sprite_rect.X = (int)sprite_pos.X - sprite.Width / 2;
            sprite_rect.Y = (int)sprite_pos.Y - sprite.Height / 2;

            sprite_rect.Width = w;
            sprite_rect.Height = h;

            sprite_colbox.Width = w;
            sprite_colbox.Height = h;

        }
        public void SpriteLoadContent(string FileSource, ContentManager content, float x, float y, int w, int h)
        {
            sprite_debug = content.Load<Texture2D>("graphics/collisionbox");
            sprite = content.Load<Texture2D>(FileSource);

            sprite_pos.X = x;
            sprite_pos.Y = y;

            sprite_rect.X = (int)sprite_pos.X;
            sprite_rect.Y = (int)sprite_pos.Y;

            sprite_rect.Width = w;
            sprite_rect.Height = h;

            sprite_colbox.Width = sprite_rect.Width;
            sprite_colbox.Height = sprite_rect.Height;

        }
        public void SpriteSetPos(float x, float y)
        {
            sprite_pos.X = x;
            sprite_pos.Y = y;
            sprite_rect.X = (int)sprite_pos.X - sprite.Width / 2;
            sprite_rect.Y = (int)sprite_pos.Y - sprite.Height / 2;
        }
        public void SpriteSetColbox()
        {
            sprite_colbox.X = (int)sprite_pos.X - sprite.Width / 2;
            sprite_colbox.Y = (int)sprite_pos.Y - sprite.Height / 2;
            sprite_rect.X = (int)sprite_pos.X - sprite.Width / 2;
            sprite_rect.Y = (int)sprite_pos.Y - sprite.Height / 2;
        }
        public void SpriteSetPos_X(float x)
        {
            sprite_pos.X = x;
        }
        public void SpriteSetPos_Y(float y)
        {
            sprite_pos.Y = y;
        }
        public void SetSpriteRect_W(int w)
        {
            sprite_rect.Width = w;
        }
        public void SetSpriteRect_H(int h)
        {
            sprite_rect.Height = h;
        }
        public Rectangle GetSpriteRect()
        {
            return sprite_rect;
        }
        public Rectangle GetColbox()
        {
            return sprite_colbox;
        }
        public Vector2 GetSpritePos()
        {
            return sprite_pos;
        }
        public void SpriteDrawPixel(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(sprite,
                new Rectangle((int)sprite_pos.X, (int)sprite_pos.Y, sprite.Width, sprite.Height),
                null,
                Color.White,
                0,
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                SpriteEffects.None,
                0);

            spriteBatch.End();
        }
        public void SpriteDrawContent(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(sprite,
                sprite_rect,
                null,
                Color.White,
                0,
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                SpriteEffects.None,
                0);

            spriteBatch.End();
        }
        public void SpriteDrawDebug(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite_debug, sprite_colbox, Color.White);
            spriteBatch.Draw(sprite_debug, sprite_rect, Color.LightGreen);
            spriteBatch.End();
        }
        public void SpriteDispose()
        {
            sprite.Dispose();
        }

    }
}
