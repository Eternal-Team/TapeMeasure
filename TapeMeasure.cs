using BaseLib.Utility;
using System.Collections.Generic;
using TapeMeasure.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace TapeMeasure
{
	public class TapeMeasure : Mod
	{
		public static TapeMeasure Instance;

		public TapeUI TapeUI;
		public UserInterface iTapeUI;

		public TapeMeasure()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			Instance = this;

			if (!Main.dedServ)
			{
				TapeUI = new TapeUI();
				TapeUI.Activate();
				iTapeUI = new UserInterface();
				iTapeUI.SetState(TapeUI);
			}
		}

		public override void Unload()
		{
			Instance = null;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			int SmartCursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Smart Cursor Targets"));

			if (MouseTextIndex != -1)
			{
				if (Main.playerInventory && TapeUI.visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
						"TapeMeasure: Configure",
						delegate
						{
							iTapeUI.Update(Main._drawInterfaceGameTime);
							TapeUI.Draw(Main.spriteBatch);

							return true;
						}, InterfaceScaleType.UI));
				}
				else TapeUI.visible = false;
			}

			if (SmartCursorIndex != -1)
			{
				for (int i = 0; i < 10; i++)
				{
					if (Main.LocalPlayer.inventory[i].type == ItemType<TapeMeasureItem>())
					{
						int pass = i;

						layers.Insert(SmartCursorIndex, new LegacyGameInterfaceLayer(
							"TapeMeasureItem: Measure",
							delegate
							{
								TapeMeasureItem data = (TapeMeasureItem)Main.LocalPlayer.inventory[pass].modItem;

								if (data != null && data.start != Point16.Zero && data.end == Point16.Zero)
								{
									Main.spriteBatch.DrawOutline(data.start, BaseLib.Utility.Utility.MouseToWorldPoint(), data.color, 2);
									Utility.DrawMeasureText(data.start, BaseLib.Utility.Utility.MouseToWorldPoint(), data.color, 0.7f);
								}

								if (data != null && data.start != Point16.Zero && data.end != Point16.Zero)
								{
									Main.spriteBatch.DrawOutline(data.start, data.end, data.color, 2);
									Utility.DrawMeasureText(data.start, data.end, data.color, 0.7f);
								}

								return true;
							}));
					}
				}
			}
		}
	}
}