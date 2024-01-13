using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClickOnCircle
{
    public class Circle
    {
        public int x;//х центра круга
        public int y;//у центра круга
        public int radius;//радиус круга
        public int speed;//скорость роста
        public Color color;//цвет круга
        public Point leftUpBox;//точка левого верхнего угла квадрата в который вписан круг
        List<Color> colors = new List<Color>() { Color.Brown, Color.Red, Color.Purple, Color.Blue, Color.Green };//список цветом для рандомной генерайии
        Random random = new Random();

        public Circle(int x, int y)
        {
            this.x = x;
            this.y = y;
            speed = 1;
            radius = 10;
            color = colors[random.Next(colors.Count)];
            leftUpBox = CalculateCoordBox(this.x, this.y, this.radius);

        }
        public Point CalculateCoordBox(int xCentre, int yCentre, int r)//функция находящая левый верхний угол квадрата
        {
            Point coordBox = new Point(xCentre - r, yCentre - r);
            return coordBox;
        }


        public bool InCircle(int x, int y)//если данные координаты попали в круг - true, не попали false
        {
            int minX = leftUpBox.X;
            int minY = leftUpBox.Y;
            int maxX = minX + 2 * radius;
            int maxY = minY + 2 * radius;
            if (x >= minX && x <= maxX && y >= minY && y <= maxY) return true;
            else return false;
        }

        public void CircleGrowing()//рост круга
        {
            radius += speed;//прибавляем к радиусу скорость роста
            leftUpBox = CalculateCoordBox(this.x, this.y, this.radius );//меняем левый верхний угол квадрата так как радиус стал больше
        }
    }
}
