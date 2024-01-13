using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamClickOnCircle
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        List<Circle> circles = new List<Circle>();//массив наших кругов
        int countClick = 0;//кол-во кликов на панель
        int speed;//скорость роста
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(panel1.ClientSize.Width, panel1.ClientSize.Height);
            textBoxSpeed.Text = "1";
            speed = 1;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            
            int curSpeed = int.Parse(textBoxSpeed.Text);//берем значение скорость из комбобокса
            if(curSpeed > 0) speed = curSpeed;
            else
            {
                MessageBox.Show("Скорость роста должна быть больше 0");
                textBoxSpeed.Text = speed.ToString();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            bmp = new Bitmap(panel1.ClientSize.Width, panel1.ClientSize.Height);
            DrawCircles();//отрисовываем все круги

            e.Graphics.DrawImageUnscaled(bmp, 0, 0);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point coordCentre = e.Location;//координаты точки куда мы кликнули
            if (!ClickOnCircle(coordCentre.X, coordCentre.Y))
            {
                circles.Add(new Circle(coordCentre.X, coordCentre.Y));//если не попали в круг значит добавляем новый
            }

            countClick++;
            labelStatClick.Text = "Количество нажатий " + countClick.ToString();//выставляем в лейбл кол-во нажатий
            panel1.Invalidate();//перерисовываем
        }

        private void DrawCircles()
        {
            Graphics graphics = Graphics.FromImage(bmp);
            Pen pen;
            SolidBrush brush;
            foreach (Circle circle in circles)// проходим по всем круга и отрисовываем их
            {
                pen = new Pen(circle.color);
                brush = new SolidBrush(circle.color);
                graphics.DrawEllipse(pen, circle.leftUpBox.X, circle.leftUpBox.Y, circle.radius * 2, circle.radius * 2);//отрисовываем границу круга
                graphics.FillEllipse(brush, circle.leftUpBox.X, circle.leftUpBox.Y, circle.radius * 2, circle.radius * 2);//отрисовываем внутренность(заливаем)
            }
        }

        public bool ClickOnCircle(int x, int y)
        {
            foreach (Circle circle in circles)//проходим по всем кругам и смотри попали в какой то из них или нет, если попали true
            {
                if(circle.InCircle(x, y))
                {
                    circle.speed = speed;//если попали меняем у круга скорость
                    circle.CircleGrowing();//наращиваем круг, делаем его больше
                    CircleCrash(circle);//теперь на измененных размерах смотрим вышел ли он за пределы экрана или врезался в другой круг
                    return true;
                }
            }
            return false;
        }

        public void CircleCrash(Circle circle)
        {
            int upDirection = circle.y - circle.radius;//верхний центр
            int downDirection = circle.y + circle.radius;//нижний центр
            int rightDirection = circle.x + circle.radius;//правый центр
            int leftDirection = circle.x - circle.radius;//левый центр

            Point leftUp = circle.leftUpBox;//левый верхний
            Point leftDown = new Point(circle.leftUpBox.X, downDirection);//левый нижний
            Point rightUp = new Point(rightDirection, upDirection);//правый верхний угол
            Point rightDown = new Point(rightDirection, downDirection);//правый нижний
            if (upDirection < 0 || downDirection > panel1.ClientSize.Height
                || leftDirection < 0 || rightDirection > panel1.ClientSize.Width)//проверка на выход за пределы
            {
                circles.Remove(circle);
            }
            else
            {
                foreach(Circle circleOther in circles)//проверка на врезание в другой круг
                {

                    if (circle!=circleOther && ((circleOther.InCircle(leftUp.X, leftUp.Y)) || (circleOther.InCircle(leftDown.X, leftDown.Y))
                        || (circleOther.InCircle(rightUp.X, rightUp.Y)) || (circleOther.InCircle(rightDown.X, rightDown.Y))
                        || (circleOther.InCircle(leftDirection, circle.y)) || (circleOther.InCircle(rightDirection, circle.y))
                        || (circleOther.InCircle(circle.x, upDirection)) || (circleOther.InCircle(circle.x, downDirection))))
                    {
                        circles.Remove(circle);
                        break;
                    }
                }
            }
            
        }

        private void textBoxSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            //это просто ограничения на ввод, то есть можем вводить только 0-9 и ентер и удалить
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }
    }
}
