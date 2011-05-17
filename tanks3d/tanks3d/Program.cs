using System;

namespace tanks3d
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
#if INSIDE_WINFORM
            WinFormContainer form = new WinFormContainer();
            form.Show();
            Game1 game = new Game1(form);
            form.game = game;
            game.Run();
#else
            using (Game1 game = new Game1())
            {
                game.Run();
            }
#endif


        }
    }
#endif
}

