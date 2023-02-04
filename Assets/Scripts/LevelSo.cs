using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Level", menuName = "Levels/New Level", order = 1)]
    public class LevelSo : ScriptableObject
    {
        public int levelIndex;
        public int width;
        public int height;
        public int gcValue;
        public int gcFrequency;
        public int maxTime;
        public int fillBarBySeconds;
        public int endIndex = -1;
        public int startIndex = -1;
    }
}
