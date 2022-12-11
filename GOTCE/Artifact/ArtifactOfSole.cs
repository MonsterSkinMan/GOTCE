using BepInEx.Configuration;
using UnityEngine;
using RoR2;
using R2API;

namespace GOTCE.Artifact
{
    public class ArtifactOfSole : ArtifactBase<ArtifactOfSole>
    {
        public override string ArtifactName => "Artifact Of Sole";

        public override string ArtifactLangTokenName => "GOTCE_ArtifactOfSole";

        public override string ArtifactDescription => "Allies bleed out while moving on the ground and have reduced jump height.";

        public override Sprite ArtifactEnabledIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Artifacts/artifactofsoleon.png");

        public override Sprite ArtifactDisabledIcon => Main.SecondaryAssets.LoadAsset<Sprite>("Assets/Icons/Artifacts/artifactofsoleoff.png");

        public override void Init(ConfigFile config)
        {
            CreateLang();
            CreateArtifact();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.Start += (orig, self) =>
            {
                orig(self);
                if (RunArtifactManager.instance.IsArtifactEnabled(ArtifactDef) && NetworkServer.active && self.playerCharacterMasterController)
                {
                    Weak com = self.gameObject.GetComponent<Weak>();
                    if (!com)
                    {
                        self.gameObject.AddComponent<Weak>();
                    }
                }
            };
            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                if (sender.isPlayerControlled && RunArtifactManager.instance.IsArtifactEnabled(ArtifactDef))
                {
                    args.jumpPowerMultAdd -= 1f / 6f;
                    args.baseJumpPowerAdd -= 6f;
                    // half jump height
                }
            };
        }
    }

    public class Weak : MonoBehaviour
    {
        private float damageMult = 0.2f;
        private float delay = 0.5f;
        private float stopwatch = 0f;
        private CharacterMaster master;

        public void Start()
        {
            master = gameObject.GetComponent<CharacterMaster>();
        }

        public void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= delay)
                {
                    stopwatch = 0f;
                    CharacterBody body = master.GetBody();
                    if (body && body.characterMotor)
                    {
                        float damage = body.characterMotor.velocity.magnitude * damageMult;
                        if (body.characterMotor.isGrounded)
                        {
                            DamageInfo info = new()
                            {
                                attacker = null,
                                inflictor = null,
                                damageType = DamageType.NonLethal,
                                damage = damage,
                                damageColorIndex = DamageColorIndex.Bleed,
                                position = gameObject.transform.position
                            };

                            body.healthComponent.TakeDamage(info);

                            body.AddTimedBuff(RoR2Content.Buffs.Bleeding, 1f);
                        }
                    }
                }
            }
        }
    }
}