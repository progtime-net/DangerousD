using DangerousD.GameCore;
using System;

namespace DangerousD
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new AppManager())
                game.Run();
        }
    }
}
