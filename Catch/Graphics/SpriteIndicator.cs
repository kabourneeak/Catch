using System;
using Catch.Base;
using Catch.Services;

namespace Catch.Graphics
{
    /// <summary>
    /// An indicator that displays its configured sprite
    /// </summary>
    public class SpriteIndicator : IIndicator
    {
        private static readonly string CfgSpriteName = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgSpriteName));
        private static readonly string CfgUseRotation = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgUseRotation));
        private static readonly string CfgLayer = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgLayer));
        private static readonly string CfgLevelOfDetail = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgLevelOfDetail));

        public ISprite Sprite { get; }

        public bool UseRotation { get; }

        public SpriteIndicator(IConfig config, SpriteProvider spriteProvider)
        {
            UseRotation = config.GetBool(CfgUseRotation, false);

            var spriteName = config.GetString(CfgSpriteName);
            Sprite = spriteProvider.GetSprite(spriteName);

            var strCfgLayer = config.GetString(CfgLayer);
            if (Enum.TryParse(strCfgLayer, out DrawLayer layer))
            {
                Layer = layer;
            }
            else
            {
                throw new ArgumentException($"Could not parse {strCfgLayer} as DrawLayer");
            }

            var strCfgLod = config.GetString(CfgLevelOfDetail);
            if (Enum.TryParse(strCfgLod, out DrawLevelOfDetail lod))
            {
                LevelOfDetail = lod;
            }
            else
            {
                throw new ArgumentException($"Could not parse {strCfgLod} as DrawLevelOfDetail");
            }
        }

        public void CreateResources(CreateResourcesArgs args)
        {
            // do nothing
        }

        public void DestroyResources()
        {
            // do nothing
        }

        public void Draw(DrawArgs drawArgs, float rotation)
        {
            if (UseRotation)
            {
                drawArgs.PushRotation(rotation);

                Sprite.Draw(drawArgs);

                drawArgs.Pop();
            }
            else
            {
                Sprite.Draw(drawArgs);
            }
        }

        public DrawLayer Layer { get; }

        public DrawLevelOfDetail LevelOfDetail { get; }
    }
}
