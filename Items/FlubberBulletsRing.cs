using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.StatAPI;

namespace GunMania
{
    public class FlubberRing : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Add()
        {
            //The name of the item
            string itemName = "Flubber Bullets Ring";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "GunMania/Resources/Items/flubber_ring";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<FlubberRing>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "?";
            string longDesc = "Makes your shots bouncy and poisonous!";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "gunmania");

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }
        public void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
            if (UnityEngine.Random.value <= 1f)
                projectile.statusEffectsToApply.Add((PickupObjectDatabase.GetById(204) as BulletStatusEffectItem).HealthModifierEffect);

            BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            bounce.numberOfBounces = Mathf.Max(4, bounce.numberOfBounces);
            bounce.chanceToDieOnBounce = 0f;
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