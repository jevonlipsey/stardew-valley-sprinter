using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.Network;
using StardewValley.TerrainFeatures;

namespace Sprinter
{
    public class ModConfig
    {
        public KeybindList SprinterButton = KeybindList.Parse("Space, RightShoulder");
        public decimal initialSpeedBoost = 3.5m;
        public float staminaCost = 0;
    }

    public class SprinterMod : Mod
    {
        private const decimal Deceleration = 0.1m;
        private const string BuffId = "Moonbaseboss.SprinterMod_Roll";
        private const string IconPath = "assets/slides.png";

        private readonly PerScreen<bool> isRolling = new(() => false);
        private readonly PerScreen<decimal> speedBuff = new(() => 0m);
        public ModConfig config;

        public override void Entry(IModHelper helper)
        {
            config = this.Helper.ReadConfig<ModConfig>();
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
            helper.Events.GameLoop.UpdateTicking += this.OnUpdateTicking;
            helper.Events.Player.Warped += this.OnWarped;
            this.Helper.ModContent.Load<Texture2D>(IconPath);
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsPlayerFree)
                return;

            if (!IsRolling() && config.SprinterButton.JustPressed() && CanRoll())
            {
                Engage();
            }
            if (IsRolling() && config.SprinterButton.GetState() == SButtonState.Released)
            {
                Disengage();
            }
        }

        private void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
        {
            if (IsRolling())
            {
                Roll(e.Ticks % 6 == 0);
            }
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (IsRolling())
            {
                Disengage();
            }
        }

        private bool IsRolling() => isRolling.Value;
        private void SetIsRolling(bool rolling) => isRolling.SetValueForScreen(Context.ScreenId, rolling);
        private decimal SpeedBuff() => speedBuff.Value;
        private void SetSpeedBuff(decimal newBuff)
        {
            Buff buff = new(
                id: BuffId,
                displayName: "Sprinter",
                description: newBuff <= 0 ? "Looks like you need a push." : "",
                iconTexture: this.Helper.ModContent.Load<Texture2D>(IconPath),
                iconSheetIndex: IconIndex(newBuff),
                duration: Buff.ENDLESS,
                effects: new BuffEffects() { Speed = { (float)newBuff } }
            );
            Game1.player.applyBuff(buff);
            speedBuff.SetValueForScreen(Context.ScreenId, newBuff);
        }

        private void BurnStamina(float staminaCost)
        {
            if (staminaCost > 0)
                Game1.player.Stamina -= staminaCost;
        }

        private int IconIndex(decimal speed)
        {
            return speed switch
            {
                > 2 => 0,
                > 1 => 1,
                > 0 => 2,
                _ => 3
            };
        }

        private bool CanRoll()
        {
            return Game1.player.movedDuringLastTick()
                && !Game1.player.isRidingHorse()
                && !Game1.player.hasBuff(Buff.slimed)
                && !Game1.player.hasBuff(Buff.tipsy);
        }

        private void Engage()
        {
            NetPosition position = Game1.player.position;
            SetSpeedBuff(config.initialSpeedBoost);
            BurnStamina(config.staminaCost);
            SetIsRolling(true);
        }

        private void Roll(bool shouldDecrement)
        {
            if (shouldDecrement)
            {
                SetSpeedBuff(Math.Max(SpeedBuff() - Deceleration, -5));
                SpawnDirtEffect();
            }
            else
            {
                SetSpeedBuff(SpeedBuff());
            }
        }

        private void SpawnDirtEffect()
        {
            int direction = Game1.player.FacingDirection;
            Vector2 offset = direction switch
            {
                0 => new Vector2(0, 32),
                1 => new Vector2(-32, 0),
                2 => new Vector2(0, -32),
                3 => new Vector2(32, 0),
                _ => Vector2.Zero
            };

            Vector2 position = Game1.player.Position + offset;

            TemporaryAnimatedSprite sprite = new(
                textureName: "TileSheets\animations",
                sourceRect: new Rectangle(0, 960, 64, 64),
                animationInterval: 80f,
                animationLength: 8,
                numberOfLoops: 1,
                position: position,
                flicker: false,
                flipped: false
            )
            {
                scale = 0.75f,
                layerDepth = 1f,
                alphaFade = 0.01f,
                motion = new Vector2(0, -0.3f),
                acceleration = new Vector2(0, -0.02f)
            };

            Game1.currentLocation.TemporarySprites.Add(sprite);
        }

        private void Disengage()
        {
            SetIsRolling(false);
            Buff buff = new(
                id: BuffId,
                displayName: "Sprinter bro",
                duration: 1
            );
            Game1.player.applyBuff(buff);
            Game1.player.completelyStopAnimatingOrDoingAction();
        }
    }
}
