Thanks for purchasing Pixel Monsters!

Each monster contains:
1) PNG spritesheet
2) Sprite Library Asset that defines a list of animations and a list of frames for each animation
3) Prefab

The full list of monsters animations available:
1) Idle
2) Ready
3) Walk/Run/Jump
4) Attack
5) Die
6) Fire (optional)

The full list of objects available:
1) Idle
2) Destroy (optional)
3) Open (optional)

All monsters/objects use the same unified animation controller. Animation transitions are managed by animation parameters (please refer to the Animation window).

Each monster/object has the [Monster]/[Creature] component, you can use it to play animations. Please refer to the MonsterState enum.
Each monster/object also comes with the [MonsterControls]/[InanimateControls] component for keyboard interaction.
You can use the binding of [MonsterControls]/[InanimateControls] and [MonsterController2D] as an example, or replace it by your own implementation.

In case if you need more info, or if you have any questions, please contact me on Discord using the link https://discord.gg/4ht2AhW