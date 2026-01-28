using System.Drawing;

namespace PlatformerFos21
{
    class Platform
    {
        public float X;
        public float Y;
        public int Width;
        public int Height;

        public RectangleF Rect => new RectangleF(X, Y, Width, Height);

        public Platform(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Green, Rect);
        }
    }
}