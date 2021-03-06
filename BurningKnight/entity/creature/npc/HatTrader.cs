using BurningKnight.entity.component;
using BurningKnight.entity.item.stand;
using BurningKnight.ui.dialog;
using Lens.util.math;

namespace BurningKnight.entity.creature.npc {
	public class HatTrader : ShopNpc {
		public override void AddComponents() {
			base.AddComponents();
			
			AlwaysActive = true;
			Width = 13;
			Height = 20;
			
			AddComponent(new AnimationComponent("hat_trader"));

			var b = new RectBodyComponent(2, 6, Width - 4, Height - 6);
			AddComponent(b);
			b.KnockbackModifier = 0;
			GetComponent<DialogComponent>().Dialog.Voice = 6;
		}

		protected override string GetDialog() {
			return $"hattrader_{Rnd.Int(3)}";
		}

		public override string GetId() {
			return HatTrader;
		}

		protected override bool OwnsStand(ItemStand stand) {
			return stand is HatStand;
		}
	}
}