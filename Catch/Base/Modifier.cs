namespace Catch.Base
{
    public abstract class Modifier
    {
        public IAgent Agent { get; protected set; }

        protected Modifier(IAgent agent)
        {
            Agent = agent;
            NeedsApply = true;
            IsActive = true;
        }

        /// <summary>
        /// Adjust the specs of the associated Agent. NeedsApply must return false after this is called.
        /// </summary>
        public abstract void Apply();

        public abstract void Update(float ticks);

        public bool NeedsApply { get; set; }

        public bool IsActive { get; protected set; }

        public ModifierPriority Priority { get; protected set; }
    }
}