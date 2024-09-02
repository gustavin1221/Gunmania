using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunMania
{
    public class FrogRing : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Add()
        {
            //The name of the item
            string itemName = "Frog Ring";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "GunMania/Resources/Items/frog_ring";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<FrogRing>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Ribbit";
            string longDesc = "Brought to the gungeon by Sir Frogger the 3rd.\n\n" +
                "Makes ur dodgeroll faster and more powerful and increases movement speed by 1, like a frog!";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "gustavin");

            //Adds the actual passive effect to the item    
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DodgeRollSpeedMultiplier, 0.23f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DodgeRollDistanceMultiplier, 0.23f);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 1f);

            //Set the rarity of the item
item.quality = PickupObject.ItemQuality.C;

            List<string> mandatoryConsoleIDs = new List<string>
{
"gustavin:frog_ring",
};
            List<string> optionalConsoleIDs = new List<string>
{
    "gustavin:frog_ring"
};
            CustomSynergies.Add("Green Bouncy Fishes!", mandatoryConsoleIDs, optionalConsoleIDs, true);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Plugin.Log($"Player picked up {DisplayName}");
        }

        public override void DisableEffect(PlayerController player)
        {
            Plugin.Log($"Player dropped or got rid of {DisplayName}");
        }
    }
}