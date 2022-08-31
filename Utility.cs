using System;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TapeMeasure;

public static class Utility
{
	public static void DrawMeasureText(SpriteBatch spriteBatch, Point16 start, Point16 end, Color color, float scale = 1f)
	{
		int width = Math.Abs(start.X - end.X) * 16 + 16;
		int height = Math.Abs(start.Y - end.Y) * 16 + 16;
		// Color inverted = ColorUtility.Invert(color);
		Color borderColor = Color.Black;

		Vector2 position = MathUtility.Min(start, end).ToScreenCoordinates(false);

		DynamicSpriteFont font = FontAssets.MouseText.Value;

		string widthText = $"{width / 16} tile{(width / 16 > 1 ? "s" : "")}";
		Utils.DrawBorderStringFourWay(spriteBatch, font, widthText, position.X + width / 2f, position.Y - 4, color, borderColor, new Vector2(font.MeasureString(widthText).X / 2f, font.MeasureString(widthText).Y * scale), scale);

		string heightText = $"{height / 16} tile{(height / 16 > 1 ? "s" : "")}";
		Utils.DrawBorderStringFourWay(spriteBatch, font, heightText, position.X - 4, position.Y + height / 2f, color, borderColor, new Vector2(font.MeasureString(heightText).X, font.MeasureString(heightText).Y / 2f), scale);
	}

	public static void DrawOutline(SpriteBatch spriteBatch, Point16 start, Point16 end, Color color, float lineSize = 2)
	{
		float width = Math.Abs(start.X - end.X) * 16 + 16;
		float height = Math.Abs(start.Y - end.Y) * 16 + 16;

		Vector2 position = MathUtility.Min(start, end).ToScreenCoordinates(false);

		spriteBatch.Draw(DrawingUtility.Pixel.Value, position, null, color, 0f, Vector2.Zero, new Vector2(width, lineSize), SpriteEffects.None, 0f);
		spriteBatch.Draw(DrawingUtility.Pixel.Value, position, null, color, 0f, Vector2.Zero, new Vector2(lineSize, height), SpriteEffects.None, 0f);

		spriteBatch.Draw(DrawingUtility.Pixel.Value, position + new Vector2(0, height - lineSize), null, color, 0f, Vector2.Zero, new Vector2(width, lineSize), SpriteEffects.None, 0f);
		spriteBatch.Draw(DrawingUtility.Pixel.Value, position + new Vector2(width - lineSize, 0), null, color, 0f, Vector2.Zero, new Vector2(lineSize, height), SpriteEffects.None, 0f);
	}

	public static void DrawLine(SpriteBatch spriteBatch, Point16 start, Point16 end, Color color, float lineSize = 2)
	{
		Vector2 actualStart = start.ToScreenCoordinates(false) + new Vector2(8f);
		Vector2 actualEnd = end.ToScreenCoordinates(false) + new Vector2(8f);

		// little cheat to prevent rotation and direction being NaN
		if (start == end) actualEnd.X += 0.001f;

		Vector2 dir = Vector2.Normalize(actualEnd - actualStart);
		float rotation = dir.ToRotation();
		float length = Vector2.Distance(actualStart, actualEnd);
		Vector2 center = actualStart + dir * length * 0.5f;

		spriteBatch.Draw(DrawingUtility.Pixel.Value, center, null, color, rotation, new Vector2(0.5f), new Vector2(length, lineSize), SpriteEffects.None, 0f);

		float tilesLength = length / 16f;
		string text = $"{tilesLength:0.#} tile{(tilesLength is > 1f or < 0.001f ? "s" : "")}";
		var font = FontAssets.MouseText.Value;

		var texture = ModContent.Request<Texture2D>(BaseLibrary.BaseLibrary.TexturePath + "UI/Dot").Value;
		spriteBatch.Draw(texture, actualStart, null, color, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
		spriteBatch.Draw(texture, actualEnd, null, color, rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

		Vector2 normal = dir.X < 0f ? new Vector2(-dir.Y, dir.X) : new Vector2(dir.Y, -dir.X);

		Vector2 textPos = center + normal * 10f;
		if (Math.Abs(rotation) > MathHelper.PiOver2) rotation += MathHelper.Pi;
		DrawingUtility.DrawTextWithBorder(Main.spriteBatch, font, text, textPos, color, Color.Black, font.MeasureString(text) * 0.5f, rotation, 0.8f);
	}
}