using System;
using System.Numerics;
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
        private static readonly string CfgUseTranslation = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgUseTranslation));
        private static readonly string CfgUseRotation = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgUseRotation));
        private static readonly string CfgLayer = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgLayer));
        private static readonly string CfgLevelOfDetail = ConfigUtils.GetConfigPath(nameof(SpriteIndicator), nameof(CfgLevelOfDetail));

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public DrawLayer Layer { get; protected set; }

        public DrawLevelOfDetail LevelOfDetail { get; protected set; }

        protected ISprite Sprite { get; }

        protected bool UseTranslation { get; set; }

        protected bool UseRotation { get; set; }

        public SpriteIndicator(IConfig config, SpriteProvider spriteProvider)
        {
            UseTranslation = config.GetBool(CfgUseTranslation, false);
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

        public virtual void Draw(DrawArgs drawArgs)
        {
            if (UseTranslation || UseRotation)
                drawArgs.PushTranslation(this.Position);

            if (UseRotation)
                drawArgs.PushRotation(this.Rotation);

            if (UseTranslation || UseRotation)
                Sprite.Draw(drawArgs);
            else
                Sprite.Draw(drawArgs, Position);

            if (UseRotation)
                drawArgs.Pop();

            if (UseTranslation || UseRotation)
                drawArgs.Pop();
        }
    }
}
