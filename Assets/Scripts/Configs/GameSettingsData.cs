using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "MadPixel/Configs/GameSettingsData")]
    public class GameSettingsData : ScriptableObject
    {
        [SerializeField] private int _maxManaCount;
        [SerializeField] private int _manaPerPlay;
        [SerializeField] private float _manaPerMinute;

        public int MaxManaCount => _maxManaCount;

        public int ManaPerPlay => _manaPerPlay;

        public float ManaPerSeconds => _manaPerMinute / 60f;
    }
}
