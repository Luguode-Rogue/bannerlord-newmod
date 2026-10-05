# 临阵换将

独立的骑砍 2 Mod。内部模块 ID 为 `BattleHeroSwitch`，游戏中显示名称为“临阵换将”。

## 功能说明

1. 原主角健康时仍保留在部队花名册，可作为 AI 英雄参加战斗。
2. 原主角受伤时，选择一名健康英雄后，战斗不会再因为原主角受伤而被阻止。
3. 强攻匪窝时，当前操控英雄会被加入受限出战名单。
4. 潜入匪窝时，由原版控制器单独生成当前操控英雄，并避免重复生成。
5. 领主大厅等有出战人数限制的战斗，也会将当前操控英雄纳入出战名单。
6. 战斗结束后恢复原始 `PlayerTroop`。战前选择仅对当前遭遇有效，不写入存档。

## 工程结构

- `SubModule.xml`：Mod 根目录的模块清单。
- `BattleHeroSwitch/`：C# 工程及功能源码。
- `BattleHeroSwitch/_Module/SubModule.xml`：由工程构建资源使用的模块清单模板。

## 依赖

- Bannerlord.Harmony
- Native
- SandBoxCore
- Sandbox

