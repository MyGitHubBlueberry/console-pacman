namespace Game
{
   sealed class Key : GameObject
   {
      public bool PacmanKeyCollected{ get; private set; }
      public int X{ get { return _x; } }
      public int Y{ get { return _y; } }
      private enum _availibleKeyColors
      {
         Yellow,
         Red,
         Magenta
      }
      public Key(int x, int y) : base (x,y)
      {
         PacmanKeyCollected = false;
      }
      public static void GenerateKey(Pacman pacman, char[,] map, out Key pacmanKey)
      {
         Random randomKeyPositionX = new Random();
         Random randomKeyPositionY = new Random();
         int possiblekeyXPosition = randomKeyPositionX.Next(0, map.GetLength(0));
         int possiblekeyYPosition = randomKeyPositionY.Next(0, map.GetLength(1));

         if(map[possiblekeyXPosition, possiblekeyYPosition] != '#' && map[possiblekeyXPosition, possiblekeyYPosition] != '@' && map[possiblekeyXPosition, possiblekeyYPosition] != '&')
         {
            pacmanKey = new Key(possiblekeyXPosition, possiblekeyYPosition);
         }
         else
            GenerateKey(pacman, map, out pacmanKey);
      }
      public void Shine()
      { 
         _availibleKeyColors KeyColor;
         ConsoleColor defaultColor = Console.ForegroundColor;

         if(!PacmanKeyCollected)
         {
            for (KeyColor = _availibleKeyColors.Yellow; KeyColor <= _availibleKeyColors.Magenta; KeyColor++)
            {
               Thread.Sleep(10);
               switch(KeyColor)
               {
                  case _availibleKeyColors.Yellow:
                     Console.ForegroundColor = ConsoleColor.Yellow;
                     Console.SetCursorPosition(_y, _x);
                     Console.Write('*');
                     Console.ForegroundColor = defaultColor;
                     break;
                  case _availibleKeyColors.Red:
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.SetCursorPosition(_y, _x);
                     Console.Write('*');
                     Console.ForegroundColor = defaultColor;
                     break;
                  case _availibleKeyColors.Magenta:
                     Console.ForegroundColor = ConsoleColor.Magenta;
                     Console.SetCursorPosition(_y, _x);
                     Console.Write('*');
                     Console.ForegroundColor = defaultColor;
                     break;
               }
            } 
         }
      }
      public void ChekingIsKeyCollected(Pacman pacman, User user)
      {
         if(pacman.X == _x && pacman.Y == _y)
         {
            PacmanKeyCollected = true;
            user.CollectedKeyToPoints();
         }
      }
   }
}