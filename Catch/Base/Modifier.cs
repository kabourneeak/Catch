namespace Catch.Base
{
    public abstract class Modifier
    {
        #region Properties

        public int Level { get; set; }

        public float Intensity { get; set; }

        public float Duration { get; set; }

        #endregion

        #region Base

        protected Modifier(IAgent agent)
        {
            Agent = agent;
            NeedsApplyToBase = true;
            IsActive = true;

            Level = 1;
            Intensity = 1.0f;
            Duration = -1.0f;
        }

        public bool IsActive { get; protected set; }

        public ModifierPriority Priority { get; protected set; }
        
        public IAgent Agent { get; protected set; }

        #endregion

        #region Behaviour

        public abstract void Update(float ticks);

        /// <summary>
        /// Adjust the specs of the associated Agent. NeedsApplyToBase must return false after this is called.
        /// </summary>
        public virtual void ApplyToBase()
        {
            // do nothing
        }

        public bool NeedsApplyToBase { get; set; }

        public virtual bool ApplyToModifier(Modifier incomingMod)
        {
            // do nothing
            return true;
        }

        public virtual bool ApplyToAttack(AttackModel outgoingAttack)
        {
            // do nothing
            return true;
        }

        public virtual bool ApplyToHit(AttackModel incomingAttack)
        {
            // do nothing
            return true;
        }

        #endregion

        #region Sandbox Methods

        #endregion
    }
}