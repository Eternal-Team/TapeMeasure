using BaseLibrary;
using BaseLibrary.Items;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TapeMeasure.Items
{
	public class TapeMeasure : BaseItem, IHasUI
	{
		public override string Texture => "TapeMeasure/Textures/TapeMeasure";

		public override bool CloneNewInstances => true;

		public virtual LegacySoundStyle OpenSound => SoundID.Item1;
		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public virtual LegacySoundStyle CloseSound => SoundID.Item1;

		public BaseLibrary.Ref<Color> Color = new BaseLibrary.Ref<Color>(Microsoft.Xna.Framework.Color.White);

		public Point16 start = Point16.Zero;
		public Point16 end = Point16.Zero;

		public override ModItem Clone()
		{
			TapeMeasure clone = (TapeMeasure)base.Clone();
			clone.Color = new BaseLibrary.Ref<Color>(Color.Value);
			clone.start = start;
			clone.end = end;
			return clone;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tape Measure");
			Tooltip.SetDefault("Left-click on opposing corners to take measurements\nRight-click in inventory to configure");
		}

		public override void SetDefaults()
		{
			UUID = Guid.NewGuid();

			item.width = 38;
			item.height = 26;
			item.useTime = 20;
			item.useAnimation = 20;
			item.maxStack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.UseSound = SoundID.Item1;
			item.rare = ItemRarityID.Cyan;
		}

		public override bool UseItem(Player player)
		{
			if (start != Point16.Zero && end == Point16.Zero) end = new Point16(Player.tileTargetX, Player.tileTargetY);
			else
			{
				start = new Point16(Player.tileTargetX, Player.tileTargetY);
				end = new Point16();
			}

			return true;
		}

		public override bool ConsumeItem(Player player) => false;

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			if (player.whoAmI == Main.LocalPlayer.whoAmI) BaseLibrary.BaseLibrary.PanelGUI.UI.HandleUI(this);
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(global::TapeMeasure.TapeMeasure.textureGlow, position, null, Color.Value, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.Draw(global::TapeMeasure.TapeMeasure.textureGlow, item.position - Main.screenPosition + new Vector2(item.width * 0.5f, item.height * 0.5f + 2), null, Color.Value, rotation, new Vector2(item.width, item.height) * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public override TagCompound Save() => new TagCompound
		{
			["UUID"] = UUID,
			["Color"] = Color.Value
		};

		public override void Load(TagCompound tag)
		{
			UUID = tag.Get<Guid>("UUID");
			Color.Value = tag.Get<Color>("Color");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(UUID);
			writer.Write(start);
			writer.Write(end);
			writer.WriteRGB(Color.Value);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			UUID = reader.ReadGUID();
			start = reader.ReadPoint16();
			end = reader.ReadPoint16();
			Color.Value = reader.ReadRGB();
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