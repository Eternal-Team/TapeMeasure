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
		foreach (Content.TapeMeasure tapeMeasure in Main.LocalPlayer.inventory.OfModItemType<Content.TapeMeasure>())
		{
			if (tapeMeasure.start == Point16.NegativeOne || tapeMeasure.end == Point16.NegativeOne) continue;

			if (tapeMeasure.Mode == Content.TapeMeasure.MeasurementMode.Area)
			{
				Utility.DrawOutline(Main.spriteBatch, tapeMeasure.start, tapeMeasure.end, tapeMeasure.Color);
				Utility.DrawMeasureText(Main.spriteBatch, tapeMeasure.start, tapeMeasure.end, tapeMeasure.Color, 0.8f);
			}
			else
			{
				Utility.DrawLine(Main.spriteBatch, tapeMeasure.start, tapeMeasure.end, tapeMeasure.Color);
			}
		}

		return true;
	}
}