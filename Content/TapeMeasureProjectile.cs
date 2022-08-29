using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TapeMeasure.Content;

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

	private ref float TileX => ref Projectile.ai[0];
	private ref float TileY => ref Projectile.ai[1];

	public override void AI()
	{
		Player owner = Main.player[Projectile.owner];

		if (Main.myPlayer == Projectile.owner)
		{
			// only update on mouse move
			if (Projectile.localAI[1] > 0f)
				Projectile.localAI[1]--;

			if (owner.noItems || owner.CCed || owner.dead)
			{
				Projectile.Kill();
			}
			else if (Main.mouseRight && Main.mouseRightRelease)
			{
				Projectile.Kill();
				owner.mouseInterface = true;
				Main.blockMouse = true;
			}
			else if (!owner.channel)
			{
				if (Projectile.localAI[0] == 0f)
					Projectile.localAI[0] = 1f;

				Projectile.Kill();
			}
			else if (Projectile.localAI[1] == 0f)
			{
				Vector2 newPosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

				if (newPosition != Projectile.Center)
				{
					Projectile.netUpdate = true;
					Projectile.Center = newPosition;
					Projectile.localAI[1] = 1f;
				}

				if (TileX == 0f && TileY == 0f)
				{
					TileX = (int)(Projectile.Center.X / 16);
					TileY = (int)(Projectile.Center.Y / 16);

					Projectile.netUpdate = true;
					Projectile.velocity = Vector2.Zero;
				}
			}

			Projectile.velocity = Vector2.Zero;
			Point start = new Vector2(TileX, TileY).ToPoint();
			Point end = Projectile.Center.ToTileCoordinates();

			if (owner.inventory[owner.selectedItem].ModItem is TapeMeasure measure)
			{
				measure.start = new Point16(start.X, start.Y);
				measure.end = new Point16(end.X, end.Y);
			}
		}

		int direction = Math.Sign(owner.velocity.X);
		if (direction != 0)
			owner.ChangeDir(direction);

		owner.heldProj = Projectile.whoAmI;
		owner.SetDummyItemTime(2);
		owner.itemRotation = 0f;
	}

	public override bool PreDraw(ref Color lightColor) => false;
}