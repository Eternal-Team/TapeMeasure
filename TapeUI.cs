using Microsoft.Xna.Framework;
using TapeMeasure.Items;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using TheOneLibrary.Base.UI;
using TheOneLibrary.UI.Elements;
using TheOneLibrary.Utils;

namespace TapeMeasure
{
	public class TapeUI : BaseUI
	{
		public UIColorBar wheel = new UIColorBar();
		public UIText textColor = new UIText("Select a color:");
		public UIText textCurrentColor = new UIText("Current color:");
		public UIColor colorCurrent = new UIColor(Color.White);

		private TapeMeasureItem measure;

		public override void OnInitialize()
		{
			panelMain.Width.Precent = 0.2f;
			panelMain.Height.Precent = 0.05f;
			panelMain.Center();
			panelMain.SetPadding(0);
			panelMain.BackgroundColor = TheOneLibrary.Utils.Utility.PanelColor;
			Append(panelMain);

			textColor.Left.Pixels = 8;
			textColor.Top.Pixels = 8;
			panelMain.Append(textColor);

			CalculatedStyle dimensions = textColor.GetDimensions();
			wheel.Width.Set(-24 - dimensions.Width, 1);
			wheel.Height.Pixels = dimensions.Height;
			wheel.HAlign = 1;
			wheel.Left.Pixels -= 8;
			wheel.Top.Pixels = 8;
			wheel.OnClick += (evt, e) =>
			{
				Main.PlaySound(SoundID.MenuTick);
				measure.color = wheel.GetColor();
				colorCurrent.color = measure.color;
			};
			panelMain.Append(wheel);

			textCurrentColor.Left.Pixels = 8;
			textCurrentColor.VAlign = 1;
			textCurrentColor.Top.Pixels -= 8;
			panelMain.Append(textCurrentColor);

			dimensions = textCurrentColor.GetDimensions();
			colorCurrent.Width.Set(-24 - dimensions.Width, 1);
			colorCurrent.Height.Pixels = dimensions.Height;
			colorCurrent.HAlign = 1;
			colorCurrent.Left.Pixels -= 8;
			colorCurrent.VAlign = 1;
			colorCurrent.Top.Pixels -= 8;
			panelMain.Append(colorCurrent);
		}

		public void Load(TapeMeasureItem item)
		{
			measure = item;
			colorCurrent.color = measure.color;
		}
	}
}