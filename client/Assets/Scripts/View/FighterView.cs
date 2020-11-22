using UnityEngine;
using Logic;

namespace View
{
    public class FighterView : BattleView<FighterView>
    {
        public float smoothMovement = 0.8f;

        public Fighter Fighter { get; set; }

        public static FighterView Create(Fighter fighter)
        {
            var newView = FetchFromPool();
            if (newView == null) newView = GameObject.Instantiate(Resources.Load<FighterView>("Prefabs/FighterView"));
            newView.Fighter = fighter;
            newView.transform.localPosition = fighter.Position;
            return newView;
        }

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Fighter.Position, 1 - smoothMovement);
            transform.localRotation = Fighter.Rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet")) 
            {
                var planet = other.GetComponentInParent<PlanetView>()?.Planet;
                Fighter.GetKilled(planet);
            }
            else if (other.CompareTag("Mothership"))
            {
                var mothership = other.GetComponentInParent<MothershipView>()?.Mothership;
                Fighter.GetKilled(mothership);
            }
        }
    }
}