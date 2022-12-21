namespace Game
{
   public sealed class Map
   {
      public int AllDots { get; private set; }

      public Map(){ }

      public char[,] ChooseMap(string[] availibleMaps)
      {
         Random randomMap = new Random();
         availibleMaps = File.ReadAllLines($"Maps/{availibleMaps[randomMap.Next(0, availibleMaps.Length)]}");
         char[,] map = new char[availibleMaps.Length, availibleMaps[0].Length];

         for (int i = 0; i < map.GetLength(0); i++)
         {
            for (int j = 0; j < map.GetLength(1); j++)
            {
               map[i, j] = availibleMaps[i][j];
            }
         }
         return map;
      }

      public void DrawMap(char[,] map)
      {
         for (int i = 0; i < map.GetLength(0); i++)
         {
            for (int j = 0; j < map.GetLength(1); j++)
            {
               Console.Write(map[i, j]);
            }
            Console.WriteLine();
         }
      }

      public void FindeCharacters(char[,] map, out Pacman pacman, out Ghost[] ghosts)
      {
         int index = 0;
         int dotsCounter = 1;
         pacman = new Pacman(0, 0);
         ghosts = new Ghost[4];
         int colorChooser = 0;
         ConsoleColor defaultColor = Console.ForegroundColor;

         for (int mapI = 0; mapI < map.GetLength(0); mapI++)
         {
            for (int mapJ = 0; mapJ < map.GetLength(1); mapJ++)
            {
               if (map[mapI, mapJ] == '@')
               {
                  pacman = InitializePacman(mapI, mapJ, defaultColor);
               }
               else if (map[mapI, mapJ] == '$')
               {
                  ghosts[index] = InitializeGhost(colorChooser, mapI, mapJ);
                  index++;
               }
               else if (map[mapI, mapJ] == ' ')
               {
                  InitializeDots(map, mapI, mapJ, dotsCounter);
               }
            }
         }
      }

      private Pacman InitializePacman(int mapI, int mapJ, ConsoleColor defaultColor)
      {
         Console.SetCursorPosition(mapJ, mapI);
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write('@');
         Console.ForegroundColor = defaultColor;
         return new Pacman(mapI, mapJ);
      }

      private Ghost InitializeGhost(int colorChooser, int mapI, int mapJ)
      {
         ConsoleColor ghostColor = ConsoleColor.White;
         switch (colorChooser)
         {
            case 0:
               ghostColor = ConsoleColor.Cyan;
               break;
            case 1:
               ghostColor = ConsoleColor.Red;
               break;
            case 2:
               ghostColor = ConsoleColor.DarkYellow;
               break;
            case 3:
               ghostColor = ConsoleColor.Magenta;
               break;
         }
         Console.SetCursorPosition(mapJ, mapI);
         Console.ForegroundColor = ghostColor;
         Console.Write('$');
         return new Ghost(mapI, mapJ, ghostColor);
      }

      private void InitializeDots(char[,]map, int mapI, int mapJ, int dotsCounter)
      {
         map[mapI, mapJ] = '.';
         Console.SetCursorPosition(mapJ, mapI);
         Console.Write('.');
         AllDots = dotsCounter;
         dotsCounter++;
      }
   }
}