namespace Game
{
   public sealed class Stats
   {
      private bool drawOneTime = true;
      private int _currentProgressBarValue = 0;
      private int _currentUserPosition;
      private Dictionary<string, int> CurrentLeaderboard = new Dictionary<string, int>();
      private Dictionary<string, int> MaximumLeaderboard = new Dictionary<string, int>();
      public void ShowStats(char[,] map, Pacman pacman, User user, Map newMap)
      {
         Console.SetCursorPosition(map.GetLength(1) + 1, 0);
         Console.Write($"На этой карте собрано {pacman.CollectedDots}/{newMap.AllDots} точек.");
         Console.SetCursorPosition(map.GetLength(1) + 1, 1);
         Console.Write($"Ваши очки: {user.Points}");
         Console.SetCursorPosition(map.GetLength(1) + 1, 3);
         DrawProgressBar(ConsoleColor.White, pacman, map, newMap);
         AddToCurrentLeaderboard(pacman, user);
         ShowCurrentLeaderboard(map, user);
         AddToMaximumLeaderboard(pacman, user);
         ShowMaximumLeaderboard(map, user);

         Console.SetCursorPosition(0, map.GetLength(0) + 1);

         Console.WriteLine("Чтобы приостановить игру нажмите \"P\".");

      }

      private void DrawProgressBar(ConsoleColor color, Pacman pacman, char[,] map, Map newMap)
      {
         int _maxProgressBarValue = 10;
         ConsoleColor defaultColor = Console.BackgroundColor;
         bool firstTime = pacman.CollectedDots == 0;

         if((drawOneTime && pacman.CollectedDots == newMap.AllDots / 10) || 
            (!drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 2) || 
            (drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 3) || 
            (!drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 4) || 
            (drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 5) || 
            (!drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 6) || 
            (drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 7) || 
            (!drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 8) || 
            (drawOneTime && pacman.CollectedDots == (newMap.AllDots / 10) * 9) || 
            (!drawOneTime && pacman.CollectedDots == newMap.AllDots))
         {
            _currentProgressBarValue++;
            drawOneTime = !drawOneTime;
            string bar = "";

            for (int i = 0; i < _currentProgressBarValue; i++)
            {
               bar += " ";
            }
            
            Console.Write("[");
            Console.BackgroundColor = color;
            Console.Write(bar);
            Console.BackgroundColor = defaultColor;
   
            bar = "";
            for (int i = _currentProgressBarValue; i < _maxProgressBarValue; i++)
            {
               bar += " ";
            }
   
            Console.Write(bar + "]");
         }
         else if(firstTime)
         {
            string bar = "          ";
            Console.Write($"[{bar}]");
         }
      }

      private void ShowCurrentLeaderboard(char[,]map, User user)
      {
         Console.SetCursorPosition(map.GetLength(1) + 1, 5);
         ConsoleColor defaultColor = Console.ForegroundColor;
         int topFiveLeaders = 0;

         Console.WriteLine("Таблица Лидеров: ");
         foreach(var player in CurrentLeaderboard.OrderByDescending(value => value.Value))
         {
            if(topFiveLeaders < 5)
            {
               Console.SetCursorPosition(map.GetLength(1) + 1, 6 + topFiveLeaders);

               if(player.Key == user.Name)
               {
                  Console.ForegroundColor = ConsoleColor.Yellow;
                  _currentUserPosition = CurrentLeaderboard.OrderByDescending(value => value.Value).ToList().IndexOf(player) + 1;
               }
               int userPosition = CurrentLeaderboard.OrderByDescending(value => value.Value).ToList().IndexOf(player) + 1;
               Console.Write($"{userPosition} - {player.Key} набрал {player.Value} очков.           ");
               if(player.Key == user.Name)
               {
                  Console.ForegroundColor = defaultColor;
               }
               topFiveLeaders++;
            }
         }
      }

      private void AddToCurrentLeaderboard(Pacman pacman, User user)
      {
         bool alreadyUsed = false;
         foreach(var player in CurrentLeaderboard)
         {
            if(player.Key == user.Name)
               alreadyUsed = true;
         }
         if(!alreadyUsed && user.Name != null)
            CurrentLeaderboard.Add(user.Name, user.Points);
         else if(alreadyUsed && user.Name != null)
         {
            CurrentLeaderboard.Remove(user.Name);
            CurrentLeaderboard.Add(user.Name, user.Points);
         }
      }

      private void AddToMaximumLeaderboard(Pacman pacman, User user)
      {
         bool alreadyUsed = false;
         foreach(var player in MaximumLeaderboard)
         {
            if(player.Key == user.Name)
               alreadyUsed = true;
         }
         if(!alreadyUsed && user.Name != null)
         {
            StreamReader getUserRecord = new StreamReader($"UserInfo/UserRecord/{user.Name}");

            MaximumLeaderboard.Add(user.Name, Convert.ToInt32(getUserRecord.ReadLine()));
            getUserRecord.Close();
         }
         else if(alreadyUsed && user.Name != null)
         {
            if (MaximumLeaderboard[user.Name] < user.Points)
            {
               MaximumLeaderboard.Remove(user.Name);
               MaximumLeaderboard.Add(user.Name, user.Points);
            }
         }
         
      }

      private void ShowMaximumLeaderboard(char[,] map, User user)
      {
         Console.SetCursorPosition(map.GetLength(1) + 50, 5);
         ConsoleColor defaultColor = Console.ForegroundColor;
         int topFiveLeaders = 0;

         Console.Write("Таблица лидеров по максимально собранным очкам: ");
         foreach(var player in MaximumLeaderboard.OrderByDescending(value => value.Value))
         {
            if(topFiveLeaders < 5)
            {
               Console.SetCursorPosition(map.GetLength(1) + 50, 6 + topFiveLeaders);

               if(player.Key == user.Name)
               {
                  Console.ForegroundColor = ConsoleColor.Yellow;
               }
               int maximumuserPosition = MaximumLeaderboard.OrderByDescending(value => value.Value).ToList().IndexOf(player) + 1;
               Console.Write($"{maximumuserPosition} - {player.Key} набрал {player.Value} очков.           ");
               if(player.Key == user.Name)
               {
                  Console.ForegroundColor = defaultColor;
               }
               topFiveLeaders++;
            }
         }
      }

      public void GameResult(Pacman pacman, char[,] map)
      {
         Console.SetCursorPosition(0, map.GetLength(0) + 3);
         if(_currentUserPosition == 1)
         {
            Console.WriteLine("Победа! Вы заняли первое место в лидер-борде!");
         }
         else
         {
            Console.WriteLine("Вы проиграли! Ваше место в лидер-борде - {0}.",_currentUserPosition);
         }
      }
   
      public void ResetBar()
      {
         _currentProgressBarValue = 0;
      }
   }
}