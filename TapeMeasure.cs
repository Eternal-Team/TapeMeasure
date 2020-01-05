using BaseLibrary;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace TapeMeasure
{
	public class TapeMeasure : Mod
	{
		internal static Texture2D textureGlow;

		public override void Load()
		{
			if (!Main.dedServ) textureGlow = ModContent.GetTexture("TapeMeasure/Textures/TapeMeasure_Glow");
		}

		public override void Unload() => this.UnloadNullableTypes();

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int SmartCursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Smart Cursor Targets"));
			if (SmartCursorIndex != -1) layers.Insert(SmartCursorIndex, new LegacyGameInterfaceLayer("TapeMeasure: Measure", DrawMeasures));
		}

		private bool DrawMeasures()
		{
			foreach (Items.TapeMeasure tapeMeasure in Main.LocalPlayer.inventory.OfType<Items.TapeMeasure>())
			{
				if (tapeMeasure.start != Point16.Zero && tapeMeasure.end == Point16.Zero && Main.LocalPlayer.HeldItem == tapeMeasure.item)
				{
					Main.spriteBatch.DrawOutline(tapeMeasure.start, new Point16(Player.tileTargetX, Player.tileTargetY), tapeMeasure.Color.Value);
					Main.spriteBatch.DrawMeasureText(tapeMeasure.start, new Point16(Player.tileTargetX, Player.tileTargetY), tapeMeasure.Color.Value, 0.8f);
				}

				if (tapeMeasure.start != Point16.Zero && tapeMeasure.end != Point16.Zero)
				{
					Main.spriteBatch.DrawOutline(tapeMeasure.start, tapeMeasure.end, tapeMeasure.Color.Value);
					Main.spriteBatch.DrawMeasureText(tapeMeasure.start, tapeMeasure.end, tapeMeasure.Color.Value, 0.8f);
				}
			}

			return true;
		}
	}
}