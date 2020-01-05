using BaseLibrary;
using BaseLibrary.UI.Elements;
using BaseLibrary.UI.New;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace TapeMeasure
{
	public class TapeMeasurePanel : BaseUIPanel<Items.TapeMeasure>
	{
		public TapeMeasurePanel(Items.TapeMeasure measure) : base(measure)
		{
			Width.Percent = 20;
			Height.Percent = 20;

			UIText textLabel = new UIText(Container.DisplayName.GetTranslation())
			{
				X = { Percent = 50 },
				HorizontalAlignment = HorizontalAlignment.Center
			};
			Add(textLabel);

			UITextButton buttonClose = new UITextButton("X")
			{
				Size = new Vector2(20),
				X = { Pixels = -20, Percent = 100 },
				Padding = Padding.Zero,
				RenderPanel = false,
				HoverText = Language.GetText("Mods.BaseLibrary.UI.Close")
			};
			buttonClose.OnClick += args => PanelUI.Instance.CloseUI(Container);
			Add(buttonClose);

			UIColorSelection wheel = new UIColorSelection(Container.Color)
			{
				Y = { Pixels = 28 },
				Width = { Percent = 100 },
				Height = { Pixels = -28, Percent = 100 }
			};
			Add(wheel);
		}
	}
}