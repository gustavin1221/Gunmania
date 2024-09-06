using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.StatAPI;

namespace GunMania
{
    public class EncheesedLead : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Add()
        {
            //The name of the item
            string itemName = "Encheesed Lead";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "GunMania/Resources/Items/cheesy_bullets";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<EncheesedLead>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Rat's Kryptonite";
            string longDesc = "The resourceful rat one created these bullets to defeat one of his great enemies, turns out, he actually ate them instead. Blast Eddie found them years later in the gungeon's sewers.\n\n" +
                "Adds a chance for shots to inflict the cheese effect!\n\n\n" +
                "-Gunmania-";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "gunmania");

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
        }
        public void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
            // yourPoisonApplyChance should be between 0f (0%) and 1f (100%)
            if (UnityEngine.Random.value <= 0.75f)
            projectile.statusEffectsToApply.Add((PickupObjectDatabase.GetById(626) as Gun).DefaultModule.projectiles[0].cheeseEffect);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Plugin.Log($"Player picked up {DisplayName}");

            player.PostProcessProjectile += PostProcessProjectile;
        }

        public override void DisableEffect(PlayerController player)
        {
            Plugin.Log($"Player dropped or got rid of {DisplayName}");

            if (player == null)
                return;

            player.PostProcessProjectile -= PostProcessProjectile;
        }
    }
}