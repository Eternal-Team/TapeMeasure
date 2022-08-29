using System;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;

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

		spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, null, color, 0f, Vector2.Zero, new Vector2(width, lineSize * 0.001f), SpriteEffects.None, 0f);
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, null, color, 0f, Vector2.Zero, new Vector2(lineSize, height * 0.001f), SpriteEffects.None, 0f);

		spriteBatch.Draw(TextureAssets.MagicPixel.Value, position + new Vector2(0, height - lineSize), null, color, 0f, Vector2.Zero, new Vector2(width, lineSize * 0.001f), SpriteEffects.None, 0f);
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, position + new Vector2(width - lineSize, 0), null, color, 0f, Vector2.Zero, new Vector2(lineSize, height * 0.001f), SpriteEffects.None, 0f);
	}
}