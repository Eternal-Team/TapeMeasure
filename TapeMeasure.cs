using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TapeMeasure.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;
using TheOneLibrary.Base;
using TheOneLibrary.Base.UI;
using static TheOneLibrary.Utils.Utility;

namespace TapeMeasure
{
	public class TapeMeasure : Mod
	{
		[Null] public static TapeMeasure Instance;

		[Null] public static Texture2D TapeMeasureGlow;

		public Dictionary<ModItem, GUI> TapeUI = new Dictionary<ModItem, GUI>();

		public LegacyGameInterfaceLayer TapeInterface;

		public override void Load()
		{
			Instance = this;

			if (!Main.dedServ)
			{
				TapeMeasureGlow = ModLoader.GetTexture("TapeMeasure/Items/TapeMeasureItem_Glow");

				TapeInterface = new LegacyGameInterfaceLayer("TapeMeasure: UI", TapeUI.Values.Draw, InterfaceScaleType.UI);
			}
		}

		public override void Unload()
		{
			UnloadNullableTypes();
		}

		public override void PreSaveAndQuit()
		{
			TapeUI.Clear();
		}

		public override void PostUpdateInput()
		{
			Keys[] pressedKeys = Main.keyState.GetPressedKeys();

			if (pressedKeys.Contains(Keys.RightShift) && Main.mouseRight && Main.mouseRightRelease)
			{
				if (Main.LocalPlayer.HeldItem.type == ItemType<TapeMeasureItem>())
				{
					TapeMeasureItem data = (TapeMeasureItem)Main.LocalPlayer.HeldItem.modItem;

					if (data != null)
					{
						data.start = Point16.Zero;
						data.end = Point16.Zero;
					}
				}
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int HotbarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
			int SmartCursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Smart Cursor Targets"));

			if (HotbarIndex != -1) layers.Insert(HotbarIndex + 1, TapeInterface);

			if (SmartCursorIndex != -1)
			{
				foreach (Item item in Main.LocalPlayer.inventory.Where((x, i) => i < 10 && x.type == ItemType<TapeMeasureItem>()).Concat(Main.LocalPlayer.HeldItem.type == ItemType<TapeMeasureItem>() ? new[] {Main.LocalPlayer.HeldItem} : new Item[] { }))
				{
					layers.Insert(SmartCursorIndex, new LegacyGameInterfaceLayer(
						"TapeMeasureItem: Measure",
						delegate
						{
							TapeMeasureItem data = (TapeMeasureItem)item.modItem;

							if (data != null && data.start != Point16.Zero && data.end == Point16.Zero && Main.LocalPlayer.HeldItem == data.item)
							{
								Main.spriteBatch.DrawOutline(data.start, MouseToWorldPoint(), data.color.Value, 2);
								Utility.DrawMeasureText(data.start, MouseToWorldPoint(), data.color.Value, 0.7f);
							}

							if (data != null && data.start != Point16.Zero && data.end != Point16.Zero)
							{
								Main.spriteBatch.DrawOutline(data.start, data.end, data.color.Value, 2);
								Utility.DrawMeasureText(data.start, data.end, data.color.Value, 0.7f);
							}

							return true;
						}));
				}
			}
		}
	}
}