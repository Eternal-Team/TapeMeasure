using BaseLibrary;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace TapeMeasure;

public class TapeMeasurePanel : BaseUIPanel<Content.TapeMeasure>
{
	private UIColorSelection colorSelection;

	public TapeMeasurePanel(Content.TapeMeasure measure) : base(measure)
	{
		Width.Percent = 20;
		Height.Pixels = 84;

		UIText textLabel = new UIText(measure.Item.Name)
		{
			X = { Percent = 50 },
			Settings = { HorizontalAlignment = HorizontalAlignment.Center }
		};
		Add(textLabel);

		UIText modeSwitch = new UIText("Mode")
		{
			Width = { Pixels = 40 },
			Height = { Pixels = 20 },
		};
		modeSwitch.OnMouseDown += args =>
		{
			if (args.Button != MouseButton.Left) return;

			args.Handled = true;

			measure.Mode = measure.Mode.NextEnum();
			textLabel.Text = measure.Item.Name;
		};
		Add(modeSwitch);
		
		UIText buttonClose = new UIText("X")
		{
			Height = { Pixels = 20 },
			Width = { Pixels = 20 },
			X = { Percent = 100 },
			HoverText = Language.GetText("Mods.BaseLibrary.UI.Close")
		};
		buttonClose.OnMouseDown += args =>
		{
			if (args.Button != MouseButton.Left) return;

			PanelUI.Instance.CloseUI(measure);
			args.Handled = true;
		};
		buttonClose.OnMouseEnter += _ => buttonClose.Settings.TextColor = Color.Red;
		buttonClose.OnMouseLeave += _ => buttonClose.Settings.TextColor = Color.White;
		Add(buttonClose);

		colorSelection = new UIColorSelection
		{
			Y = { Pixels = 28 },
			Width = { Percent = 100 },
			Height = { Pixels = -28, Percent = 100 }
		};
		colorSelection.OnColorChange += color => Container.Color = color;
		Add(colorSelection);
	}

	protected override void Activate()
	{
		colorSelection.SetColor(Container.Color);
	}
}