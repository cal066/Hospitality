using HarmonyLib;
using Verse;

namespace Hospitality.Harmony
{
    public static class ITab_Pawn_Guest_Patch
    {
        [HarmonyPatch(typeof(RimWorld.ITab_Pawn_Guest))]
        [HarmonyPatch(nameof(ITab_Pawn_Guest.IsVisible), MethodType.Getter)]
        public static class IsVisible
        {
            // Added so guests will not show vanilla guest tab
            [HarmonyPostfix]
            public static void Postfix(RimWorld.ITab_Pawn_Guest __instance, ref bool __result)
            {
                var selPawn = __instance.SelPawn;
                __result &= !selPawn.IsGuest();
            }
        }
    }
}
