using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TheOneLibrary.Base.Items;
using TheOneLibrary.Base.UI;
using static TheOneLibrary.Utils.Utility;

namespace TapeMeasure.Items
{
	public class TapeMeasureItem : BaseItem
	{
		public override string Texture => "TapeMeasure/Items/TapeMeasureItem";

		public override bool CloneNewInstances => false;

		public GUI<TapeUI> gui;

		public Ref<Color> color = new Ref<Color>(Color.White);

		public Point16 start = Point16.Zero;
		public Point16 end = Point16.Zero;

		public override ModItem Clone(Item item)
		{
			TapeMeasureItem clone = (TapeMeasureItem)base.Clone(item);
			clone.start = start;
			clone.end = end;
			clone.color = color;
			clone.gui = gui;
			return clone;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tape Measure");
			Tooltip.SetDefault("Left-click on opposing corners to take measurements\nRight-click in invetory to configure");
		}

		public override void SetDefaults()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				TapeUI ui = new TapeUI();
				ui.measure = this;
				UserInterface userInterface = new UserInterface();
				ui.Activate();
				userInterface.SetState(ui);
				gui = new GUI<TapeUI>(ui, userInterface);
			}

			item.width = 38;
			item.height = 26;
			item.useTime = 20;
			item.useAnimation = 20;
			item.maxStack = 1;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
		}

		public override bool UseItem(Player player)
		{
			if (start != Point16.Zero && end == Point16.Zero) end = MouseToWorldPoint();
			else
			{
				start = MouseToWorldPoint();
				end = new Point16();
			}

			return true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			if (player.whoAmI == Main.LocalPlayer.whoAmI)
			{
				if (!TapeMeasure.Instance.TapeUI.ContainsValue(gui))
				{
					gui.ui.Load();
					TapeMeasure.Instance.TapeUI.Add(item.modItem, gui);
				}
				else
				{
					gui.ui.Unload();
					TapeMeasure.Instance.TapeUI.Remove(TapeMeasure.Instance.TapeUI.First(kvp => kvp.Value == gui).Key);
				}
			}

			item.stack++;
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(TapeMeasure.TapeMeasureGlow, position, null, color.Value, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.Draw(TapeMeasure.TapeMeasureGlow, item.position - Main.screenPosition + new Vector2(item.width / 2, item.height / 2 + 2), null, color.Value, rotation, new Vector2(item.width / 2, item.height / 2), scale, SpriteEffects.None, 0f);
		}

		public override TagCompound Save() => new TagCompound
		{
			["Color"] = color.Value
		};

		public override void Load(TagCompound tag)
		{
			try
			{
				Vector3 vec = tag.Get<Vector3>("Color");
				color.Value = new Color(vec.X, vec.Y, vec.Z);
			}
			catch
			{
				color.Value = tag.Get<Color>("Color");
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.WritePoint16(start);
			writer.WritePoint16(end);
			writer.WriteRGB(color.Value);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			start = reader.ReadPoint16();
			end = reader.ReadPoint16();
			color.Value = reader.ReadRGB();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddIngredient(ItemID.Daybloom);
			recipe.AddTile(TileID.WorkBenches);
			recipe.anyIronBar = true;
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}