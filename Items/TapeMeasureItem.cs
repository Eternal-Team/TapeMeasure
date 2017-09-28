using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static BaseLib.Utility.Utility;

namespace TapeMeasure.Items
{
	public class TapeMeasureItem : ModItem
	{
		public override bool CloneNewInstances => false;

		public override ModItem Clone(Item item)
		{
			TapeMeasureItem clone = (TapeMeasureItem)base.Clone(item);
			clone.start = start;
			clone.end = end;
			clone.color = color;
			return clone;
		}

		public Color color = Color.White;

		public Point16 start = Point16.Zero;
		public Point16 end = Point16.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tape Measure");
			Tooltip.SetDefault("Left-click on opposing corners to take measurements\nRight-click in invetory to configure");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.useTime = 20;
			item.useAnimation = 20;
			item.maxStack = 1;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
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
			TapeMeasure.Instance.TapeUI.visible = !TapeMeasure.Instance.TapeUI.visible;
			if (TapeMeasure.Instance.TapeUI.visible) TapeMeasure.Instance.TapeUI.Load(this);

			item.stack++;
		}

		public override TagCompound Save() => new TagCompound
		{
			["Color"] = color.ToVector3()
		};

		public override void Load(TagCompound tag)
		{
			Vector3 vec = tag.Get<Vector3>("Color");
			color = new Color(vec.X, vec.Y, vec.Z);
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.WritePoint16(start);
			writer.WritePoint16(end);
			writer.WriteRGB(color);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			start = reader.ReadPoint16();
			end = reader.ReadPoint16();
			color = reader.ReadRGB();
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