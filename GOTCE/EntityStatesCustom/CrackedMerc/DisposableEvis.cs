using RoR2;
using EntityStates;
using R2API;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Rewired.ComponentControls.Effects;
using EntityStates.Merc.Weapon;
using EntityStates.Merc;
using HG;
using RoR2.Orbs;

namespace GOTCE.EntityStatesCustom.CrackedMerc {
    public class DisposableEvis : BaseState {
        public float damageCoeff = 3.9f;
        public float hitTimerSlash;
        public float hitTimerShock;
        public float slashDelay = 0.2f;
        public float shockDelay = 0.1f;
        public float duration = 3f;
        public float spawnEffectTimer = 0f;
        public float spawnEffectDelay = 0.1f;
        public GameObject model;
        public override void OnEnter()
        {
            base.OnEnter();
            _ = new Evis(); // make sure evis fields have been set

            model = base.GetModelTransform().gameObject;
            model.SetActive(false);
            base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            characterBody.baseMoveSpeed *= 3f;
            characterBody.statsDirty = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            hitTimerSlash -= Time.fixedDeltaTime;
            hitTimerShock -= Time.fixedDeltaTime;

            if (hitTimerSlash <= 0f)
            {
                hitTimerSlash = slashDelay;
                Slash();
                FireMissile();
            }

            if (hitTimerShock <= 0f)
            {
                hitTimerShock = shockDelay;
                Shock();
            }

            spawnEffectTimer -= Time.fixedDeltaTime;
            if (spawnEffectTimer <= 0f)
            {
                spawnEffectTimer = spawnEffectDelay;
                EffectManager.SimpleImpactEffect(Evis.hitEffectPrefab, base.transform.position, Random.insideUnitCircle.normalized, false);
            }

            if (base.fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public void Shock() {
            if (!NetworkServer.active) return;

            foreach (HurtBox box in GetTargets(20f)) {
                LightningOrb lightningOrb = new();
                lightningOrb.origin = base.transform.position;
                lightningOrb.target = box;
                lightningOrb.damageValue = damageStat * damageCoeff;
                lightningOrb.isCrit = base.RollCrit();
                lightningOrb.teamIndex = base.GetTeam();
                lightningOrb.attacker = base.gameObject;
                lightningOrb.bouncesRemaining = 3;
                lightningOrb.procChainMask = default;
                lightningOrb.procCoefficient = 2f;
                lightningOrb.lightningType = LightningOrb.LightningType.Tesla;
                lightningOrb.damageType = DamageType.ApplyMercExpose;

                OrbManager.instance.AddOrb(lightningOrb);
            }
        }

        public void Slash() {
            if (!NetworkServer.active) return;

            foreach (HurtBox box in GetTargets(5f)) {
                if (box.healthComponent) {
                    DamageInfo info = new();
                    info.damage = damageStat * damageCoeff;
                    info.attacker = base.gameObject;
                    info.inflictor = base.gameObject;
                    info.force = Vector3.zero;
                    info.crit = base.RollCrit();
                    info.procChainMask = default;
                    info.procCoefficient = 2f;
                    info.position = box.transform.position;
                    
                    box.healthComponent.TakeDamage(info);
                    GlobalEventManager.instance.OnHitAll(info, box.healthComponent.gameObject);
                    GlobalEventManager.instance.OnHitEnemy(info, box.healthComponent.gameObject);

                    EffectManager.SimpleImpactEffect(Evis.hitEffectPrefab, box.transform.position, Random.insideUnitCircle.normalized, false);
                }
            }
        }

        public HurtBox[] GetTargets(float range) {
            SphereSearch sphereSearch = new SphereSearch();
            sphereSearch.mask = LayerIndex.entityPrecise.mask;
            sphereSearch.origin = base.transform.position;
            sphereSearch.radius = range;
            sphereSearch.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
            sphereSearch.RefreshCandidates();
            sphereSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(base.GetTeam()));
            sphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
            sphereSearch.OrderCandidatesByDistance();
            return sphereSearch.GetHurtBoxes();
        }

        public void FireMissile() {
            if (base.isAuthority)
            {
                MissileUtils.FireMissile(base.transform.position, characterBody, default, null, damageStat * DisposableRising.baseDamageCoefficient, base.RollCrit(), Utils.Paths.GameObject.MissileProjectile.Load<GameObject>(), DamageColorIndex.Default, false);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
            characterBody.baseMoveSpeed /= 3f;
            characterBody.statsDirty = true;
            model.SetActive(true);
        }
    }
}