using Windows.Foundation;
using Windows.UI;
using Catch.Base;
using Catch.Services;
using Microsoft.Graphics.Canvas;

namespace Catch.Models
{
    public class BlockMob : PathMobAgent
    {
        private readonly IConfig _config;
        private readonly int _blockSize;
        private readonly Color _blockColour;

        public BlockMob(IPath path, IBehaviourComponent brain, IConfig config) : base(path, brain)
        {
            // This class can be massively generalized!  e.g., take in a config object and a mob name, and then fill out
            // all of the other details like health, speed, attack, defense, mod loadout, from config.

            _config = config;

            Velocity = 0.5f;

            // TODO copy down relevant config
            _blockSize = 10;
            _blockColour = Colors.Yellow;
        }

        public override string GetAgentType()
        {
            return typeof (BlockMob).Name;
        }

        public override void CreateResources(DrawArgs drawArgs)
        {
            // do nothing
        }

        public override void Draw(DrawArgs drawArgs)
        {
            var tilePos = Tile.Position;

            drawArgs.Ds.FillRectangle(new Rect(tilePos.X - _blockSize / 2, tilePos.Y - _blockSize / 2, _blockSize,
                    _blockSize), _blockColour);
        }
    }
}
