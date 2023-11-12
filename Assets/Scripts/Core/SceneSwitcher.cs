﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneSwitcher
    {
        public event Action SceneChangeInitiated;
        public event Action SceneChanged;
        
        private int _currentSceneIndex;
        private int _nextSceneIndex;

        public async UniTask ChangeSceneAsync(Enums.SceneType sceneType, CancellationToken token)
        {
            SceneChangeInitiated?.Invoke();

            _nextSceneIndex = (int) sceneType + 1;

            AsyncOperation loadScene = SceneManager.LoadSceneAsync(_nextSceneIndex, LoadSceneMode.Additive);

            await LoadSceneAsync(loadScene, token);
            
            if (_currentSceneIndex != Constants.StartSceneIndex)
                loadScene.completed += UnloadSceneAsync;

            loadScene.completed -= OnSceneLoadCompleted;
            loadScene.completed -= UnloadSceneAsync;

            _currentSceneIndex = _nextSceneIndex;
        }

        private async UniTask LoadSceneAsync(AsyncOperation loadScene, CancellationToken token)
        {
            loadScene.allowSceneActivation = false;

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            loadScene.completed += OnSceneLoadCompleted;

            UniTask secondDelayTask = UniTask.Delay(TimeSpan.FromSeconds(0f), cancellationToken: token);
            UniTask loadSceneTask = UniTask.WaitUntil(() => loadScene.progress == 0.9f, cancellationToken: token);

            await UniTask.WhenAll(secondDelayTask, loadSceneTask);

            loadScene.allowSceneActivation = true;

            await UniTask.WaitUntil(() => loadScene.isDone, cancellationToken: token);
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;

            SceneChanged?.Invoke();
        }

        private void OnSceneLoadCompleted(AsyncOperation operation)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(_nextSceneIndex);

            SceneManager.SetActiveScene(scene);
        }
        
        private async void UnloadSceneAsync(AsyncOperation obj) => 
            await SceneManager.UnloadSceneAsync(_currentSceneIndex);
    }
}