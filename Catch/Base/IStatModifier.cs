namespace Catch.Base
{
    public interface IStatModifier<in T>
    {
        /// <summary>
        /// The internal multiplier for the intensity of this mod. A value of 1.0 is nominal.
        /// </summary>
        float Intensity { get; }

        /// <summary>
        /// The priority for this mod, which determines the order in which mods are cummulatively
        /// applied
        /// </summary>
        ModifierPriority Priority { get; }

        void Apply(T statModel);
    }
}