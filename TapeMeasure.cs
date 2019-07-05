using BaseLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace TapeMeasure
{
	public class TapeMeasure : Mod
	{
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int SmartCursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Smart Cursor Targets"));
			if (SmartCursorIndex != -1)
			{
				layers.Insert(SmartCursorIndex, new LegacyGameInterfaceLayer(
					"TapeMeasure: Measure",
					delegate
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
					}));
			}
		}
	}
}