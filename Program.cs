namespace Game
{
   sealed class Program
   {
      static void Main(string[] args)
      {
         start:
         User? user;
         Program program = new Program();
         Map newMap = new Map();
         Stats stats = new Stats();
         GameStart gameStart= new GameStart();
         gameStart.GameIntrodution();
         try
         {
            gameStart.PreLoad();
         }
         catch(DirectoryNotFoundException)
         {
            goto closeProgram;
         }
         
         gameStart.Menu(out user);

         generateNewMap:
         string[] availibleMaps = { "map", "map2", "map4" };
         char[,] map;
         Pacman pacman;
         Ghost[] ghosts;

         Key pacmanKey = new Key(0,0);
         bool isPlaying = true;
         bool firstAttention = true;
         bool pacmanKeyIsGenerated = false;

         try
         {
            gameStart.Load(out map, availibleMaps, out pacman, out ghosts, newMap);
         }
         catch(DirectoryNotFoundException)
         {
            goto closeProgram;
         }

         if(user != null)
         while(isPlaying)
         {
            stats.ShowStats(map, pacman, user, newMap);
            if(pacman.isAlive)
               pacman.CheckedMove(map, ref pacman, user);
            stats.ShowStats(map, pacman, user, newMap);

            pacman.CheckingIsPackmanAlive(pacman, ghosts);
          
            ghosts[0].CheckedMove(map, ref ghosts);
            pacman.CheckingIsPackmanAlive(pacman, ghosts);

            Thread.Sleep(200);

            if (pacman.isAlive && pacman.CollectedDots == newMap.AllDots)
            {
               if(pacmanKeyIsGenerated)
               {
                  pacmanKey.Shine();
                  pacmanKey.ChekingIsKeyCollected(pacman, user);
               }
               else
               {
                  Key.GenerateKey(pacman, map, out pacmanKey);
                  pacmanKeyIsGenerated = true;
               }
            }
            if (pacmanKey.PacmanKeyCollected)
            {
               stats.ResetBar();
               Console.Clear();
               goto generateNewMap;
            }
            isPlaying = pacman.isAlive;
         }
         stats.GameResult(pacman,map);

         Console.WriteLine("\nЧтобы играть снова нажмите \"P\". Если хотите выйти, нажмите \"Q\".");

         tryagain:

         ConsoleKeyInfo key = Console.ReadKey(true);
         switch(key.Key)
         {
            case ConsoleKey.Q:
               break;
            case ConsoleKey.P:
               goto start;
            default:
               if(firstAttention)
               {
                  Console.WriteLine("Нажми одну из предложенных кнопок.");
                  firstAttention = false;
               }
               goto tryagain;
         }
         closeProgram:
         Console.ReadKey();
      }
   }
}