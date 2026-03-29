using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Edgar.Unity.Examples.Metroidvania
{
    public class MetroidvaniaGameManager : GameManagerBase<MetroidvaniaGameManager>
    {
       
       
        public MetroidvaniaLevelType LevelType;
        private long generatorElapsedMilliseconds;

        // To make sure that we do not start the generator multiple times
        private bool isGenerating;

        public static readonly string LevelMapLayer = "LevelMap";
        public static readonly string StaticEnvironmentLayer = "StaticEnvironment";


        public PlatformerGeneratorGrid2D grid2D;
        public PlatformerGeneratorGrid2D grid2D2;

        private PlatformerGeneratorGrid2D generator;
        public  int loadtime =0;
        private string loadtext;

        public GameObject Bossroom;
        public GameObject loadimage;

        protected override void SingletonAwake()
        {
            if (LayerMask.NameToLayer(StaticEnvironmentLayer) == -1)
            {
                throw new Exception($"\"{StaticEnvironmentLayer}\" layer is needed for this example to work. Please create this layer.");
            }

          //  LoadNextLevel();
        }

        public void Update()
        {
         
            if (InputHelper.GetKeyDown(KeyCode.U))
            {
                Canvas.SetActive(!Canvas.activeSelf);
            }
        }

        public override void LoadNextLevel()
        {
            
            isGenerating = true;
            loadtime++;

            
         

            switch (loadtime)
            {
                case 1:

                    loadtext = "Layer One";
                 
                    break;
                case 2:
                    loadtext = "Layer Two";
                    break;
                case 3:
                    loadtext = "Boss Room";
                    break;

            }

            // Show loading screen
            ShowLoadingScreen($"Shadow Castle - {loadtext}" , "Loading···");


          
            // Find the generator runner
            switch (loadtime)
            {
                case 1:
                     generator = grid2D;
                    StartCoroutine(GeneratorCoroutine(generator));
                    break;
                case 2:
                    generator = grid2D2;
                    StartCoroutine(GeneratorCoroutine(generator));
                    break;
                case 3:
                    Instantiate(Bossroom);
                    Invoke("bossrommpa", 1f);
                    break;

            }
            
               
            // Start the generator coroutine
          
            
        }

        private void bossrommpa()
        {
            loadimage.SetActive(false);
           // GameObject aimed = GameObject.Find("Generated Level");
           // aimed.SetActive(false);
        }


        private IEnumerator GeneratorCoroutine(PlatformerGeneratorGrid2D generator)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var generatorCoroutine = this.StartSmartCoroutine(generator.GenerateCoroutine());

            yield return generatorCoroutine.Coroutine;

            yield return null;

            stopwatch.Stop();

            isGenerating = false;

            // Throw an exception if the coroutine was not successful.
            // The point of this custom coroutine is that you can actually catch the exception (unlike with the default coroutines).
            // It makes it possible to run the generator again if needed while still having coroutines and not blocking the main thread.
            generatorCoroutine.ThrowIfNotSuccessful();

            generatorElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            yield return new WaitForSeconds(1f); // 等待1秒
            HideLoadingScreen();
           // Invoke("solvecharu", 1f);
        }

        private void RefreshLevelInfo()
        {
            SetLevelInfo($"Generated in {generatorElapsedMilliseconds / 1000d:F}s\nLevel type: {LevelType}");
        }

        public bool LevelMapSupported()
        {
            var layer = LayerMask.NameToLayer(LevelMapLayer);

            if (layer == -1)
            {
                Debug.Log($"Level map is currently not supported. Please add a layer called \"{LevelMapLayer}\" to enable this feature and then restart the game.");
            }

            return layer != -1;
        }

        public void solvecharu()
        {
            Debug.Log("kaishixiaochu");
            GameObject aimed = GameObject.Find("Generated Level");
            Transform tilemaps = aimed.transform.Find("Tilemaps");
            GameObject wall = tilemaps.transform.Find("Walls").gameObject;
            CompositeCollider2D compCollider = wall.GetComponent<CompositeCollider2D>();

            if (compCollider != null)
            {
                // 方法1：禁用组件（可重新启用）
                Destroy(compCollider);

            }
        }

    }


   

}