using System.Collections.Generic;
using BaseLibrary.Utility;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace TapeMeasure;

public class TapeMeasure : Mod
{
}

public class TapeMeasureSystem : ModSystem
{
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int SmartCursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Smart Cursor Targets"));
		if (SmartCursorIndex != -1) layers.Insert(SmartCursorIndex, new LegacyGameInterfaceLayer("TapeMeasure: Measure", DrawMeasures));
	}

	private static bool DrawMeasures()
	{
		foreach (Items.TapeMeasure tapeMeasure in Main.LocalPlayer.inventory.OfModItemType<Items.TapeMeasure>())
		{
			if (tapeMeasure.start != Point16.NegativeOne && tapeMeasure.end == Point16.NegativeOne && Main.LocalPlayer.HeldItem == tapeMeasure.Item)
			{
				Utility.DrawOutline(Main.spriteBatch, tapeMeasure.start, new Point16(Player.tileTargetX, Player.tileTargetY), tapeMeasure.Color);
				Utility.DrawMeasureText(Main.spriteBatch, tapeMeasure.start, new Point16(Player.tileTargetX, Player.tileTargetY), tapeMeasure.Color, 0.8f);
			}

			if (tapeMeasure.start != Point16.NegativeOne && tapeMeasure.end != Point16.NegativeOne)
			{
				Utility.DrawOutline(Main.spriteBatch, tapeMeasure.start, tapeMeasure.end, tapeMeasure.Color);
				Utility.DrawMeasureText(Main.spriteBatch, tapeMeasure.start, tapeMeasure.end, tapeMeasure.Color, 0.8f);
			}
		}

		return true;
	}
}