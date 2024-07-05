using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Common.Models.ParticlesMessage
{
    [RequireComponent(typeof(ParticleSystem))]
    public class MessageParticleSystem : MonoBehaviour
    {
        [SerializeField] private SymbolsTextureData _textureData;
        [SerializeField] private Color _backgroundColor;
        
        private ParticleSystemRenderer _renderer;
        private ParticleSystem _particleSystem;

        private bool _hasChilds;
        private IReadOnlyList<ParticleSystem> _childs;

#if UNITY_EDITOR
        [ContextMenu("Test")]
        public void Test()
        {
            float damage = Random.Range(1, 1000);
            
            Validate();
            SpawnParticle(transform.position, damage.ToString(), Color.white);
        }
#endif

        private void Awake()
        {
            Validate();
        }

        public void SpawnParticle(Vector2 position, string message, Color color)
        {
            Vector2[] coordinates = new Vector2[24];
            int messageLength = Mathf.Min(23, message.Length);

            FillCoordinates(message, ref coordinates, messageLength);

            Vector4 custom1Data = CreateCustomData(coordinates);
            Vector4 custom2Data = CreateCustomData(coordinates, 12);

            Emit(position, color, messageLength);
            SetCustomData(custom1Data, custom2Data);
        }

        public float PackFloat(IReadOnlyList<Vector2> vectors)
        {
            if (vectors == null || vectors.Count == 0)
                return 0;

            float result = vectors[0].y * 10000 + vectors[0].x * 100000;

            if (vectors.Count > 1)
                result += vectors[1].y * 100 + vectors[1].x * 1000;
            if (vectors.Count > 2)
                result += vectors[2].y + vectors[2].x * 10;

            return result;
        }

        private void Validate()
        {
            if (_particleSystem == null)
                _particleSystem = GetComponent<ParticleSystem>();

            if (_renderer == null)
            {
                _renderer = _particleSystem.GetComponent<ParticleSystemRenderer>();

                List<ParticleSystemVertexStream> streams = new List<ParticleSystemVertexStream>();

                _renderer.GetActiveVertexStreams(streams);
                
                if (streams.Contains(ParticleSystemVertexStream.UV2) == false) 
                    streams.Add(ParticleSystemVertexStream.UV2);
                if (streams.Contains(ParticleSystemVertexStream.Custom1XYZW) == false) 
                    streams.Add(ParticleSystemVertexStream.Custom1XYZW);
                if (streams.Contains(ParticleSystemVertexStream.Custom2XYZW) == false) 
                    streams.Add(ParticleSystemVertexStream.Custom2XYZW);

                _renderer.SetActiveVertexStreams(streams);
            }

            _hasChilds = _particleSystem.transform.childCount > 0;
            
            if (_hasChilds)
                _childs = _particleSystem.transform.GetChilds().Select(t => t.GetComponent<ParticleSystem>()).ToList();
        }
        
        private void FillCoordinates(string message, ref Vector2[] coordinates, int messageLength)
        {
            coordinates[^1] = new Vector2(0, messageLength);

            for (var i = 0; i < coordinates.Length; i++)
            {
                if (i >= messageLength)
                    break;

                coordinates[i] = _textureData.GetTextureCoordinates(message[i]);
            }
        }
        
        private void Emit(Vector2 position, Color color, int messageLength)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
            {
                startColor = color,
                position = position,
                applyShapeToPosition = true,
                startSize3D = new Vector3(messageLength, 1, 1)
            };

            _particleSystem.Play();
            _particleSystem.Emit(emitParams, 1);
            
            if (_hasChilds == false)
                return;
            
            EmitChilds(emitParams);
        }

        private void EmitChilds(ParticleSystem.EmitParams emitParams)
        {
            Vector3 rotation = new Vector3(0, 0, Random.Range(-10, 10));
                
            emitParams.startColor = _backgroundColor;
            emitParams.rotation3D = rotation;
                
            foreach (var child in _childs)
            {
                child.Play();
                child.Emit(emitParams, 1);
            }
        }

        private void SetCustomData(Vector4 custom1Data, Vector4 custom2Data)
        {
            List<Vector4> customData = new List<Vector4>();

            _particleSystem.GetCustomParticleData(customData, ParticleSystemCustomData.Custom1);
            customData[^1] = custom1Data;
            _particleSystem.SetCustomParticleData(customData, ParticleSystemCustomData.Custom1);
            
            _particleSystem.GetCustomParticleData(customData, ParticleSystemCustomData.Custom2);
            customData[^1] = custom2Data;
            _particleSystem.SetCustomParticleData(customData, ParticleSystemCustomData.Custom2);
        }
        
        private Vector4 CreateCustomData(IReadOnlyList<Vector2> coordinates, int offset = 0)
        {
            Vector4 data = Vector4.zero;

            for (int i = 0; i < 4; i++)
            {
                Vector2[] vectors = new Vector2[3];

                for (int j = 0; j < 3; j++)
                {
                    int index = i * 3 + j + offset;

                    if (coordinates.Count > index)
                    {
                        vectors[j] = coordinates[index];
                    }
                    else
                    {
                        data[i] = PackFloat(vectors);
                        i = 5;
                        
                        break;
                    }
                }

                if (i < 4)
                    data[i] = PackFloat(vectors);
            }

            return data;
        }
    }
}