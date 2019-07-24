using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using Microsoft.Xna.Framework;

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
				HAlign = 0.5f
			};
			Append(textLabel);

			UITextButton buttonClose = new UITextButton("X")
			{
				Size = new Vector2(20),
				Left = (-20, 1),
				RenderPanel = false
			};
			buttonClose.GetHoverText += () => "Close";
			buttonClose.OnClick += (evt, element) => BaseLibrary.BaseLibrary.PanelGUI.UI.CloseUI(Container);
			Append(buttonClose);

			UIText textColor = new UIText("Select a color:")
			{
				Top = (28, 0)
			};
			Append(textColor);

			UIColorSelection wheel = new UIColorSelection(Container.Color, 24)
			{
				Top = (28, 0),
				Width = (-"Select a color:".Measure().X, 1),
				Height = (-28, 1),
				HAlign = 1
			};
			Append(wheel);
		}
	}
}