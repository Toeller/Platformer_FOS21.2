using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatformerFos21
{
    public partial class Form1 : Form
    {
        #region Attribute
        // === Платформы ===
        List<Platform> platforms = new List<Platform>();
        Random rand = new Random();
        private float lastY; // координата Y последней платформы для генерации новых платформ

        // === ПОЛЯ КЛАССА ===
        Player player;

        // === GAME TIMER ===
        Timer gameTimer = new Timer();

        // === клавиши ===
        bool leftPressed = false;
        bool rightPressed = false;
        bool jumpPressed = false;

        // === КАМЕРА ===
        float cameraY = 0; // смещение камеры по вертикали
        #endregion
        public Form1()
        {
            InitializeComponent();

            // === НАСТРОЙКИ ОКНА ===
            this.DoubleBuffered = true;
            this.Width = 1000;
            this.Height = 600;
            this.Text = "2D Platformer (pure C#)";

            // === ИНИЦИАЛИЗАЦИЯ ПЛАТФОРМ ===
            lastY = 500; // начальная нижняя платформа
            platforms.Add(new Platform(0, lastY, 1000, 100)); // пол

            // первые платформы
            for (int i = 0; i < 5; i++)
                GeneratePlatform();

            // === СОЗДАЁМ ИГРОКА ===
            player = new Player(100, 100);

            // === GAME LOOP ===
            gameTimer.Interval = 16; // ~60 FPS
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        // === Генерация платформ ===
        void GeneratePlatform()
        {
            int width = rand.Next(100, 250);
            int x = rand.Next(0, 1000 - width);
            int gap = rand.Next(80, 150);

            lastY -= gap; // реальная координата новой платформы
            platforms.Add(new Platform(x, lastY, width, 20));
        }

        // === ОБРАБОТКА КЛАВИШ ===
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.A || keyData == Keys.Left) leftPressed = true;
            if (keyData == Keys.D || keyData == Keys.Right) rightPressed = true;
            if (keyData == Keys.Space) jumpPressed = true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left) leftPressed = false;
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right) rightPressed = false;
            if (e.KeyCode == Keys.Space) jumpPressed = false;
            base.OnKeyUp(e);
        }

        // === ОСНОВНОЙ ЦИКЛ ИГРЫ ===
        void GameLoop(object sender, EventArgs e)
        {
            UpdateGame();
            Invalidate();
        }

        // === ЛОГИКА ИГРЫ ===
        void UpdateGame()
        {
            // обновление игрока
            player.Update(leftPressed, rightPressed, jumpPressed);

            // проверка коллизий
            player.OnGround = false;
            foreach (var plat in platforms)
                player.CheckCollision(plat);

            // === Генерация новых платформ сверху ===
            while (lastY > 0)
                GeneratePlatform();

            // === Удаление платформ, которые ушли за экран вниз, кроме пола ===
            platforms.RemoveAll(p => p.Y > 600 && p != platforms[0]);

            // === КАМЕРА: только визуальное смещение ===
            float upperLimit = 300;
            float lowerLimit = 400;

            float diff = 0;

            if (player.Y < upperLimit)
                diff = upperLimit - player.Y;
            else if (player.Y > lowerLimit)
                diff = lowerLimit - player.Y;

            if (diff != 0)
            {
                // игрок остаётся в пределах зоны
                player.Y += diff;

                // смещаем платформы кроме пола
                for (int i = 1; i < platforms.Count; i++)
                    platforms[i].Y += diff;
            }
        }

        // === ОТРИСОВКА ===
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            foreach (var plat in platforms)
                plat.Draw(g);

            player.Draw(g);
        }
    }
}
