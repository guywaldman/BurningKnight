﻿using BurningKnight.entity.component;
using ImGuiNET;
using Lens.entity;
using Lens.lightJson;

namespace BurningKnight.entity.item.use {
	public class ModifyMaxHpUse : ItemUse {
		public int Amount;
		public bool GiveHp;

		public override void Use(Entity entity, Item item) {
			var component = entity.GetComponent<HealthComponent>();
			component.MaxHealth += Amount;
			
			if (GiveHp && Amount > 0) {
				component.ModifyHealth(Amount, entity);
			}
		}

		public override void Setup(JsonValue settings) {
			base.Setup(settings);
			
			Amount = settings["amount"].Int(1);
			GiveHp = settings["give_hp"].Bool(true);
		}
		
		public static void RenderDebug(JsonValue root) {
			var val = root["amount"].AsInteger;

			if (ImGui.InputInt("Amount", ref val)) {
				root["amount"] = val;
			}

			var giveHp = root["give_hp"].AsBoolean;

			if (ImGui.Checkbox("Give health", ref giveHp)) {
				root["give_hp"] = giveHp;
			}
		}
	}
}