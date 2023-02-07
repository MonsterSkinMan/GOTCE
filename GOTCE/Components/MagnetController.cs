using UnityEngine;
using RoR2;
using RoR2.Projectile;

namespace GOTCE.Components {
    public class MagnetController : MonoBehaviour {
        public float distance = 10f;
        public List<ProjectileController> projectiles = new();
        private float searchDelay = 0.06f;
        private float stopwatch = 0f;
        public List<ProjectileController> modified = new();

        private void FixedUpdate() {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= searchDelay) {
                stopwatch = 0f;
                projectiles = GameObject.FindObjectsOfType<ProjectileController>().ToList();
            }

            foreach (ProjectileController controller in projectiles.Where(x => Vector3.Distance(base.transform.position, x.transform.position) <= distance && !modified.Contains(x))) {
                Vector3 targetPosition = base.transform.position + (Random.insideUnitSphere * 0.5f);
                controller.transform.LookAt(targetPosition);
                modified.Add(controller);
            }
        }
    }
}