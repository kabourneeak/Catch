using Catch.Base;
using Catch.Services;

namespace Catch.Graphics
{
    /// <summary>
    /// A <see cref="SpriteIndicator"/> which uses the position and rotation of its host <see cref="IAgent"/> when drawing
    /// </summary>
    public class AgentPositionSpriteIndicator : SpriteIndicator
    {
        private readonly IAgent _host;

        public AgentPositionSpriteIndicator(IConfig config, SpriteProvider spriteProvider, IAgent host) : base(config, spriteProvider)
        {
            _host = host;
        }

        public override void Draw(DrawArgs drawArgs)
        {
            Position = _host.Position;
            Rotation = _host.Rotation;

            base.Draw(drawArgs);
        }
    }
}
