namespace Game
{
   public sealed class Pacman : GameObject
   {
      private int _directionX, _directionY;
      public bool isAlive{ get; private set; }
      public int CollectedDots{ get; private set; }
      public int X{ get { return _x; } }
      public int Y{ get { return _y; } }

      public Pacman(int x, int y) : base(x,y)
      {
         isAlive = true;
      }
      
      private void ChangeDirection(ConsoleKeyInfo key, char[,] map, ref Pacman pacman)
      {
         switch(key.Key)
         {
            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
               if(map[pacman._x-1, pacman._y] != '#')
               {
                  pacman._directionY = 0;
                  pacman._directionX = -1;
               }
               break;
            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
               if(map[pacman._x+1, pacman._y] != '#')
               {
                  pacman._directionY = 0;
                  pacman._directionX = 1;
               }
               break;
            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
               if(map[pacman._x, pacman._y+1] != '#')
               {
                  pacman._directionY = 1;
                  pacman._directionX = 0;
               }
               break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
               if(map[pacman._x, pacman._y-1] != '#')
               {
                  pacman._directionY = -1;
                  pacman._directionX = 0;
               }
               break;
         }
      }
      
      private void Move(char[,] map, ref Pacman pacman)
      {
         ConsoleColor defaultColor = Console.ForegroundColor;
         
         Console.SetCursorPosition(pacman._y, pacman._x);
         Console.Write(' ');

         if (map[pacman._x, pacman._y] == '@')
         {
            map[pacman._x, pacman._y] = ' ';
         }

         pacman._x += pacman._directionX;
         pacman._y += pacman._directionY;

         Console.SetCursorPosition(pacman._y, pacman._x);
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.Write('@');
         Console.ForegroundColor = defaultColor;
      }

      private void CollectDots(char[,] map,Pacman pacman, User user)
      {
         if(map[pacman._x, pacman._y] == '.')
         {
            map[pacman._x, pacman._y] = ' ';
            pacman.CollectedDots++;
            user.CollectedDotsToPoints();
         }
      }

      public void CheckedMove(char[,] map,ref Pacman pacman, User user)
      {
         if(Console.KeyAvailable)
         {
            ConsoleKeyInfo key = Console.ReadKey(true);
            pacman.ChangeDirection(key, map, ref pacman);
            if(key.Key == ConsoleKey.P)
            {
               Console.SetCursorPosition(0, map.GetLength(0) + 5);
               Console.WriteLine("Чтобы продолжить игарть нажмите \"R\"");
               while(true)
               {
                  ConsoleKeyInfo outKey = Console.ReadKey(true);
                  if(outKey.Key == ConsoleKey.R)
                  {  
                     Console.SetCursorPosition(0, map.GetLength(0) + 5);
                     Console.Write("                                          ");
                     break;
                  }
               }
            }
         }
      
         if(map[pacman._x + pacman._directionX,pacman._y + pacman._directionY] != '#' && (pacman._directionX+pacman._directionY != 0))
         {
            Move(map, ref pacman);

            CollectDots(map, pacman, user);
         }
      }

      public void CheckingIsPackmanAlive(Pacman pacman, Ghost[] ghosts)
      {
         for (int i = 0; i < ghosts.Length; i++)
         {
            if(pacman._x == ghosts[i].X && pacman._y == ghosts[i].Y)
            {
               isAlive = false;
            }
         }
      }
   }
}