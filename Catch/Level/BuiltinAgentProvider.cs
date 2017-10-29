using System;
using Catch.Base;
using Catch.Graphics;
using Catch.Mobs;
using Catch.Services;
using Catch.Towers;

namespace Catch.Level
{
    /// <summary>
    /// Provides agents built into the program
    /// </summary>
    public class BuiltinAgentProvider : IAgentProvider
    {
        private readonly IConfig _config;
        private readonly IGraphicsManager _graphicsManager;
        private readonly ILabelProvider _labelProvider;

        public BuiltinAgentProvider(IConfig config, IGraphicsManager graphicsManager, ILabelProvider labelProvider)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _graphicsManager = graphicsManager ?? throw new ArgumentNullException(nameof(graphicsManager));
            _labelProvider = labelProvider ?? throw new ArgumentNullException(nameof(labelProvider));
        }

        public IExtendedAgent CreateAgent(string name, CreateAgentArgs args)
        {
            switch (name)
            {
                case nameof(GunTower):
                    return CreateGunTowerAgent(args);
                case nameof(EmptyTower):
                    return CreateEmptyTower(args);
                case nameof(BlockMob):
                    return CreateBlockMob(args);
                default:
                    throw new ArgumentException($"I don't know how to construct an agent with name {name}");
            }
        }

        private IExtendedAgent CreateGunTowerAgent(CreateAgentArgs args)
        {
            var agent = new GunTower(_graphicsManager.Resolve<GunTowerGraphicsProvider>(), args.Tile);
            agent.ExtendedStats.Team = args.Team;

            return agent;
        }

        private IExtendedAgent CreateEmptyTower(CreateAgentArgs args)
        {
            var agent = new EmptyTower(_graphicsManager.Resolve<EmptyTowerGraphicsProvider>(), args.Tile);
            agent.ExtendedStats.Team = args.Team;

            return agent;
        }

        private IExtendedAgent CreateBlockMob(CreateAgentArgs args)
        {
            var agent = new BlockMob(_config, _graphicsManager.Resolve<BlockMobGraphicsProvider>(), args.Path);
            agent.Tile = args.Tile;
            agent.ExtendedStats.Team = args.Team;

            return agent;
        }
    }
}
