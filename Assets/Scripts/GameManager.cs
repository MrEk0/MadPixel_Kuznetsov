using System;
using System.Collections.Generic;
using Windows;
using Configs;
using Enums;
using JetBrains.Annotations;
using Save;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettingsData _gameSettingsData;
    [SerializeField] private SlotItemSettingsData _slotItemSettings;
    [SerializeField] private GameWindow _gameWindow;
    [SerializeField] private DropItemWindow _dropItemWindow;

    [CanBeNull] private SaveSystem _saveSystem;
    [CanBeNull] private SlotItemSettingsData.SlotItemSettings _currentSlot;

    private float _timer;

    private void OnEnable()
    {
        _gameWindow.PlayEvent += OnPlayClicked;
        _dropItemWindow.TakeEvent += OnItemTaken;
        _dropItemWindow.SellEvent += OnItemSold;
    }

    private void OnDisable()
    {
        _gameWindow.PlayEvent -= OnPlayClicked;
        _dropItemWindow.TakeEvent -= OnItemTaken;
        _dropItemWindow.SellEvent -= OnItemSold;
    }

    private void Start()
    {
        _saveSystem = new SaveSystem();

        var gameWindowSetup = new GameWindowSetup
        {
            MaxSliderValue = _gameSettingsData.MaxManaCount
        };

        if (_saveSystem.SaveData.IsFirstEntrance)
        {
            _saveSystem.SaveData.IsFirstEntrance = false;
            _saveSystem.SaveData.ManaCount = _gameSettingsData.MaxManaCount;
            _saveSystem.SaveData.GoldCount = 0;
            _saveSystem.SaveData.LastManaUpdate.value = DateTime.Now;

            gameWindowSetup.GoldCount = 0;
            gameWindowSetup.ManaCount = _saveSystem.SaveData.ManaCount;
            gameWindowSetup.SlotsDictionary = new Dictionary<Slots, WindowSlotItemData>();

            return;
        }

        var manaCount = _saveSystem.SaveData.ManaCount + Mathf.FloorToInt((float)(DateTime.Now - _saveSystem.SaveData.LastManaUpdate.value).TotalSeconds * _gameSettingsData.ManaPerSeconds);

        gameWindowSetup.GoldCount = _saveSystem.SaveData.GoldCount;
        gameWindowSetup.ManaCount = Mathf.Min(_gameSettingsData.MaxManaCount, manaCount);
        gameWindowSetup.SlotsDictionary = _saveSystem.SaveData.SlotItems;

        _gameWindow.Init(gameWindowSetup);
    }

    private void Update()
    {
        if (_saveSystem == null)
            return;

        if (_saveSystem.SaveData.ManaCount == _gameSettingsData.MaxManaCount)
        {
            _timer = 0f;
            return;
        }

        _timer += Time.deltaTime;
        var extraMana = Mathf.FloorToInt(_timer * _gameSettingsData.ManaPerSeconds);
        if (extraMana < 1)
            return;

        _timer = 0f;
        _saveSystem.SaveData.ManaCount = Mathf.Min(_gameSettingsData.MaxManaCount, _saveSystem.SaveData.ManaCount + extraMana);
        _gameWindow.UpdateSlider(_saveSystem.SaveData.ManaCount);
    }

    private void OnPlayClicked()
    {
        if (_saveSystem == null)
            return;

        if (_saveSystem.SaveData.ManaCount <= 0)
            return;

        if (_slotItemSettings.SlotItemSettingsList.Count == 0)
            return;

        _saveSystem.SaveData.LastManaUpdate.value = DateTime.Now;
        _saveSystem.SaveData.ManaCount = Mathf.Max(0, _saveSystem.SaveData.ManaCount - _gameSettingsData.ManaPerPlay);

        var index = Random.Range(0, _slotItemSettings.SlotItemSettingsList.Count);
        _currentSlot = _slotItemSettings.SlotItemSettingsList[index];
        if (_currentSlot == null)
            return;

        var windowSetup = new DropItemWindowSetup
        {
            ItemSprite = _currentSlot.ItemSprite,
            ItemLevel = _currentSlot.Level
        };
        _dropItemWindow.Init(windowSetup);
    }

    private void OnItemTaken()
    {
        if (_saveSystem == null)
            return;

        if (_currentSlot == null)
            return;

        _saveSystem.SaveData.ChangeSlot(_currentSlot.Slot, new WindowSlotItemData
        {
            IsTaken = true,
            ItemSprite = _currentSlot.ItemSprite,
            Level = _currentSlot.Level
        });

        UpdateWindow();
    }

    private void OnItemSold()
    {
        if (_saveSystem == null)
            return;

        if (_currentSlot == null)
            return;

        _saveSystem.SaveData.GoldCount += _currentSlot.SellGold;

        UpdateWindow();
    }

    private void UpdateWindow()
    {
        if (_saveSystem == null)
            return;
        
        var manaCount = _saveSystem.SaveData.ManaCount + (float)(DateTime.Now - _saveSystem.SaveData.LastManaUpdate.value).TotalSeconds * _gameSettingsData.ManaPerSeconds;
        _saveSystem.SaveData.ManaCount = Mathf.Min(_gameSettingsData.MaxManaCount, Mathf.FloorToInt(manaCount));
            
        var gameWindowSetup = new GameWindowSetup
        {
            MaxSliderValue = _gameSettingsData.MaxManaCount,
            GoldCount = _saveSystem.SaveData.GoldCount,
            ManaCount = _saveSystem.SaveData.ManaCount,
            SlotsDictionary = _saveSystem.SaveData.SlotItems
        };

        _gameWindow.Init(gameWindowSetup);
    }

    private void OnDestroy()
    {
        _saveSystem?.Save();
    }
}

