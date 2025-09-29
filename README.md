# Sprinter

Sprinter is a rework of [HeeliesMod](https://www.nexusmods.com/stardewvalley/mods/7751) originally created by [fakemonster](https://github.com/fakemonster). This mod preserves the original concept of activating a speed boost with a key press, while improving animation behavior and adding a visual dirt kick-up effect to enhance the sense of motion. I made this mod because I love the original, but dislike seeing the character freeze.

![sprinter](https://i.imgur.com/iIbgyps.gif)

## Features

- Activate a speed boost with a configurable key (default: Space)
- Preserves natural walking and running animations
- Adds a dirt kick-up visual effect behind the player while rolling
- Gradual speed decay over time
- Configurable speed and stamina cost

## Installation

1. Install [SMAPI](https://smapi.io)
2. Download and extract this mod into your `Mods` folder
3. Launch the game

## Configuration

After launching the game once with the mod installed, a `config.json` file will be generated in the mod's folder. You can edit the following options:

- `heeliesButton`: The key used to activate the speed boost. Default is `"Space, RightShoulder"`. See [Stardew Valley Wiki - Button Codes](https://stardewvalleywiki.com/Modding:Player_Guide/Key_Bindings#Button_codes) for valid options. If you're playing with a gamepad, I recommend using Steam Input or some other rebinding software to map RightShoulder to Space, so your inventory wont swap when you activate it.

- `initialSpeedBoost`: The initial speed value applied when activating the boost. Default is `3.5`. Higher values result in faster movement.

- `staminaCost`: The amount of stamina consumed when activating the boost. Default is `0`. Go Usain mode!

## Credits

All original credit goes to fakemonster/moonbaseboss for the concept and implementation. This version was reworked by Jevon Lipsey, only adding modifications that preserve animations and add visual effects.
You can find the original repo [here](https://github.com/fakemonster/stardew-valley-heelies).

## Links

This mod is also posted on [Nexus Mods](https://www.nexusmods.com/stardewvalley/mods/37986).

## License

This project is licensed under the GNU Lesser General Public License v3.0 (LGPL-3.0). See the `LICENSE` file for details.
