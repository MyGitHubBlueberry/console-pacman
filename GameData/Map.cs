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
               System.Console.Write(map[i, j]);
            }
            System.Console.WriteLine();
         }
      }

      public void FindeCharacters(char[,] map, out Pacman pacman, out Ghost[] ghosts)
      {
         int index = 0;
         int dotsCounter = 1;
         pacman = new Pacman(0, 0);
         ghosts = new Ghost[4];
         int colorChooser = 0;
         ConsoleColor ghostColor = ConsoleColor.White;
         ConsoleColor defaultColor = Console.ForegroundColor;

         for (int i = 0; i < map.GetLength(0); i++)
         {
            for (int j = 0; j < map.GetLength(1); j++)
            {
               if (map[i, j] == '@')
               {
                  Console.SetCursorPosition(j, i);
                  Console.ForegroundColor = ConsoleColor.Yellow;
                  Console.Write('@');
                  Console.ForegroundColor = defaultColor;
                  pacman = new Pacman(i, j);
               }
               if (map[i, j] == '$')
               {
                  switch (colorChooser)
                  {
                     case 0:
                        Console.SetCursorPosition(j, i);
                        ghostColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ghostColor;
                        Console.Write('$');
                        Console.ForegroundColor = defaultColor;
                        break;
                     case 1:
                        Console.SetCursorPosition(j, i);
                        ghostColor = ConsoleColor.Red;
                        Console.ForegroundColor = ghostColor;
                        Console.Write('$');
                        Console.ForegroundColor = defaultColor;
                        break;
                     case 2:
                        Console.SetCursorPosition(j, i);
                        ghostColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ghostColor;
                        Console.Write('$');
                        Console.ForegroundColor = defaultColor;
                        break;
                     case 3:
                        Console.SetCursorPosition(j, i);
                        ghostColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ghostColor;
                        Console.Write('$');
                        Console.ForegroundColor = defaultColor;
                        break;
                  }
                  ghosts[index] = new Ghost(i, j, ghostColor);
                  colorChooser++;
                  index++;
               }
               if (map[i, j] == ' ')
               {
                  map[i, j] = '.';
                  Console.SetCursorPosition(j, i);
                  Console.Write('.');
                  AllDots = dotsCounter;
                  dotsCounter++;
               }
            }
         }
      }
   }
}