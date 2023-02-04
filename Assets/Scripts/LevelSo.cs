using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Level", menuName = "Levels/New Level", order = 1)]
    public class LevelSo : ScriptableObject
    {
        public string levelName;
        public int width;
        public int height;
        public int gcValue;
        public int gcFrequency;
        public int maxTime;
        public int fillBarBySeconds;
    }
}
