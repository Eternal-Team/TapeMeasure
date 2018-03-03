using System.Linq;
using Microsoft.Xna.Framework.Input;
using TapeMeasure.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TapeMeasure
{
	public class TapePlayer : ModPlayer
	{
		public override void PreUpdate()
		{
			Keys[] pressedKeys = Main.keyState.GetPressedKeys();

			if (pressedKeys.Contains(Keys.RightShift) && Main.mouseRight && Main.mouseRightRelease)
			{
				if (Main.LocalPlayer.HeldItem.type == mod.ItemType<TapeMeasureItem>())
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
	}
}