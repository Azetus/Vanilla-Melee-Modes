# Vanilla-Melee-Modes

一个《边缘世界》游戏 Mod，为原版近战系统加入可切换近战模式，并在不破坏原版战斗框架的前提下添加格挡与反击机制。

## 主要功能

- 提供 4 种近战模式：默认 / 强攻 / 迅捷连击 / 防御姿态
- 影响近战命中率、闪避率、伤害倍率、冷却时间、穿甲倍率
- 格挡系统：正面格挡近战攻击，减伤并触发反击
- 反击系统：格挡成功后有机会立刻反击（内置冷却）
- 自动切换近战模式：根据敌人护甲、技能、威胁数量等自动选择最优模式
- 所有参数可在 Mod 设置中调整
- 支持 Combat Extended（独立兼容层）

## 注意事项

1. 格挡与反击默认仅对玩家阵营生效，可在 Mod 设置中为 NPC 开启
2. 自动切换模式默认关闭，殖民者通过 Gizmo 按钮手动开启
3. 社交打架中不触发格挡与反击

## 兼容性

与绝大多数 Mod 兼容。采用 Harmony 补丁仅在三个节点注入逻辑：
`Pawn.PreApplyDamage`（格挡/反击）、`VerbProperties.AdjustedArmorPenetration`（穿甲修正）、
`Pawn_JobTracker.StartJob`（自动切换玩家响应轨）。侵入性极低。

各近战模式对命中率、闪避率、伤害倍率、冷却时间等属性的影响通过运行时
`StatPart` 注入的方式叠加，不覆盖原版 StatWorker，与其他修改同类属性的 Mod 自然共存。

## FAQ

**Q：Combat Extended？**  
A：完全兼容。检测到 CE 时自动加载独立兼容层，评估器使用 CE 的 mm RHA 穿甲体系判定。

**Q：可以中途加入/移除吗？**  
A：可以（移除理论上安全）。

---

# Vanilla-Melee-Modes

A Rimworld mod that adds selectable melee modes to vanilla melee combat, along with parry and counterattack mechanics — no core combat overhaul required.

## Features

- 4 melee modes: Default / Aggressive / Flurry / Guard
- Affects melee hit chance, dodge chance, damage factor, cooldown, and armor penetration
- Parry system: block incoming melee attacks from the front, reducing or negating damage
- Counterattack system: chance to retaliate immediately after a successful parry
- Auto-Selection: AI evaluates enemy armor, skill, threat count, and other factors to pick the optimal mode
- Fully configurable via Mod Settings
- Combat Extended support via a dedicated compatibility layer

## Notes

1. Parry and counterattack only apply to the player faction by default; can be enabled for NPCs in settings
2. Auto-Selection is off by default toggled via the Gizmo button for colonists
3. Social fights do not trigger parry or counterattack

## Compatibility

Compatible with the vast majority of mods. Harmony patches are injected at only three points:
`Pawn.PreApplyDamage` (parry/counterattack), `VerbProperties.AdjustedArmorPenetration` (AP modifier),
and `Pawn_JobTracker.StartJob` (auto-selection player response track) — minimal footprint.

Mode effects on stats such as melee hit chance, dodge chance, damage factor, and cooldown are
applied via runtime `StatPart` injection, leaving the vanilla `StatWorker` intact and coexisting
naturally with other mods that modify the same stats.

## FAQ

**Q: Combat Extended?**  
A: Fully compatible. A dedicated compatibility layer loads automatically when CE is detected, with a separate evaluator using CE's mm RHA armor penetration system.

**Q: Safe to add/remove mid-save?**  
A: Yes (removal is theoretically safe).