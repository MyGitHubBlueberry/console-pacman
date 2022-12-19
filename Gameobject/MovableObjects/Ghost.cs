namespace Game
{
   public sealed class Ghost : GameObject
   {
      private int _directionX, _directionY;
      public int X { get { return _x; } }
      public int Y { get { return _y; } }
      public ConsoleColor GhostColor { get; private set; }
      public Ghost(int x, int y, ConsoleColor ghostColor) : base(x, y) 
      {
         GhostColor = ghostColor;
      }
      private void ChangeDirection(ref Ghost ghost)
      {
         Random rand = new Random();
         switch (rand.Next(1, 5))
         {
            case 1:
               ghost._directionX = 1;
               ghost._directionY = 0;
               break;
            case 2:
               ghost._directionX = -1;
               ghost._directionY = 0;
               break;
            case 3:
               ghost._directionX = 0;
               ghost._directionY = 1;
               break;
            case 4:
               ghost._directionX = 0;
               ghost._directionY = -1;
               break;
         }

      }

      private void Move(char[,] map, ref Ghost ghost)
      {
         ConsoleColor defaultColor = Console.ForegroundColor;
         
         Console.SetCursorPosition(ghost._y, ghost._x);
         if (map[ghost._x, ghost._y] == '$')
         {
            map[ghost._x, ghost._y] = ' ';
         }
         Console.Write(map[ghost._x, ghost._y]);

         ghost._x += ghost._directionX;
         ghost._y += ghost._directionY;
         
         Console.SetCursorPosition(ghost._y, ghost._x);
         Console.ForegroundColor = ghost.GhostColor;
         Console.Write('$');
         Console.ForegroundColor = defaultColor;
      }
      
      public void CheckedMove(char[,] map, ref Ghost[] ghosts)
      {
         for (int i = 0; i < ghosts.Length; i++)
         {
            if(map[ghosts[i]._x + ghosts[i]._directionX, ghosts[i]._y + ghosts[i]._directionY] != '#' && (ghosts[i]._directionX + ghosts[i]._directionY)!= 0)
            {
               Move(map, ref ghosts[i]);
            }
            else
            {
               ChangeDirection(ref ghosts[i]);
            }
         }
      }
   }
}