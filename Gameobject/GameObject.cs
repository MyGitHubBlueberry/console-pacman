namespace Game
{
   public abstract class GameObject
   {
      protected int _x, _y;
      public GameObject(int x, int y)
      {
         _x = x;
         _y = y;
      }
   }
}