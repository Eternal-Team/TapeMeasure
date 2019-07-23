using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;

namespace TapeMeasure
{
	public class TapeUI : BaseUIPanel<Items.TapeMeasure>
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