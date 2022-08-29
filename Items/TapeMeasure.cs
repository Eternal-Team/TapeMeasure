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

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace TapeMeasure.Items
{
	public class TapeMeasureProjectile : ModProjectile
	{
		public override string Texture => BaseLibrary.BaseLibrary.PlaceholderTexture;

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		// public override void Kill(int timeLeft)
		// {
		// 	if (Projectile.localAI[0] == 1f && Projectile.owner == Main.myPlayer)
		// 	{
		// 		Player master = Main.player[Projectile.owner];
		// 		Point ps = new Vector2(Projectile.ai[0], Projectile.ai[1]).ToPoint();
		// 		Point pe = Projectile.Center.ToTileCoordinates();
		// 		
		// 		if (master.inventory[master.selectedItem].ModItem is TapeMeasure measure)
		// 		{
		// 			measure.start = new Point16(ps.X, ps.Y);
		// 			measure.end = new Point16(pe.X, pe.Y);
		// 		}
		// 	}
		// }

		public override void AI()
		{
			Player player13 = Main.player[Projectile.owner];
			if (Main.myPlayer == Projectile.owner)
			{
				if (Projectile.localAI[1] > 0f)
					Projectile.localAI[1]--;

				if (player13.noItems || player13.CCed || player13.dead)
				{
					Projectile.Kill();
				}
				else if (Main.mouseRight && Main.mouseRightRelease)
				{
					Projectile.Kill();
					player13.mouseInterface = true;
					Main.blockMouse = true;
				}
				else if (!player13.channel)
				{
					if (Projectile.localAI[0] == 0f)
						Projectile.localAI[0] = 1f;

					Projectile.Kill();
				}
				else if (Projectile.localAI[1] == 0f)
				{
					Vector2 vector103 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
					if (player13.gravDir == -1f)
						vector103.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y;

					if (vector103 != Projectile.Center)
					{
						Projectile.netUpdate = true;
						Projectile.Center = vector103;
						Projectile.localAI[1] = 1f;
					}

					if (Projectile.ai[0] == 0f && Projectile.ai[1] == 0f)
					{
						Projectile.ai[0] = (int)Projectile.Center.X / 16;
						Projectile.ai[1] = (int)Projectile.Center.Y / 16;


						Projectile.netUpdate = true;
						Projectile.velocity = Vector2.Zero;
					}
				}

				Projectile.velocity = Vector2.Zero;
				Point point2 = new Vector2(Projectile.ai[0], Projectile.ai[1]).ToPoint();
				Point point3 = Projectile.Center.ToTileCoordinates();

				if (player13.inventory[player13.selectedItem].ModItem is TapeMeasure measure)
				{
					measure.start = new Point16(point2.X, point2.Y);
					measure.end = new Point16(point3.X, point3.Y);
				}

				int num990 = Math.Abs(point2.X - point3.X);
				int num991 = Math.Abs(point2.Y - point3.Y);
				int num992 = Math.Sign(point3.X - point2.X);
				int num993 = Math.Sign(point3.Y - point2.Y);
				Point point4 = default(Point);
				bool flag60 = false;
				bool flag61 = player13.direction == 1;
				int num994;
				int num995;
				int num996;
				if (flag61)
				{
					point4.X = point2.X;
					num994 = point2.Y;
					num995 = point3.Y;
					num996 = num993;
				}
				else
				{
					point4.Y = point2.Y;
					num994 = point2.X;
					num995 = point3.X;
					num996 = num992;
				}

				for (int num997 = num994; num997 != num995; num997 += num996)
				{
					if (flag60)
						break;

					if (flag61)
						point4.Y = num997;
					else
						point4.X = num997;

					if (WorldGen.InWorld(point4.X, point4.Y, 1))
					{
						Tile tile2 = Main.tile[point4.X, point4.Y];
					}
				}

				if (flag61)
				{
					point4.Y = point3.Y;
					num994 = point2.X;
					num995 = point3.X;
					num996 = num992;
				}
				else
				{
					point4.X = point3.X;
					num994 = point2.Y;
					num995 = point3.Y;
					num996 = num993;
				}

				for (int num998 = num994; num998 != num995; num998 += num996)
				{
					if (flag60)
						break;

					if (!flag61)
						point4.Y = num998;
					else
						point4.X = num998;

					if (WorldGen.InWorld(point4.X, point4.Y, 1))
					{
						Tile tile2 = Main.tile[point4.X, point4.Y];
					}
				}
			}

			int num999 = Math.Sign(player13.velocity.X);
			if (num999 != 0)
				player13.ChangeDir(num999);

			player13.heldProj = Projectile.whoAmI;
			player13.SetDummyItemTime(2);
			player13.itemRotation = 0f;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}

	internal class TapeMeasureGlowLayer : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.heldProj >= 0 && Main.projectile[drawInfo.drawPlayer.heldProj].type == ModContent.ProjectileType<TapeMeasureProjectile>();
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

		protected override void Draw(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.JustDroppedAnItem)
				return;

			if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && !drawinfo.heldProjOverHand)
				drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;

			if (drawinfo.shadow != 0f || drawinfo.drawPlayer.frozen || drawinfo.drawPlayer.dead)
				return;

			Texture2D texture = ModContent.Request<Texture2D>("TapeMeasure/Textures/TapeMeasure_Glow").Value;

			int num12 = 10;

			// If the item is animated, its texture is larger than the item because it contains multiple animation frames.
			// Because of this, we can't trust the dimensions of the item's texture.
			// Thankfully, `sourceRect` contains the width and height of the item.
			// Specifically, it contains either the dimensions of the item's texture or the dimensions of a single frame of the item's animation if it is animated.

			Item heldItem = drawinfo.heldItem;
			int num = heldItem.type;

			Rectangle? sourceRect = Main.itemAnimations[num]?.GetFrame(texture) ?? texture.Frame();
			var frame = sourceRect.Value;
			int width = frame.Width;
			int height = frame.Height;

			/*
			Vector2 vector5 = new Vector2(value.Width *0.5f, value.Height *0.5f);
			*/
			Vector2 vector5 = new Vector2(width * 0.5f, height * 0.5f);

			Vector2 vector6 = Main.DrawPlayerItemPos(drawinfo.drawPlayer.gravDir, num);
			num12 = (int)vector6.X;
			vector5.Y = vector6.Y;
			float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);

			/*
			Vector2 origin7 = new Vector2(-num12, value.Height *0.5f);
			*/
			Vector2 origin7 = new Vector2(-num12, height * 0.5f);

			if (drawinfo.drawPlayer.direction == -1)
				/*
				origin7 = new Vector2(value.Width + num12, value.Height *0.5f);
				*/
				origin7 = new Vector2(width + num12, height * 0.5f);

			Color color = Color.White;
			if (heldItem.ModItem is TapeMeasure measure)
				color = measure.Color;

			var item = new DrawData(texture, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector5.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector5.Y)), sourceRect, color, drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect, 0);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public class TapeMeasure : BaseItem, IHasUI
	{
		public override string Texture => "TapeMeasure/Textures/TapeMeasure";

		public Guid ID;

		public Color Color;

		public Point16 start = Point16.NegativeOne;
		public Point16 end = Point16.NegativeOne;

		public TapeMeasure()
		{
			ID = Guid.NewGuid();
			Color = ColorUtility.FromHSV(Main.rand.NextFloat(), 1f, 1f);
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

		public override bool CanRightClick() => true;

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				start = end = Point16.NegativeOne;

				return false;
			}

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

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
			ID = tag.Get<Guid>("UUID");
			Color = tag.Get<Color>("Color");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(ID);
			// writer.Write(start);
			// writer.Write(end);
			writer.WriteRGB(Color);
		}

		public override void NetReceive(BinaryReader reader)
		{
			ID = reader.ReadGuid();
			// start = reader.ReadPoint16();
			// end = reader.ReadPoint16();
			Color = reader.ReadRGB();
		}

		public Guid GetID() => ID;
	}
}