using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TapeMeasure
{
	public class TMNPC : GlobalNPC
	{
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.GoblinTinkerer && Main.hardMode)
			{
				shop.item[nextSlot] = new Item();
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.TapeMeasure>());
				nextSlot++;
			}
		}
	}
}