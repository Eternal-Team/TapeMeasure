using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;

namespace TapeMeasure
{
	public static class Utility
	{
		public static void DrawMeasureText(this SpriteBatch spriteBatch, Point16 start, Point16 end, Color color, float scale = 1f)
		{
			int width = Math.Abs(start.X - end.X) * 16 + 16;
			int height = Math.Abs(start.Y - end.Y) * 16 + 16;
			Color inverted = color.Invert();

			Vector2 position = start.Min(end).ToScreenCoordinates(false);

			string widthText = $"{width / 16} tile{(width / 16 > 1 ? "s" : "")}";
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, widthText, position.X + width / 2f, position.Y - 4, color, inverted, new Vector2(Main.fontMouseText.MeasureString(widthText).X / 2f, Main.fontMouseText.MeasureString(widthText).Y * scale), scale);

			string heightText = $"{height / 16} tile{(height / 16 > 1 ? "s" : "")}";
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, heightText, position.X - 4, position.Y + height / 2f, color, inverted, new Vector2(Main.fontMouseText.MeasureString(heightText).X, Main.fontMouseText.MeasureString(heightText).Y / 2f), scale);
		}

		public static void DrawOutline(this SpriteBatch spriteBatch, Point16 start, Point16 end, Color color, float lineSize = 2)
		{
			float width = Math.Abs(start.X - end.X) * 16 + 16;
			float height = Math.Abs(start.Y - end.Y) * 16 + 16;

			Vector2 position = start.Min(end).ToScreenCoordinates(false);

			spriteBatch.Draw(Main.magicPixel, position, null, color, 0f, Vector2.Zero, new Vector2(width, lineSize / 1000f), SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.magicPixel, position, null, color, 0f, Vector2.Zero, new Vector2(lineSize, height / 1000f), SpriteEffects.None, 0f);

			spriteBatch.Draw(Main.magicPixel, position + new Vector2(0, height - lineSize), null, color, 0f, Vector2.Zero, new Vector2(width, lineSize / 1000f), SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.magicPixel, position + new Vector2(width - lineSize, 0), null, color, 0f, Vector2.Zero, new Vector2(lineSize, height / 1000f), SpriteEffects.None, 0f);
		}
	}
}