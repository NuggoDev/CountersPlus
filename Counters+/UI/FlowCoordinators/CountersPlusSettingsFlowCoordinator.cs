﻿using BeatSaberMarkupLanguage;
using CountersPlus.ConfigModels;
using CountersPlus.UI.ViewControllers;
using CountersPlus.Utils;
using HMUI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CountersPlus.UI.FlowCoordinators
{
    public class CountersPlusSettingsFlowCoordinator : FlowCoordinator
    {
        [Inject] public List<ConfigModel> AllConfigModels;
        [Inject] private CanvasUtility canvasUtility;

        [Inject] private CountersPlusCreditsViewController credits;
        [Inject] private CountersPlusBlankViewController blank;
        [Inject] private CountersPlusSettingSectionSelectionViewController settingsSelection;
        [Inject] private CountersPlusHorizontalSettingsListViewController horizontalSettingsList;

        private TweenPosition mainScreenTween;
        private Vector3 mainScreenOrigin;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation && activationType == ActivationType.AddedToHierarchy)
            {
                Transform mainScreenTransform = GameObject.Find("MainScreen").transform;
                mainScreenTween = mainScreenTransform.gameObject.AddComponent<TweenPosition>();
                mainScreenTween._duration = 1f;
                mainScreenOrigin = mainScreenTransform.position;

                Plugin.Logger.Warn($"Loading options menu with {AllConfigModels.Count} config models...");

                showBackButton = true;
                title = "Counters+";
            }
            SetMainScreenOffset(new Vector3(0, -4, 0));

            PushViewControllerToNavigationController(settingsSelection, horizontalSettingsList);

            ProvideInitialViewControllers(blank, credits, null, settingsSelection);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            SetMainScreenOffset(Vector3.zero);
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
            canvasUtility.ClearAllText();
        }

        internal void SetMainScreenOffset(Vector3 offset, float duration = 1)
        {
            mainScreenTween._duration = duration;
            mainScreenTween.TargetPos = mainScreenOrigin + offset;
        }
    }
}
