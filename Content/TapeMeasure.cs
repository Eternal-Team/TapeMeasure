using System;
using System.IO;
using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TapeMeasure.Content
{
	// todo: line measurement
	public class TapeMeasure : BaseItem, IHasUI
	{
		public override string Texture => "TapeMeasure/Textures/TapeMeasure";

		public Guid ID;

		public Color Color;

		public Point16 start;
		public Point16 end;

		public TapeMeasure()
		{
			ID = Guid.NewGuid();
			Color = ColorUtility.FromHSV(Main.rand.NextFloat(), 1f, 1f);
			start = Point16.NegativeOne;
			end = Point16.NegativeOne;
		}

		public override ModItem Clone(Item Item)
		{
			TapeMeasure clone = (TapeMeasure)base.Clone(Item);
			clone.Color = Color;
			clone.start = start;
			clone.end = end;
			return clone;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 26;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 5);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.channel = true;
			Item.UseSound = SoundID.Item64;
			Item.rare = ItemRarityID.LightRed;
			Item.shoot = ModContent.ProjectileType<TapeMeasureProjectile>();
			Item.shootSpeed = 10;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.IronBar, 3)
				.AddIngredient(ItemID.Gel, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}

		public override bool ConsumeItem(Player player) => false;

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			if (player.whoAmI == Main.LocalPlayer.whoAmI)
				PanelUI.Instance.HandleUI(this);
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>("TapeMeasure/Textures/TapeMeasure_Glow").Value;
			spriteBatch.Draw(texture, position, null, Color, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("TapeMeasure/Textures/TapeMeasure_Glow").Value;
			spriteBatch.Draw(texture, Item.position - Main.screenPosition + new Vector2(Item.width * 0.5f, Item.height * 0.5f + 2), null, Color, rotation, new Vector2(Item.width, Item.height) * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public override void SaveData(TagCompound tag)
		{
			tag["ID"] = ID;
			tag["Color"] = Color;
		}

		public override void LoadData(TagCompound tag)
		{
			ID = tag.Get<Guid>("ID");
			Color = tag.Get<Color>("Color");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(ID);
			writer.WriteRGB(Color);
			writer.Write(start);
			writer.Write(end);
		}

		public override void NetReceive(BinaryReader reader)
		{
			ID = reader.ReadGuid();
			Color = reader.ReadRGB();
			start = reader.ReadPoint16();
			end = reader.ReadPoint16();
		}

		public Guid GetID() => ID;
	}
}