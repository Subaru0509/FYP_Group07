using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Edgar.Unity
{
    /// <summary>
    /// Base class for level generators.
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public abstract class LevelGeneratorBase<TPayload> : VersionedMonoBehaviour, ILevelGenerator where TPayload : class
    {
        private readonly Random seedsGenerator = new Random();

        protected readonly PipelineRunner<TPayload> PipelineRunner = new PipelineRunner<TPayload>();

        [Obsolete("The ThrowExceptionImmediately is no longer used. It was previously used inside SmartCoroutine but that piece of code was removed.")]
        protected abstract bool ThrowExceptionImmediately { get; }

        public bool EnableDiagnostics = false;

        protected virtual (Random, int) GetRandomNumbersGenerator(bool useRandomSeed, int seed)
        {
            if (useRandomSeed)
            {
                seed = seedsGenerator.Next();
            }

            Debug.Log($"Random generator seed: {seed}");

            return (new Random(seed), seed);
        }

        public virtual object Generate()
        {
            Debug.Log($"--- Generator started (Edgar v{AssetInfo.Version}) ---");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var (pipelineItems, payload) = GetPipelineItemsAndPayload();

            PipelineRunner.Run(pipelineItems, payload, EnableDiagnostics);

            Debug.Log($"--- Level generated in {stopwatch.ElapsedMilliseconds / 1000f:F}s ---");

            return payload;
        }

        public virtual IEnumerator GenerateCoroutine()
        {
            Debug.Log("--- Generator started ---");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var (pipelineItems, payload) = GetPipelineItemsAndPayload();

            var pipelineIterator = PipelineRunner.GetEnumerator(pipelineItems, payload, EnableDiagnostics);

            if (Application.isPlaying)
            {
                yield return pipelineIterator;
            }
            else
            {
                while (pipelineIterator.MoveNext())
                {
                }
            }
            
            yield return payload;

            Debug.Log($"--- Level generated in {stopwatch.ElapsedMilliseconds / 1000f:F}s ---");
        }

        protected abstract (List<IPipelineTask<TPayload>> pipelineItems, TPayload payload) GetPipelineItemsAndPayload();
    }
}