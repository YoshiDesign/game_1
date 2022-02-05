
namespace Custom {

    class Helpers {
        public const int HOMING_L0 = 2;
        public const int HOMING_L1 = 4;
        public const int HOMING_L2 = 8;
        private static int max_homing_targets = 2;
        public const float max_dist = 5500f;

        public static int getMaxHomingTargets()
        {
            return max_homing_targets;
        }
        public static void setMaxHomingTargets(int n)
        {
            max_homing_targets = n;
        }
        
    }


}