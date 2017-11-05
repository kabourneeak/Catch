using System;
using System.Numerics;
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

        public override Vector2 Position
        {
            get => _host.Position;
            set => throw new InvalidOperationException();
        }

        public override float Rotation
        {
            get => _host.Rotation;
            set => throw new InvalidOperationException();
        }

        public AgentPositionSpriteIndicator(IConfig config, SpriteProvider spriteProvider, IAgent host) : base(config, spriteProvider)
        {
            _host = host;
        }
    }
}
