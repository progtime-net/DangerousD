using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace AnimationsFileCreator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в костыльную программу по созданию файлов анимации для игры DungerousD");
            Console.Write("Введите название текстуры (нажмите enter, чтобы выбрать файл во всплывающем окошке): ");
            string textureName = Console.ReadLine();
            if (textureName == "")
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                textureName = dialog.FileName.Split('\\').Last();
                textureName = textureName.Split('.')[0];
            }
            Console.WriteLine("Введите количество кадров анимации: ");
            int framesCount = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите длительность кадра в анимации: ");
            int interval = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите начальную позицию X ректенгла анимации: ");
            Rectangle rectangle = new Rectangle();
            rectangle.X = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите начальную позицию Y ректенгла анимации: ");
            rectangle.Y = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите начальную позицию Width ректенгла анимации: ");
            rectangle.Width = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите начальную позицию Height ректенгла анимации: ");
            rectangle.Height = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите название для этого файла - id анимации");
            string id = Console.ReadLine();
            Console.WriteLine("Введите 1 если анимация зациклена, и 0 если нет");
            AnimationContainer container = new AnimationContainer();
            
            int a = int.Parse(Console.ReadLine());
            if (a==1)
            {
                container.IsCycle = true;
            }
            else
            {
                container.IsCycle = false;
            }
            Console.WriteLine("Введите отклонение анимации от стандартной (сначала X, потом enter, потом Y): ");
            int otklx = int.Parse(Console.ReadLine());
            int otkly = int.Parse(Console.ReadLine());
            container.Offset =new Vector2(otklx,otkly);
            container.FramesCount = framesCount;
            container.FrameTime = new System.Collections.Generic.List<Tuple<int, int>>();
            container.FrameTime.Add(new Tuple<int, int>(0, interval));
            container.StartSpriteRectangle = rectangle;
            container.TextureName = textureName;
            container.TextureFrameInterval = 1;
            container.Id = id;
            string json = JsonConvert.SerializeObject(container);
            StreamWriter writer = new StreamWriter(id);
            writer.WriteLine(json);
            writer.Close();
        }
    }
}
