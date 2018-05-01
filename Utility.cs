using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using TheOneLibrary.Utils;

namespace TapeMeasure
{
	public static class Utility
	{
		public static void DrawMeasureText(Point16 start, Point16 end, Color color, float scale = 1f)
		{
			int width = Math.Abs(start.X - end.X) * 16 + 16;
			int height = Math.Abs(start.Y - end.Y) * 16 + 16;

			Vector2 position = -Main.screenPosition + start.Min(end).ToVector2() * 16;

			string widthText = $"{width / 16} tile{(width / 16 > 1 ? "s" : "")}";
			Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, widthText, position.X + width / 2f, position.Y - 4, color, Color.Black, new Vector2(Main.fontMouseText.MeasureString(widthText).X / 2f, Main.fontMouseText.MeasureString(widthText).Y * scale), scale);

			string heightText = $"{height / 16} tile{(height / 16 > 1 ? "s" : "")}";
			Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, heightText, position.X - 4, position.Y + height / 2f, color, Color.Black, new Vector2(Main.fontMouseText.MeasureString(heightText).X, Main.fontMouseText.MeasureString(heightText).Y / 2f), scale);
		}
	}
}