using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BattleHeroSwitch
{
    public sealed class SubModule : MBSubModuleBase
    {
        private const string HarmonyId = "BattleHeroSwitch";

        private Harmony _harmony;

        /// <summary>
        /// OnSubModuleLoad（模块加载）
        /// 初始化 Harmony 并应用当前程序集中的所有补丁。
        /// </summary>
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            _harmony = new Harmony(HarmonyId);

            // PatchAll（应用全部 Harmony 补丁）
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// InitializeGameStarter（初始化游戏启动器）
        /// 在战役模式中注册 CampaignBehavior。
        /// </summary>
        protected override void InitializeGameStarter(
            Game game,
            IGameStarter gameStarterObject)
        {
            base.InitializeGameStarter(game, gameStarterObject);

            if (game.GameType is Campaign &&
                gameStarterObject is CampaignGameStarter campaignStarter)
            {
                // AddBehavior（添加战役行为）
                campaignStarter.AddBehavior(
                    new HeroChangeCampaignBehavior());
            }
        }

        /// <summary>
        /// OnMissionBehaviorInitialize（初始化任务行为）
        /// 每次进入 Mission 时注册换将逻辑。
        /// </summary>
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            // AddMissionBehavior（添加任务行为）
            mission.AddMissionBehavior(
                new HeroChangeMissionBehavior());
        }

        /// <summary>
        /// OnSubModuleUnloaded（模块卸载）
        /// 清除属于本 Mod 的 Harmony 补丁。
        /// </summary>
        protected override void OnSubModuleUnloaded()
        {
            if (_harmony != null)
            {
                // UnpatchAll（取消指定 Harmony ID 的全部补丁）
                _harmony.UnpatchAll(HarmonyId);

                _harmony = null;
            }

            base.OnSubModuleUnloaded();
        }
    }
}