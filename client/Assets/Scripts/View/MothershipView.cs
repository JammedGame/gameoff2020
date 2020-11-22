using Logic;
using Settings;

namespace View
{
    public class MothershipView : BattleView<MothershipView>
    {
        public Mothership Mothership { get; set; }

        public static MothershipView Create(Mothership mothership)
        {
            var newView = FetchFromPool();
            if (newView == null) newView = mothership.Type.LoadView();
            newView.Mothership = mothership;
            return newView;
        }

        private void Update()
        {
            transform.localPosition = Mothership.Position;
            transform.localRotation = Mothership.Rotation;
        }
    }
}