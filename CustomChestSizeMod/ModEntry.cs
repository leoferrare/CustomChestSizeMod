using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Objects;

namespace CustomChestSizeMod
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
                original: AccessTools.Method(typeof(Chest), nameof(Chest.GetActualCapacity)),
                postfix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.Chest_GetActualCapacity_Postfix))
            );
        }

        private static void Chest_GetActualCapacity_Postfix(Chest __instance, ref int __result)
        {
            if (StardewValley.Game1.bigCraftableData != null && 
                StardewValley.Game1.bigCraftableData.TryGetValue(__instance.ItemId, out var data) && 
                data.CustomFields != null && 
                data.CustomFields.TryGetValue("Darkiriq30.CustomChestSizeMod/Capacity", out string rawCapacity) && 
                int.TryParse(rawCapacity, out int capacity))
            {
                __result = capacity;
            }
        }
    }
}
