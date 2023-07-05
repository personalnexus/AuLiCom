

# Command Syntax

The command-line interface and the `AuLiComExecuteCommand` function in Excel let you control channel values using a simple command syntax.

Commands consist of two parts separated by the **@** character. The first part specifies the channels to adjust. The second part defines what kind of adjustment to make. Inputs are trimmed, so whitespace at the beginning, end and around separators is ignored.

The channel specification consists of one or more channel sections separated by the **+** character. Each channel section can be a:
 - **range of channel numbers** e.g. "6-11", where start and end are separated by the **-** character,
 - **channel number** e.g. "4"
 - **search sub-string** e.g. "red". The command executor will then search for this substring (ignoring case) in all registered fixtures' names, channel names, and aliases. All channels that match will be adjusted by the command.

The adjustment can be one of these:
- **absolute value** e.g. "5" to set all channels to the same value of 5%.
- **add percentage** e.g. "+10" to add 10 percentage points to all channel values. Values cannot go higher than 100%.
- **subtract percentage** e.g. "-15" to subtract 15 percentage points from all channel values. Values cannot go lower than 0%.
- **multiply** e.g. "*3" to triple channel values. Values are rounded, but cannot go higher than 100%.
- **divide** e.g. "/2" to halve channel values. Values are rounded, but cannot go lower than 0%.
- **empty** (i.e. there is nothing but whitespace after the **@**) to set channels to 100%.

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
- ``Red @ 90``	set channels on fixtures where channel name, fixture name or lias contains 'red' (ignoring case) to 90%
- ``31-33 @``	set channels 31 through 33 to 100%