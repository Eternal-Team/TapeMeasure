using BaseLibrary;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace TapeMeasure;

public class TapeMeasurePanel : BaseUIPanel<Content.TapeMeasure>
{
	public TapeMeasurePanel(Content.TapeMeasure measure) : base(measure)
	{
		Width.Percent = 20;
		Height.Percent = 20;

		UIText textLabel = new UIText(Container.Item.Name)
		{
			X = { Percent = 50 },
			Y = { Percent = 50 }
		};
		Add(textLabel);

		UIText buttonClose = new UIText("X")
		{
			Height = { Pixels = 20 },
			Width = { Pixels = 20 },
			X = { Percent = 100 },
			HoverText = Language.GetText("Mods.PortableStorage.UI.Close")
		};
		buttonClose.OnMouseDown += args =>
		{
			if (args.Button != MouseButton.Left) return;

			PanelUI.Instance.CloseUI(Container);
			args.Handled = true;
		};
		buttonClose.OnMouseEnter += _ => buttonClose.Settings.TextColor = Color.Red;
		buttonClose.OnMouseLeave += _ => buttonClose.Settings.TextColor = Color.White;
		Add(buttonClose);

		// todo: add
		// UIColorSelection wheel = new UIColorSelection(Container.Color)
		// {
		// 	Y = { Pixels = 28 },
		// 	Width = { Percent = 100 },
		// 	Height = { Pixels = -28, Percent = 100 }
		// };
		// Add(wheel);
	}
}