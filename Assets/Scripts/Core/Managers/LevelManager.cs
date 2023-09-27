using System;
using System.Collections;
using System.Collections.Generic;
using CaseStudy.Collectables;
using CaseStudy.Core.Managers;
using CaseStudy.Core.Pools;
using CaseStudy.Core.ScriptableObjects;
using CaseStudy.Core.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CaseStudy.Core.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public List<PixelData> storedPixelData = new List<PixelData>();
        public List<Vector3> storedPositions = new List<Vector3>();
        private List<CollectableBase> _spawnedCubes = new List<CollectableBase>();
        [SerializeField] private Material sourceMaterial;
        [SerializeField] private float distanceMultiplier;
        private List<CollectableBase> _collectedCubes = new List<CollectableBase>();

        private List<Material> _materials = new List<Material>();

        private GameState _gameState;
        private Coroutine returnPoolRoutine;

        private void ReadTextureData(Texture2D texture2D)
        {
            storedPixelData.Clear();
            for (int i = 0; i <texture2D.width ; i++)
            {
                for (int j = 0; j < texture2D.height; j++)
                {
                    if(texture2D.GetPixel(i,j).a == 0) continue;
                    // PixelData p = storedPixelData.Find(x => x.Position == new Vector2(i, j));
                    //
                    // if(!p.Equals(default(PixelData))) continue;
                    
                    storedPixelData.Add(new PixelData()
                    {
                        Position = new Vector2(i,j),
                        Color = texture2D.GetPixel(i,j)
                    });            
                }
                
            }
            CalculateLevelArea(texture2D);
        }

        private void CalculateLevelArea(Texture2D texture2D)
        {
            storedPositions.Clear();
            float unitScale = GameData.Instance.AreaWidth / texture2D.width;

            for (int i = 0; i < storedPixelData.Count; i++)
            {
                storedPositions.Add(new Vector3(storedPixelData[i].Position.x - (float)texture2D.width/2,0.5f,
                    storedPixelData[i].Position.y- (float)texture2D.height/2)*unitScale*distanceMultiplier);
            }
            
        }
        private void SpawnLevel()
        {
            for (int i = 0; i < storedPixelData.Count; i++)
            {
                CollectableBase collectableCube = CollectablePool.Instance.Get();
                collectableCube.transform.position = storedPositions[i];
                collectableCube.transform.rotation = Quaternion.Euler(Vector3.zero);
                collectableCube.transform.parent = transform;

                Material m0 = _materials.Find(x => x.GetColor("_Color") == storedPixelData[i].Color);

                if (m0)
                {
                    collectableCube.SetMaterial(m0);
                }
                else
                {
                    Material m = new Material(sourceMaterial);
                    m.enableInstancing = true;
                    m.SetColor("_Color", storedPixelData[i].Color);
                    collectableCube.SetMaterial(m);

                    _materials.Add(m);

                }

                _spawnedCubes.Add(collectableCube);
            }
        }

        private void EventManagerOnOnChangeGameState(GameState obj)
        {
            _gameState = obj;
        }
        
        private void EventManagerOnOnGameStarted()
        {
            Level level = GameData.Instance.GetCurrentLevel();
            
            ReadTextureData(level.LevelTexture);
            SpawnLevel();

            if (_gameState == GameState.TimeChallenge)
            {
                StartCoroutine(TimeChallengeSpawner());
            }
        }

        private IEnumerator TimeChallengeSpawner()
        {
            while (true)
            {
                yield return new WaitUntil(()=>_spawnedCubes.Count < 10);
                GameData.Instance.LevelCompletedForTimeChallenge();
                Level level = GameData.Instance.GetCurrentLevel();
            
                ReadTextureData(level.LevelTexture);
                SpawnLevel();
            }
        }
        
        private void OnOnRemoveCollectedCube(CollectableBase obj)
        {
            if (_spawnedCubes.Contains(obj))
                _spawnedCubes.Remove(obj);

            if (_gameState == GameState.TimeChallenge)
            {
                _collectedCubes.Add(obj);
                StartReturnPoolRoutine();
                Debug.LogError("1");
            }
            
            if(_spawnedCubes.Count<1 && _gameState != GameState.TimeChallenge)
                EventManager.GameEnd();
            
        }

        private void StartReturnPoolRoutine()
        {
            if(returnPoolRoutine != null) return;
            returnPoolRoutine = StartCoroutine(ReturnPoolRoutine());
        }

        IEnumerator ReturnPoolRoutine()
        {
            yield return new WaitForSeconds(2);
            while (_collectedCubes.Count>0)
            {
                yield return new WaitForSeconds(0.02f);
                _collectedCubes[0].ResetItem();
                CollectablePool.Instance.ReturnToPool(_collectedCubes[0]);
                _collectedCubes.RemoveAt(0);
            } 
            
            returnPoolRoutine = null;
        }
        private void OnEnable()
        {
            EventManager.OnGameStarted += EventManagerOnOnGameStarted;
            EventManager.OnGameEnd += EventManagerOnOnGameEnd;
            EventManager.OnChangeGameState += EventManagerOnOnChangeGameState;
            OnRemoveCollectedCube += OnOnRemoveCollectedCube;
        }

        private void EventManagerOnOnGameEnd()
        {
            if(_gameState == GameState.TimeChallenge)
                StopCoroutine(TimeChallengeSpawner());
        }

        private void OnDisable()
        {
            EventManager.OnGameStarted -= EventManagerOnOnGameStarted;
            EventManager.OnGameEnd -= EventManagerOnOnGameEnd;
            EventManager.OnChangeGameState -= EventManagerOnOnChangeGameState;
            OnRemoveCollectedCube -= OnOnRemoveCollectedCube;
        }
        
        #region events

        public static event Action<CollectableBase> OnRemoveCollectedCube;

        public static void RemoveCollectableCube(CollectableBase cube)
        {
            OnRemoveCollectedCube?.Invoke(cube);
        }
        #endregion
    }
    
    [Serializable]
    public struct PixelData
    {
        public Vector2 Position;
        public Color Color;
    }
}