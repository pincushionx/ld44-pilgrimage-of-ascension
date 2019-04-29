
namespace Pincushion.LD44
{
    public struct PaperDoll
    {
        public bool headSacrificed;
        public bool torsoSacrificed;
        public bool leftArmSacrificed;
        public bool rightArmSacrificed;
        public bool leftLegSacrificed;
        public bool rightLegSacrificed;

        public float bloodLevel; // 1 for full, 0 for empty

        public void InitializeEmpty()
        {
            headSacrificed = false;
            torsoSacrificed = false;
            leftArmSacrificed = false;
            rightArmSacrificed = false;
            leftLegSacrificed = false;
            rightLegSacrificed = false;
            bloodLevel = 1f;
        }
    }
}