using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace TapeMeasure
{
	public class TapeMeasurePanel : BaseUIPanel<Items.TapeMeasure>
	{
		public override void OnInitialize()
		{
			Width = (0, 0.2f);
			Height = (0, 0.2f);
			this.Center();

			UIText textLabel = new UIText(Container.DisplayName.GetTranslation())
			{
				HAlign = 0.5f,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Append(textLabel);

			UITextButton buttonClose = new UITextButton("X")
			{
				Size = new Vector2(20),
				Left = (-20, 1),
				Padding = (0, 0, 0, 0),
				RenderPanel = false,
				HoverText = Language.GetText("Mods.BaseLibrary.UI.Close")
			};
			buttonClose.OnClick += (evt, element) => BaseLibrary.BaseLibrary.PanelGUI.UI.CloseUI(Container);
			Append(buttonClose);

			UIColorSelection wheel = new UIColorSelection(Container.Color)
			{
				Top = (28, 0),
				Width = (0, 1),
				Height = (-28, 1)
			};
			Append(wheel);
		}
	}
}