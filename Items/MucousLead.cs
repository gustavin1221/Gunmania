using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.StatAPI;

namespace GunMania
{
    public class MucousLead : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Add()
        {
            //The name of the item
            string itemName = "Mucous Lead";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "GunMania/Resources/Items/mucous_lead";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<MucousLead>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Ew!";
            string longDesc = "The first gungeoneer to find this ancient bullet modifier fell 15 times while trying to pick it up from its mucous pool.\n\n" +
                "Shots get a high chance to inflict the slow effect.\n\n\n" +
                "''Slugs - Skilotar''\n\n\n" +
                "-Gunmania-";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "gunmania");

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }
        public void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
            // yourPoisonApplyChance should be between 0f (0%) and 1f (100%)
            if (UnityEngine.Random.value <= 0.9f)
            projectile.statusEffectsToApply.Add((PickupObjectDatabase.GetById(381) as Gun).DefaultModule.projectiles[0].speedEffect);
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