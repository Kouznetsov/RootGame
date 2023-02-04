using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Levels List", menuName = "Levels/List", order = 1)]
    public class LevelsListSo : ScriptableObject
    {
        public List<LevelSo> levelsList;
    }
}