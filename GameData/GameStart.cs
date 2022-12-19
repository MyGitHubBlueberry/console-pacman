namespace Game
{
   public sealed class GameStart
   {
      private StreamReader? readFromUserInfo = null;
      private StreamWriter? writeToUserInfo = null;
      private string _gameIntroduction = "Игра \"Пакмен\": \n\nПакмен - \"@\"; \nПризраки - \"$\"; \nГраницы карты - \"#\"; \nКлюч - \"*\"; \nТочка - \".\";\n\nВаша задача набрать как можно больше очков (1 очко за точку и 200 за каждый ключ), \nизбегая столкновения с \"призраками\".";
      private Dictionary<string, string> UserLoginAndPassword = new Dictionary<string, string>();
      private (int Left, int Top) _cursorPosition;
      public GameStart() { }

      public void GameIntrodution()
      {
         Console.Clear();
         Console.CursorVisible = false;

         
         System.Console.Write(_gameIntroduction);
         System.Console.WriteLine("\n\nПеред началом рекомендуется развернуть консоль на весь экран. \nСлишком маленький размер может быть причиной некорректной отрисовки карты.");
         System.Console.Write("\nНажмите любую клавишу, чтобы начать");

         _cursorPosition = Console.GetCursorPosition();

         FakeLoading(_cursorPosition.Left, _cursorPosition.Top);

         Console.Clear();
      }

      public void PreLoad()
      {
         string? userLongin;
         string? userPassword;
         bool userNotExist = true;

         try
         {
            readFromUserInfo = new StreamReader("UserInfo/user_info");
            while (!readFromUserInfo.EndOfStream)
            {
               userLongin = readFromUserInfo.ReadLine();
               userPassword = readFromUserInfo.ReadLine();

               if (!String.IsNullOrWhiteSpace(userLongin) && !String.IsNullOrWhiteSpace(userPassword))
               {
                  if (UserLoginAndPassword != null && UserLoginAndPassword.Count != 0)
                  {
                     foreach (var existLogin in UserLoginAndPassword.Keys)
                     {
                        if (existLogin == userLongin)
                        {
                           userNotExist = false;
                           break;
                        }
                     }
                     if (userNotExist)
                     {
                        UserLoginAndPassword.Add(userLongin, userPassword);
                     }
                     userNotExist = true;
                  }
                  else if (UserLoginAndPassword != null)
                  {
                     UserLoginAndPassword.Add(userLongin, userPassword);
                  }
               }
            }
         }
         catch (DirectoryNotFoundException) 
         {
            System.Console.WriteLine("Папка не найдена. По пути 'bin/Debug/net6.0/UserInfo' должен находиться файл user_info. \nЕсли вы пытаетесь запуститься из встроенного терминала, перенесите папки 'Maps' и 'UserInfo' в корень папки с игрой.");
            
            throw new DirectoryNotFoundException();
         }
         finally
         {
            if (readFromUserInfo != null)
            {
               readFromUserInfo.Close();
            }
         }
      }

      public void Menu(out User? user)
      {
         user = null;
         tryAgain:

         FileStream? userRecord = null;
         string? userLogin;
         string? userPassword;
         string? tryToLogin;
         string? tryToPassword;

         Console.Clear();
         Console.WriteLine("\"R\" - зарегестрироваться; \n\"L\" - войти; \n\"I\" - инфо;");
         ConsoleKeyInfo logOrSignIn = Console.ReadKey(true);

         switch (logOrSignIn.Key)
         {
            case ConsoleKey.L:
               Console.Clear();
               System.Console.Write("Введите ваше имя пользователя, затем нажмите \"Enter\": ");
               tryToLogin = Console.ReadLine();

               System.Console.Write("Введите ваш пароль, затем нажмите \"Enter\": ");
               tryToPassword = Console.ReadLine();

               if (!String.IsNullOrWhiteSpace(tryToLogin) && !String.IsNullOrWhiteSpace(tryToPassword))
               {
                  try
                  {
                     if (UserLoginAndPassword[tryToLogin] == tryToPassword)
                     {
                        user = new User(tryToLogin);
                        Console.Clear();
                        break;
                     }
                     else
                     {
                        System.Console.WriteLine("Имя пользователя или пароль введено не верно.");
                        Console.ReadKey();
                        Console.Clear();
                        goto tryAgain;
                     }
                  }
                  catch (Exception)
                  {
                     System.Console.WriteLine("Нет такого пользователя.");
                     Console.ReadKey();
                     Console.Clear();
                     goto tryAgain;
                  }
               }
               if (String.IsNullOrWhiteSpace(tryToLogin) || String.IsNullOrWhiteSpace(tryToPassword))
               {
                  Console.WriteLine("Имя пользователя и пароль не могут быть пустыми!");
                  Console.Clear();
                  goto tryAgain;
               }
               break;
            case ConsoleKey.R:
               Console.Clear();
               System.Console.Write("Придумайте имя пользователя, затем нажмите \"Enter\": ");
               userLogin = Console.ReadLine();

               System.Console.Write("Придумайте пароль, затем нажмите \"Enter\": ");
               userPassword = Console.ReadLine();

               if (!String.IsNullOrWhiteSpace(userLogin) && !String.IsNullOrWhiteSpace(userPassword))
               {
                  bool addToTheEnd = true;
                  string? savedLogin;
                  string? savedPassword;
                  bool UserIsExist = false;

                  if (userLogin.Length > 10)
                  {
                     System.Console.WriteLine("Имя пользователя не может быть больше 10 символов.");
                     Console.ReadKey();
                     Console.Clear();
                     goto tryAgain;
                  }
                  try
                  {
                     writeToUserInfo = new StreamWriter("UserInfo/user_info", addToTheEnd);
                     readFromUserInfo = new StreamReader("UserInfo/user_info");

                     while (!readFromUserInfo.EndOfStream)
                     {
                        savedLogin = readFromUserInfo.ReadLine();
                        savedPassword = readFromUserInfo.ReadLine();
                        readFromUserInfo.ReadLine();
                        if (savedLogin == userLogin)
                        {
                           UserIsExist = true;
                           break;
                        }
                     }
                     if (UserIsExist)
                     {
                        System.Console.WriteLine("Такой пользователь уже есть. Выберете другое имя или войдите под этим.");
                        Console.ReadKey();
                        Console.Clear();
                        goto tryAgain;
                     }
                     else
                     {
                        UserLoginAndPassword.Add(userLogin, userPassword);
                        writeToUserInfo.WriteLine(userLogin);
                        writeToUserInfo.WriteLine(userPassword);
                        writeToUserInfo.Flush();

                        userRecord = new FileStream($"UserInfo/UserRecord/{userLogin}", FileMode.OpenOrCreate);
                        writeToUserInfo = new StreamWriter($"UserInfo/UserRecord/{userLogin}");
                        writeToUserInfo.Write(0);
                        writeToUserInfo.Flush();
                     }
                  }
                  catch (Exception exc)
                  {
                     System.Console.WriteLine("Ошибка ввода-вывода:\n " + exc.Message);
                     Console.ReadKey();
                     Console.Clear();
                     goto tryAgain;
                  }
                  finally
                  {
                     if (writeToUserInfo != null)
                     {
                        writeToUserInfo.Close();
                     }
                     if (readFromUserInfo != null)
                     {
                        readFromUserInfo.Close();
                     }
                     if (userRecord != null)
                     {
                        userRecord.Close();
                     }
                  }

                  System.Console.WriteLine("Теперь войдите под иминенм которое придумали.");
                  Console.ReadKey();
                  Console.Clear();
                  goto tryAgain;
               }
               if (String.IsNullOrWhiteSpace(userLogin) || String.IsNullOrWhiteSpace(userPassword))
               {
                  System.Console.WriteLine("Имя пользователя и пароль не могут быть пустыми.");
                  Console.ReadKey();
                  Console.Clear();
                  goto tryAgain;
               }
               break;
            case ConsoleKey.I:
               Console.Clear();
               System.Console.WriteLine(_gameIntroduction);
               System.Console.WriteLine("Пакменом можно управлять стрелочками на клавиатуре или клавишами wasd; \nКлюч появляется когда все точки на карте будут собраны, собрав его вы попадёте на следующую карту.\n\nВ таблице лидеров отображаются игроки, которые играли с вашего устройсва во время текущего сеанса. \nЧтобы пользователь отображался в таблице лидеров попросите его сиграть.\n\nВы можете сделать собственную карту и добавить её и игру. \nДля этого создайте в папке 'bin/Debug/net6.0/Maps' новый файл. Вы можете его назвать как угодно, \nглавное, чтобы названия файлов в папке не повтарялись.Для удобства скопируйте \nсодержимое любого из этой папки в ваш файл, главное, чтобы карта отсавалась размером \n39# в длину и 11# в высоту, чтобы не вызвать случайных ошибок. На ней должны быть \nотмечены 4 призрака и один пакмен. \nПосле этого добавьте название вашего файла в массив строк в файле Program.cs:");

               System.Console.WriteLine("\nstring[] availibleMaps = { \"map\", \"map2\", \"map4\" , \"Название вашего файла\"};");

               System.Console.Write("\n\nНажмите любую клавишу, чтобы вернуться");
               _cursorPosition = Console.GetCursorPosition();
               
               FakeLoading(_cursorPosition.Left, _cursorPosition.Top);
               goto tryAgain;
            default:
               goto tryAgain;
         }
      }

      public void Load(out char[,] map, string[] availibleMaps, out Pacman pacman, out Ghost[] ghosts, Map newMap)
      {
         string waitText = "Нажмите любую клавишу, чтобы начать";

         try
         {
            map = newMap.ChooseMap(availibleMaps);
         }
         catch (DirectoryNotFoundException) 
         {
            System.Console.WriteLine("Папка не найдена. По пути 'bin/Debug/net6.0/Maps' должны находиться файлы карт. \nЕсли вы пытаетесь запуститься из встроенного терминала, перенесите папки 'Maps' и 'UserInfo' в корень папки с игрой.");
            
            throw new DirectoryNotFoundException();
         }

         newMap.DrawMap(map);
         newMap.FindeCharacters(map, out pacman, out ghosts);

         Console.SetCursorPosition(0, map.GetLength(0) + 1);
         Console.Write(waitText);
         _cursorPosition = Console.GetCursorPosition();
         FakeLoading(_cursorPosition.Left, _cursorPosition.Top);

         Console.SetCursorPosition(0, map.GetLength(0) + 1);
         System.Console.WriteLine("                                        ");
      }

      private void FakeLoading(int left, int top)
      {
         while (!Console.KeyAvailable)
         {
            Console.SetCursorPosition(left, top);
            for (int i = 0; i < 3; i++)
            {
               Console.Write(".");
               System.Threading.Thread.Sleep(1000);
               if (Console.KeyAvailable)
               {
                  break;
               }
            }
            Console.SetCursorPosition(left, top);
            Console.Write("   ");
            System.Threading.Thread.Sleep(1000);
         }
      }
   }
}