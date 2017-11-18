namespace Catch.Graphics
{
    public enum DrawLayer
    {
        // map design and other decorations
        Background,

        // player sprawl, tower presence
        Base,

        // shape and status of economic network
        EconomyNetwork,

        // transport activity along the economic network
        EconomyTransport,

        // tile agent design
        Tower,

        // status indicators for tile agents
        TowerStatus,

        // non-tile agent design
        Agent,

        // non-tile agent status indicators
        AgentStatus,

        // user interface hover and highlight effects
        Ui
    }
}