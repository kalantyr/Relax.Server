namespace Relax.Models.Commands
{
    public abstract class CommandBase
    {
        public DateTimeOffset ServerTime { get; }

        protected CommandBase(DateTimeOffset serverTime)
        {

        }
    }
}
