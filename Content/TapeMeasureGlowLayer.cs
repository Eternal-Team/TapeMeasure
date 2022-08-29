using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TapeMeasure.Content;

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

		Item heldItem = drawinfo.heldItem;
		int num = heldItem.type;

		Rectangle? sourceRect = Main.itemAnimations[num]?.GetFrame(texture) ?? texture.Frame();
		Rectangle frame = sourceRect.Value;
		int width = frame.Width;
		int height = frame.Height;

		Vector2 itemPos = Main.DrawPlayerItemPos(drawinfo.drawPlayer.gravDir, num);
		int posX = (int)itemPos.X;
		Vector2 offset = new Vector2(width * 0.5f, itemPos.Y);
		float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);

		Vector2 origin = new Vector2(-posX, height * 0.5f);

		if (drawinfo.drawPlayer.direction == -1)
			origin = new Vector2(width + posX, height * 0.5f);

		Color color = Color.White;
		if (heldItem.ModItem is TapeMeasure measure)
			color = measure.Color;

		var item = new DrawData(texture, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + offset.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + offset.Y)), sourceRect, color, drawinfo.drawPlayer.itemRotation, origin, adjustedItemScale, drawinfo.itemEffect, 0);
		drawinfo.DrawDataCache.Add(item);
	}
}