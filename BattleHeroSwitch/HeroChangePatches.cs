using HarmonyLib;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Localization;

namespace BattleHeroSwitch
{
    /// <summary>
    /// 原版战斗按钮硬编码检查 Hero.MainHero.IsWounded。
    /// 已选择健康英雄时，只撤销“原主角受伤”造成的禁用，其余限制保持不变。
    /// </summary>
    [HarmonyPatch(typeof(MenuHelper), nameof(MenuHelper.EncounterAttackCondition))]
    internal static class HeroChangeEncounterAttackConditionPatch
    {
        private static readonly TextObject WoundedTooltip =
            new TextObject("{=UL8za0AO}You are wounded.");

        [HarmonyPostfix]
        private static void Postfix(MenuCallbackArgs args)
        {
            if (CanOverrideWoundedRestriction(args, WoundedTooltip))
                args.IsEnabled = true;
        }

        internal static bool CanOverrideWoundedRestriction(
            MenuCallbackArgs args,
            TextObject woundedTooltip)
        {
            return HeroChangeCampaignBehavior.Current?.HasHealthySelectedHero == true &&
                   Hero.MainHero != null &&
                   Hero.MainHero.IsWounded &&
                   args?.Tooltip != null &&
                   args.Tooltip.HasSameValue(woundedTooltip);
        }
    }

    [HarmonyPatch(typeof(HideoutCampaignBehavior), "game_menu_hideout_sneak_in_on_condition")]
    internal static class HeroChangeHideoutSneakInConditionPatch
    {
        private static readonly TextObject WoundedTooltip =
            new TextObject("{=pM9GOxrV}You are wounded, you can't sneak in!");

        [HarmonyPostfix]
        private static void Postfix(MenuCallbackArgs args)
        {
            if (HeroChangeEncounterAttackConditionPatch.CanOverrideWoundedRestriction(args, WoundedTooltip))
                args.IsEnabled = true;
        }
    }

    [HarmonyPatch(typeof(HideoutCampaignBehavior), "game_menu_assault_hideout_parties_on_condition")]
    internal static class HeroChangeHideoutAssaultConditionPatch
    {
        private static readonly TextObject WoundedTooltip =
            new TextObject("{=ZCKKVRjT}You are wounded, you can't assault the hideout!");

        [HarmonyPostfix]
        private static void Postfix(MenuCallbackArgs args)
        {
            if (HeroChangeEncounterAttackConditionPatch.CanOverrideWoundedRestriction(args, WoundedTooltip))
                args.IsEnabled = true;
        }
    }

    [HarmonyPatch(typeof(HideoutCampaignBehavior), "OnTroopRosterManageDone")]
    internal static class HeroChangeHideoutRosterPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TroopRoster hideoutTroops, bool isDirectAssault)
        {
            HeroChangeCampaignBehavior.Current?.PrepareHideoutRoster(hideoutTroops, isDirectAssault);
        }
    }

    [HarmonyPatch(typeof(MenuHelper), "LordsHallTroopRosterManageDone")]
    internal static class HeroChangeLordsHallRosterPatch
    {
        [HarmonyPrefix]
        private static void Prefix(TroopRoster selectedTroops)
        {
            HeroChangeCampaignBehavior.Current?.PrepareLordsHallRoster(selectedTroops);
        }
    }

    [HarmonyPatch(typeof(SiegeEventCampaignBehavior), "game_menu_siege_strategies_lead_assault_on_condition")]
    internal static class HeroChangeSiegeAssaultConditionPatch
    {
        private static readonly TextObject WoundedTooltip =
            new TextObject("{=gzYuWR28}You are wounded, and in no condition to lead an assault.");

        [HarmonyPostfix]
        private static void Postfix(MenuCallbackArgs args)
        {
            if (HeroChangeEncounterAttackConditionPatch.CanOverrideWoundedRestriction(args, WoundedTooltip))
                args.IsEnabled = true;
        }
    }

    [HarmonyPatch(typeof(SiegeAmbushCampaignBehavior), "menu_siege_strategies_ambush_condition")]
    internal static class HeroChangeSiegeAmbushConditionPatch
    {
        private static readonly TextObject WoundedTooltip =
            new TextObject("{=pQaQW1As}You cannot ambush right now due to your wounds.");

        [HarmonyPostfix]
        private static void Postfix(MenuCallbackArgs args)
        {
            if (HeroChangeEncounterAttackConditionPatch.CanOverrideWoundedRestriction(args, WoundedTooltip))
                args.IsEnabled = true;
        }
    }
}
