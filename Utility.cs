using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;

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

		public static void WritePoint16(this BinaryWriter bb, Point16 p)
		{
			bb.Write(p.X);
			bb.Write(p.Y);
		}

		public static Point16 ReadPoint16(this BinaryReader bb) => new Point16(bb.ReadInt16(), bb.ReadInt16());

		public static Vector2 ToVector2(this Point16 point) => new Vector2(point.X, point.Y);

		public static Point16 Min(this Point16 point, Point16 compareTo) => new Point16(point.X > compareTo.X ? compareTo.X : point.X, point.Y > compareTo.Y ? compareTo.Y : point.Y);
	}
}