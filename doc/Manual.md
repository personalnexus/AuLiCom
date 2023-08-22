# Objects

**Channels** are the most basic object you can control. Each channel can have a 1-byte value (0..255) e.g. representing the brightness of a light. In most user interface elements, the value is expressed as a percentage (0%..100%).

**Chasers** consist of a name and one or more scene names. There are different types of chasers (random, back and forth...). Scenes are resolved by name only when the chaser is playing. Meaning you can modify a scene that is already part of the chaser without having to modify the chaser explicitly. Since th chaser stored only the scene name, not its contents, it will "see" the new scnene contents when it starts playing.

**Colors** give a name to three channel values representing red, green and blue. When assigning a fixture a color (either when setting a channel value via a command or via a scene), the fixture's red, green and blue channels are given the color's values. This allows for the central definition of colors to be used in multiple scenes.

**Fixtures** represent lighting devices with one channel (a classic stage light) or more *channels* (e.g. LED). Support for each fixture type is built into AuLiCom. While technically it would be enough to have a single generic lamp for all 1-channel applications, there are multiple identical types named after the actual types of fixtures out there (PAR64, Cantata PC...)

**Scenes** consist of a name and one or more saved channel values. You can use the scene player to switch between scenes, optionally with a fade time to seemlessly fade each channel.


# Command Syntax

The command-line interface and the `AuLiComExecuteCommand` function in Excel let you control channel values using a simple command syntax.

Usually, commands consist of two parts separated by the **@** character. The first part specifies the channels to adjust. The second part defines what kind of adjustment to make. Inputs are trimmed, so whitespace at the beginning, end and around separators is ignored.

One exception, where a command does not have to contain the **@** character: when the entire command is the  single **\*** character, this repeats the entire previous command (see examples below).

The channel specification usually consists of one or more channel sections separated by the **+** character. Each channel section can be a:
 - **range of channel numbers** e.g. "6-11", where start and end are separated by the **-** character,
 - **channel number** e.g. "4"
 - **search sub-string** e.g. "red". The command executor will then search for this substring (ignoring case) in all registered fixtures' names, channel names, and aliases. All channels that match will be adjusted by the command.

 One exception for channel specifications is a single **\*** character which uses the same channels as in the previous command, e.g. for two incremental changes to many channels where you do not want to retype all channels (see examples below).

The adjustment can be one of these:
- **absolute value** e.g. "5" to set all channels to the same value of 5%.
- **add percentage** e.g. "+10" to add 10 percentage points to all channel values. Values cannot go higher than 100%.
- **subtract percentage** e.g. "-15" to subtract 15 percentage points from all channel values. Values cannot go lower than 0%.
- **multiply** e.g. "*3" to triple channel values. Values are rounded, but cannot go higher than 100%.
- **divide** e.g. "/2" to halve channel values. Values are rounded, but cannot go lower than 0%.
- **color** a user-defined color name (ignoring case)
- **@** to set channels to 100%.

## Examples

The following is an incomplete list of how different channel sections and adjustments can be combined to easily change many channels at once:

- ``1 @ +10``	increase channel 1 by 10 percentage points
- ``2+3 @ -15``	lower channels 2 and 3 by 15 percentage points
- ``4-7 @ *3``	triple the percentage values of channels 4 through 7
- ``8-9 + 10 @ /2``	halve the percentage values of channels 8 through 9 and 0
- ``11 @ 50``	set channel 11 to 50%
- ``12-15 @ 60``	set channels 12 through 15 to 60%
- ``16 + 19 @ 70``	set channels 16 and 19 to 70%
- ``20-25 + 27-30 @ 80``	set channels 20 through 25 and 27 through 30 to 80% 
- ``Red @ 90``	set channels on fixtures where channel name, fixture name or alias contains 'red' (ignoring case) to 90%
- ``31-33 @ @``	set channels 31 through 33 to 100%
- ``*@+5``	following the command ``1-2+4+7-9@75`` results in the channels set to 75 after the first step and then have another 5 percentage points added.
- ``*``	following the command ``red@+10`` results in all red channels to be adjusted up by 10 percentage points twice. If this was too much of an upward adjustment ``*@-5`` could be executed next to adjust the channels back down by 5 percentage points again.
- ``35 @ orange`` if channel 35 belongs to an RGB fixture and you have previously defined the color "orange" to have specific RGB values, these values will be applied to all three RGB channels (not just 35).
- ``LED1 @ orange`` if LED1 is part of the name of an RGB fixture and you have previously defined the color "orange" to have specific RGB values, these values will be applied to all three RGB channels of the LED1 fixture.

## Ease of Use

To make entering the <kbd>@</kbd> character easier, I have remapped it to <kbd>NumLock</kbd> using [PowerToys Keyboard Manager](https://learn.microsoft.com/en-us/windows/powertoys/keyboard-manager). This way, all the keys needed to enter basic commands are available on the keypad for single handed use and I only need to use my second hand with the keyboard when entering channel/fixture names.
