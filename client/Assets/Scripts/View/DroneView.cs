using Logic;
using Settings;

namespace View
{
    public class DroneView : BattleView<DroneView>
    {
        public Drone Drone { get; set; }

        public static DroneView Create(Drone drone)
        {
            var newView = FetchFromPool();
            if (newView == null) newView = drone.Type.LoadView();
            newView.Drone = drone;
            return newView;
        }

        private void Update()
        {
            transform.localPosition = Drone.Position;
            transform.localRotation = Drone.Rotation;
        }
    }
}