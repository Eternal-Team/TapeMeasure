using System;
using System.IO;
using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TapeMeasure.Content
{
	public class TapeMeasure : BaseItem, IHasUI
	{
		public enum MeasurementMode : byte
		{
			Line,
			Area
		}

		public override string Texture => "TapeMeasure/Textures/TapeMeasure";

		private static Asset<Texture2D> GlowTexture;
		private MeasurementMode mode;

		public Guid ID;
		public Color Color;
		public Point16 start;
		public Point16 end;

		public MeasurementMode Mode
		{
			get => mode;
			set
			{
				mode = value;

				Item.SetNameOverride(Lang.GetItemNameValue(Item.type) + $" ({(mode == MeasurementMode.Area ? "Area" : "Line")})");
			}
		}

		public TapeMeasure()
		{
			GlowTexture ??= ModContent.Request<Texture2D>("TapeMeasure/Textures/TapeMeasure_Glow");

			ID = Guid.NewGuid();
			Color = ColorUtility.FromHSV(Main.rand.NextFloat(), 1f, 1f);
			start = Point16.NegativeOne;
			end = Point16.NegativeOne;
			mode = MeasurementMode.Area;
		}

		public override ModItem Clone(Item Item)
		{
			TapeMeasure clone = (TapeMeasure)base.Clone(Item);
			clone.Color = Color;
			clone.Mode = Mode;
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
			spriteBatch.Draw(GlowTexture.Value, position, null, Color, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.Draw(GlowTexture.Value, Item.position - Main.screenPosition + new Vector2(Item.width * 0.5f, Item.height * 0.5f + 2), null, Color, rotation, new Vector2(Item.width, Item.height) * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public override void SaveData(TagCompound tag)
		{
			tag["ID"] = ID;
			tag["Color"] = Color;
			tag["Mode"] = (byte)Mode;
		}

		public override void LoadData(TagCompound tag)
		{
			ID = tag.Get<Guid>("ID");
			Color = tag.Get<Color>("Color");
			Mode = (MeasurementMode)tag.GetByte("Mode");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(ID);
			writer.WriteRGB(Color);
			writer.Write((byte)Mode);
			writer.Write(start);
			writer.Write(end);
		}

		public override void NetReceive(BinaryReader reader)
		{
			ID = reader.ReadGuid();
			Color = reader.ReadRGB();
			Mode = (MeasurementMode)reader.ReadByte();
			start = reader.ReadPoint16();
			end = reader.ReadPoint16();
		}

		public Guid GetID() => ID;
	}
}