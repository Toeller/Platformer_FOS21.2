using System.Drawing;

namespace PlatformerFos21
{
    class Player
    {
        public float X;
        public float Y;

        public int Width = 50;
        public int Height = 50;

        public float VelX;
        public float VelY;

        public bool OnGround;

        public RectangleF Rect =>
            new RectangleF(X, Y, Width, Height);

        public Player(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Обновление игрока с клавишами
        public void Update(bool left, bool right, bool jump)
        {
            float speed = 5f;

            // движение влево/вправо
            if (left) VelX = -speed;
            else if (right) VelX = speed;
            else VelX = 0;

            // прыжок
            if (jump && OnGround)
            {
                VelY = -15f; // сила прыжка
                OnGround = false;
            }

            // гравитация
            VelY += 0.8f;

            // перемещение
            X += VelX;
            Y += VelY;

            // === ОГРАНИЧЕНИЕ ПО КРАЯМ ЭКРАНА ===
            if (X < 0) X = 0;
            if (X + Width > 1000) X = 1000 - Width;  // ширина окна 1000
            if (Y < 0) Y = 0;
            if (Y + Height > 600)                     // нижний край окна
            {
                Y = 600 - Height;
                VelY = 0;
                OnGround = true;
            }
        }

        // Проверка коллизии с платформой
        public void CheckCollision(Platform platform)
        {
            // проверяем только падение
            if (VelY >= 0 && Rect.IntersectsWith(platform.Rect))
            {
                Y = platform.Y - Height; // ставим игрока на платформу
                VelY = 0;
                OnGround = true; // игрок на платформе
            }
            // НЕ ставим OnGround = false здесь — сброс делаем в Form1.cs
        }

        // Отрисовка игрока
        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.White, Rect);
        }
    }
}

