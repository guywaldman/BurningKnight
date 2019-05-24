using System.Collections.Generic;
using BurningKnight.assets.items;
using BurningKnight.util;
using Lens.util;

namespace BurningKnight.entity.item {
	public class ItemPool {
		public static Dictionary<string, ItemPool> ByName = new Dictionary<string, ItemPool>();
		public static ItemPool[] ById = new ItemPool[32];
		
		public static readonly ItemPool Consumable = new ItemPool("consumable");
		public static readonly ItemPool Chest = new ItemPool("chest");
		public static readonly ItemPool Secret = new ItemPool("secret");
		public static readonly ItemPool Lamp = new ItemPool("lamp");

		private static int count;
		
		public readonly string Name;
		public readonly int Id;

		public ItemPool(string name) {
			if (count >= 32) {
				Log.Error($"Can not define item pool {name}, 32 pools were already defined");
				return;
			}
			
			Name = name;
			Id = count;

			ById[Id] = this;
			ByName[name] = this;
			
			count++;
		}

		public int Apply(int pools) {
			return BitHelper.SetBit(pools, Id, true);
		}

		public int Unapply(int pools) {
			return BitHelper.SetBit(pools, Id, false);
		}
		
		public bool Contains(int pools) {
			return BitHelper.IsBitSet(pools, Id);
		}
	}
}