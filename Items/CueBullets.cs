using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Alexandria.StatAPI;

namespace GunMania
{
    public class CueBullets : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Add()
        {
            //The name of the item
            string itemName = "Casey Ring";

            //Refers to an embedded png in the project. Make sure to embed your resources! Google it
            string resourceName = "GunMania/Resources/Items/casey_ring";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<BilliardsStickItem>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "My son!";
            string longDesc = "The gunslinger missed playing billiards while building this place.\n\n" +
                "Makes enemies killed act like when smacked by casey! \n\n" +
                "Yes its the same thing as Cue Bullets.\n\n\n" +
                "-Gunmania-";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "gunmania");

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }
        public void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {

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