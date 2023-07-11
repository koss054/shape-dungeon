namespace ShapeDungeon.Exceptions
{
    public class NoActiveCombatException : Exception
    {
        public int StatusCode { get; set; }

        public NoActiveCombatException(string message)
            : base(message)
        {
            StatusCode = 454;
        }
    }
}
