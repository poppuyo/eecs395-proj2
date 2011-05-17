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
            WinFormContainer form = new WinFormContainer();
            form.Show();
            Game1 game = new Game1(form);
            form.game = game;
            game.Run();
        }
    }
#endif
}

