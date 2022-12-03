namespace Game
{
   public sealed class User
   {
      public int Points { get; private set; }
      public string Name{ get; private set; }
      public User(string name) 
      {
         Name = name;
      }
      public void CollectedDotsToPoints()
      {
         Points++;
      }

      public void CollectedKeyToPoints()
      {
         Points += 200;
      }
   }
}