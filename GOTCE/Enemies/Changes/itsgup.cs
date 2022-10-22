using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using GOTCE.Items.Yellow;

namespace GOTCE.Enemies.Changes
{
    public class Itsgup
    {
        public static void OhTheMisery()
        {
            GameObject gupBodyPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody");
            DeathRewards deathRewards = gupBodyPrefab.GetComponent<DeathRewards>();
            var item = new SerializablePickupIndex()
            {
                pickupName = "ItemIndex.ITEM_GOTCE_NeverEndingAgony"
            };
            ExplicitPickupDropTable dt = ScriptableObject.CreateInstance<ExplicitPickupDropTable>();
            dt.pickupEntries = new ExplicitPickupDropTable.PickupDefEntry[]
            {
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = NeverEndingAgony.Instance.ItemDef, pickupWeight = 1f},
            };
            deathRewards.bossDropTable = dt;
        }
        public static void SoMyMainGoalIsToBlowUp() {
            GameObject jellyfishBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Jellyfish/JellyfishBody.prefab").WaitForCompletion();
            DeathRewards deathRewards = jellyfishBodyPrefab.GetComponent<DeathRewards>();
            ExplicitPickupDropTable dt = ScriptableObject.CreateInstance<ExplicitPickupDropTable>();
            dt.pickupEntries = new ExplicitPickupDropTable.PickupDefEntry[]
            {
                new ExplicitPickupDropTable.PickupDefEntry {pickupDef = ViscousBlast.Instance.ItemDef, pickupWeight = 1f},
            };
            deathRewards.bossDropTable = dt;
        }
    }
}